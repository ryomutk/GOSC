using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

[Serializable]
public class SerializableDictionary<K, V>
{
    [Serializable]
    class SerializableKeyValuePair
    {
        public K key;
        public V value;
    }

    [SerializeField] List<SerializableKeyValuePair> keyValuePairs = new List<SerializableKeyValuePair>();
    public ReadOnlyCollection<K> keys { get { return _keys.AsReadOnly(); } }
    public ReadOnlyCollection<V> values { get { return _values.AsReadOnly(); } }
    List<K> _keys = new List<K>();
    List<V> _values = new List<V>();


    public V this[K key]
    {
        get
        {
            if (!keyValuePairs.Any( x => x.key.Equals(key)))
            {
                throw new KeyNotFoundException();
            }

            return keyValuePairs.Find(x => x.key.Equals(key)).value;
        }
        set
        {
            if (keyValuePairs.Any(x => x.key.Equals(key)))
            {
                keyValuePairs.Find(x => x.key.Equals(key)).value = value;
            }
            else
            {
                _keys.Add(key);
                _values.Add(value);
                keyValuePairs.Add(new SerializableKeyValuePair() {key = key,value = value });
            }
        }
    }
    
    public IEnumerator<KeyValuePair<K,V>> GetEnumerator()
    {
        Dictionary<K,V> tmp = new Dictionary<K, V>();

        foreach(var kvp in keyValuePairs)
        {
            tmp[kvp.key] = kvp.value;
        }

        return tmp.GetEnumerator();
    } 
}