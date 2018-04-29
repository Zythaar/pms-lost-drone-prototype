using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TopDownScroller.StarCraft.Data
{
	/// <summary>
	/// The asset which holds the list of different towers
	/// </summary>
	[CreateAssetMenu(fileName = "TowerLibrary.asset", menuName = "TowerDefense/Tower Library", order = 1)]
	public class TowerLibrary : ScriptableObject, IList<StarCraft>, IDictionary<string, StarCraft>
	{
		/// <summary>
		/// The list of all the towers
		/// </summary>
		public List<StarCraft> configurations;

		/// <summary>
		/// The internal reference to the dictionary made from the list of towers
		/// with the name of tower as the key
		/// </summary>
		Dictionary<string, StarCraft> m_ConfigurationDictionary;

		/// <summary>
		/// The accessor to the towers by index
		/// </summary>
		/// <param name="index"></param>
		public StarCraft this[int index]
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

		public void Add(string key, StarCraft value)
		{
			m_ConfigurationDictionary.Add(key, value);
		}

		public bool Remove(string key)
		{
			return m_ConfigurationDictionary.Remove(key);
		}

		public bool TryGetValue(string key, out StarCraft value)
		{
			return m_ConfigurationDictionary.TryGetValue(key, out value);
		}

		StarCraft IDictionary<string, StarCraft>.this[string key]
		{
			get { return m_ConfigurationDictionary[key]; }
			set { m_ConfigurationDictionary[key] = value; }
		}

		public ICollection<string> Keys
		{
			get { return ((IDictionary<string, StarCraft>) m_ConfigurationDictionary).Keys; }
		}

		ICollection<StarCraft> IDictionary<string, StarCraft>.Values
		{
			get { return m_ConfigurationDictionary.Values; }
		}

		IEnumerator<KeyValuePair<string, StarCraft>> IEnumerable<KeyValuePair<string, StarCraft>>.GetEnumerator()
		{
			return m_ConfigurationDictionary.GetEnumerator();
		}

		public void Add(KeyValuePair<string, StarCraft> item)
		{
			m_ConfigurationDictionary.Add(item.Key, item.Value);
		}

		public bool Remove(KeyValuePair<string, StarCraft> item)
		{
			return m_ConfigurationDictionary.Remove(item.Key);
		}

		public bool Contains(KeyValuePair<string, StarCraft> item)
		{
			return m_ConfigurationDictionary.Contains(item);
		}

		public void CopyTo(KeyValuePair<string, StarCraft>[] array, int arrayIndex)
		{
			int count = array.Length;
			for (int i = arrayIndex; i < count; i++)
			{
				StarCraft config = configurations[i - arrayIndex];
				KeyValuePair<string, StarCraft> current = new KeyValuePair<string, StarCraft>(config.towerName, config);
				array[i] = current;
			}
		}

		public int IndexOf(StarCraft item)
		{
			return configurations.IndexOf(item);
		}

		public void Insert(int index, StarCraft item)
		{
			configurations.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			configurations.RemoveAt(index);
		}

		StarCraft IList<StarCraft>.this[int index]
		{
			get { return configurations[index]; }
			set { configurations[index] = value; }
		}

		public IEnumerator<StarCraft> GetEnumerator()
		{
			return configurations.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable) configurations).GetEnumerator();
		}

		public void Add(StarCraft item)
		{
			configurations.Add(item);
		}

		public void Clear()
		{
			configurations.Clear();
		}

		public bool Contains(StarCraft item)
		{
			return configurations.Contains(item);
		}

		public void CopyTo(StarCraft[] array, int arrayIndex)
		{
			configurations.CopyTo(array, arrayIndex);
		}

		public bool Remove(StarCraft item)
		{
			return configurations.Remove(item);
		}

		public int Count
		{
			get { return configurations.Count; }
		}

		public bool IsReadOnly
		{
			get { return ((ICollection<StarCraft>) configurations).IsReadOnly; }
		}
	}
}