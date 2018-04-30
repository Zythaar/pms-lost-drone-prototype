using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TopDownScroller.Starcraft.Data
{
	/// <summary>
	/// The asset which holds the list of different starcrafts
	/// </summary>
	[CreateAssetMenu(fileName = "StarcraftLibrary.asset", menuName = "TopDownScroller/Starcraft Library", order = 1)]
	public class StarcraftLibrary : ScriptableObject, IList<Starcraft>, IDictionary<string, Starcraft>
	{
        /// <summary>
        /// The list of all the starcrafts
        /// </summary>
        public List<Starcraft> configurations;

        /// <summary>
        /// The internal reference to the dictionary made from the list of starcrafts
        /// with the name of starcrafts as the key
        /// </summary>
        Dictionary<string, Starcraft> m_ConfigurationDictionary;

        /// <summary>
        /// The accessor to the starcrafts by index
        /// </summary>
        /// <param name="index"></param>
        public Starcraft this[int index]
		{
			get { return configurations[index]; }
		}

		public void OnBeforeSerialize()
		{
		}

		/// <summary>
		/// Convert the list (m_Configurations) to a dictionary for access via name
		/// </summary>
		public void OnAfterDeserialize()
		{
			if (configurations == null)
			{
				return;
			}
			m_ConfigurationDictionary = configurations.ToDictionary(t => t.towerName);
		}

		public bool ContainsKey(string key)
		{
			return m_ConfigurationDictionary.ContainsKey(key);
		}

		public void Add(string key, Starcraft value)
		{
			m_ConfigurationDictionary.Add(key, value);
		}

		public bool Remove(string key)
		{
			return m_ConfigurationDictionary.Remove(key);
		}

		public bool TryGetValue(string key, out Starcraft value)
		{
			return m_ConfigurationDictionary.TryGetValue(key, out value);
		}

		Starcraft IDictionary<string, Starcraft>.this[string key]
		{
			get { return m_ConfigurationDictionary[key]; }
			set { m_ConfigurationDictionary[key] = value; }
		}

		public ICollection<string> Keys
		{
			get { return ((IDictionary<string, Starcraft>) m_ConfigurationDictionary).Keys; }
		}

		ICollection<Starcraft> IDictionary<string, Starcraft>.Values
		{
			get { return m_ConfigurationDictionary.Values; }
		}

		IEnumerator<KeyValuePair<string, Starcraft>> IEnumerable<KeyValuePair<string, Starcraft>>.GetEnumerator()
		{
			return m_ConfigurationDictionary.GetEnumerator();
		}

		public void Add(KeyValuePair<string, Starcraft> item)
		{
			m_ConfigurationDictionary.Add(item.Key, item.Value);
		}

		public bool Remove(KeyValuePair<string, Starcraft> item)
		{
			return m_ConfigurationDictionary.Remove(item.Key);
		}

		public bool Contains(KeyValuePair<string, Starcraft> item)
		{
			return m_ConfigurationDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, Starcraft>[] array, int arrayIndex)
		{
			int count = array.Length;
			for (int i = arrayIndex; i < count; i++)
			{
				Starcraft config = configurations[i - arrayIndex];
				KeyValuePair<string, Starcraft> current = new KeyValuePair<string, Starcraft>(config.towerName, config);
				array[i] = current;
			}
		}

		public int IndexOf(Starcraft item)
		{
			return configurations.IndexOf(item);
		}

		public void Insert(int index, Starcraft item)
		{
			configurations.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			configurations.RemoveAt(index);
		}

		Starcraft IList<Starcraft>.this[int index]
		{
			get { return configurations[index]; }
			set { configurations[index] = value; }
		}

		public IEnumerator<Starcraft> GetEnumerator()
		{
			return configurations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) configurations).GetEnumerator();
		}

		public void Add(Starcraft item)
		{
			configurations.Add(item);
		}

		public void Clear()
		{
			configurations.Clear();
		}

		public bool Contains(Starcraft item)
		{
			return configurations.Contains(item);
		}

		public void CopyTo(Starcraft[] array, int arrayIndex)
		{
			configurations.CopyTo(array, arrayIndex);
		}

		public bool Remove(Starcraft item)
		{
			return configurations.Remove(item);
		}

		public int Count
		{
			get { return configurations.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<Starcraft>) configurations).IsReadOnly; }
		}
	}
}