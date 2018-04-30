using System;
using Core.Economy;
using Core.Health;
using Core.Utilities;
using TopDownScroller.Economy;
using TopDownScroller.Starcraft.Data;
using UnityEngine;

namespace TopDownScroller.Level
{
	/// <summary>
	/// The level manager - handles the level states and tracks the player's currency
	/// </summary>
	[RequireComponent(typeof(WaveManager))]
	public class LevelManager : Singleton<LevelManager>
	{
		/// <summary>
		/// The configured level intro. If this is null the LevelManager will fall through to the gameplay state (i.e. SpawningEnemies)
		/// </summary>
		public LevelIntro intro;

		/// <summary>
		/// The starcrafft library for this level
		/// </summary>
		public StarcraftLibrary starcraftLibrary;

		/// <summary>
		/// The currency that the player starts with
		/// </summary>
		public int startingCurrency;

		/// <summary>
		/// The controller for gaining currency
		/// </summary>
		public CurrencyGainer currencyGainer;

		/// <summary>
		/// Configuration for if the player gains currency even in pre-upgrade phase
		/// </summary>
		[Header("Setting this will allow currency gain during the Intro and Pre-Upgrade phase")]
		public bool alwaysGainCurrency;

		/// <summary>
		/// The player starcraft that the player must defend
		/// </summary>
		public PlayerController[] starcrafts;

		public Collider[] environmentColliders;

		/// <summary>
		/// The attached wave manager
		/// </summary>
		public WaveManager waveManager { get; protected set; }

		/// <summary>
		/// Number of enemies currently in the level
		/// </summary>
		public int numberOfEnemies { get; protected set; }

		/// <summary>
		/// The current state of the level
		/// </summary>
		public LevelState levelState { get; protected set; }

		/// <summary>
		/// The currency controller
		/// </summary>
		public Currency currency { get; protected set; }

		/// <summary>
		/// Number of player lives left
		/// </summary>
		public int numberOfLivesLeft { get; protected set; }

		/// <summary>
		/// Starting number of lives
		/// </summary>
		public int numberOfLives { get; protected set; }

        /// <summary>
        /// An accessor for the starcrafts
        /// </summary>
        public PlayerController[] playerStarcrafts
        {
            get { return starcrafts; }
        }

        /// <summary>
        /// If the game is over
        /// </summary>
        public bool isGameOver
		{
			get { return (levelState == LevelState.Win) || (levelState == LevelState.Lose); }
		}

		/// <summary>
		/// Fired when all the waves are done and there are no more enemies left
		/// </summary>
		public event Action levelCompleted;

		/// <summary>
		/// Fired when all of the player starcrafts are destroyed
		/// </summary>
		public event Action levelFailed;

		/// <summary>
		/// Fired when the level state is changed - first parameter is the old state, second parameter is the new state
		/// </summary>
		public event Action<LevelState, LevelState> levelStateChanged;

		/// <summary>
		/// Fired when the number of enemies has changed
		/// </summary>
		public event Action<int> numberOfEnemiesChanged;

		/// <summary>
		/// Event for player starcraft being destroyed
		/// </summary>
		public event Action playerStarcraftDestroyed;

		/// <summary>
		/// Increments the number of enemies. Called on Agent spawn
		/// </summary>
		public virtual void IncrementNumberOfEnemies()
		{
			numberOfEnemies++;
			SafelyCallNumberOfEnemiesChanged();
		}

		/// <summary>
		/// Returns the sum of all HomeBases' health
		/// </summary>
		public float GetAllStarcraftsHealth()
		{
			float health = 0.0f;
            foreach (PlayerController starcraft in starcrafts)
            {
                health += starcraft.configuration.currentHealth;
            }
            return health;
		}

		/// <summary>
		/// Decrements the number of enemies. Called on Agent death
		/// </summary>
		public virtual void DecrementNumberOfEnemies()
		{
			numberOfEnemies--;
			SafelyCallNumberOfEnemiesChanged();
			if (numberOfEnemies < 0)
			{
				Debug.LogError("[LEVEL] There should never be a negative number of enemies. Something broke!");
				numberOfEnemies = 0;
			}

			if (numberOfEnemies == 0 && levelState == LevelState.AllEnemiesSpawned)
			{
				ChangeLevelState(LevelState.Win);
			}
		}

		/// <summary>
		/// Completes upgrading phase, setting state to spawn enemies
		/// </summary>
		public virtual void UpgradingCompleted()
		{
			ChangeLevelState(LevelState.SpawningEnemies);
		}

		/// <summary>
		/// Caches the attached wave manager and subscribes to the spawning completed event
		/// Sets the level state to intro and ensures that the number of enemies is set to 0
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			waveManager = GetComponent<WaveManager>();
			waveManager.spawningCompleted += OnSpawningCompleted;

			// Does not use the change state function as we don't need to broadcast the event for this default value
			levelState = LevelState.Intro;
			numberOfEnemies = 0;

			// Ensure currency change listener is assigned
			currency = new Currency(startingCurrency);
			currencyGainer.Initialize(currency);

			// If there's an intro use it, otherwise fall through to gameplay
			if (intro != null)
			{
				intro.introCompleted += IntroCompleted;
			}
			else
			{
				IntroCompleted();
			}

            // Iterate through home bases and subscribe
            numberOfLives = starcrafts.Length;
            numberOfLivesLeft = numberOfLives;
            for (int i = 0; i < numberOfLives; i++)
            {
                starcrafts[i].died += OnPlayerStarcraftDestroyed;
            }
        }

		/// <summary>
		/// Updates the currency gain controller
		/// </summary>
		protected virtual void Update()
		{
			if (alwaysGainCurrency ||
			    (!alwaysGainCurrency && levelState != LevelState.Upgrading && levelState != LevelState.Intro))
			{
				currencyGainer.Tick(Time.deltaTime);
			}
		}

		/// <summary>
		/// Unsubscribes from events
		/// </summary>
		protected override void OnDestroy()
		{
			base.OnDestroy();
			if (waveManager != null)
			{
				waveManager.spawningCompleted -= OnSpawningCompleted;
			}
			if (intro != null)
			{
				intro.introCompleted -= IntroCompleted;
			}

            // Iterate through home bases and unsubscribe
            for (int i = 0; i < numberOfLives; i++)
            {
                starcrafts[i].died -= OnPlayerStarcraftDestroyed;
            }
        }

		/// <summary>
		/// Fired when Intro is completed or immediately, if no intro is specified
		/// </summary>
		protected virtual void IntroCompleted()
		{
			ChangeLevelState(LevelState.Upgrading);
		}

		/// <summary>
		/// Fired when the WaveManager has finished spawning enemies
		/// </summary>
		protected virtual void OnSpawningCompleted()
		{
			ChangeLevelState(LevelState.AllEnemiesSpawned);
		}

		/// <summary>
		/// Changes the state and broadcasts the event
		/// </summary>
		/// <param name="newState">The new state to transitioned to</param>
		protected virtual void ChangeLevelState(LevelState newState)
		{
			// If the state hasn't changed then return
			if (levelState == newState)
			{
				return;
			}

			LevelState oldState = levelState;
			levelState = newState;
			if (levelStateChanged != null)
			{
				levelStateChanged(oldState, newState);
			}
			
			switch (newState)
			{
				case LevelState.SpawningEnemies:
					waveManager.StartWaves();
					break;
				case LevelState.AllEnemiesSpawned:
					// Win immediately if all enemies are already dead
					if (numberOfEnemies == 0)
					{
						ChangeLevelState(LevelState.Win);
					}
					break;
				case LevelState.Lose:
					SafelyCallLevelFailed();
					break;
				case LevelState.Win:
					SafelyCallLevelCompleted();
					break;
			}
		}

		/// <summary>
		/// Fired when a home base is destroyed
		/// </summary>
		protected virtual void OnPlayerStarcraftDestroyed(DamageableBehaviour starcraft)
		{
			// Decrement the number of home bases
			numberOfLivesLeft--;

			// Call the destroyed event
			if (playerStarcraftDestroyed != null)
			{
				playerStarcraftDestroyed();
			}

			// If there are no home bases left and the level is not over then set the level to lost
			if ((numberOfLivesLeft == 0) && !isGameOver)
			{
				ChangeLevelState(LevelState.Lose);
			}
		}

		/// <summary>
		/// Calls the <see cref="levelCompleted"/> event
		/// </summary>
		protected virtual void SafelyCallLevelCompleted()
		{
			if (levelCompleted != null)
			{
				levelCompleted();
			}
		}

		/// <summary>
		/// Calls the <see cref="numberOfEnemiesChanged"/> event
		/// </summary>
		protected virtual void SafelyCallNumberOfEnemiesChanged()
		{
			if (numberOfEnemiesChanged != null)
			{
				numberOfEnemiesChanged(numberOfEnemies);
			}
		}

		/// <summary>
		/// Calls the <see cref="levelFailed"/> event
		/// </summary>
		protected virtual void SafelyCallLevelFailed()
		{
			if (levelFailed != null)
			{
				levelFailed();
			}
		}
	}
}