using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyValuePair<T1, T2>
{
    public string Name;
    public T1 Key;
    public T2 Value;

    public KeyValuePair(T1 key, T2 value, string name)
    {
        Key = key;
        Value = value;
        Name = name;
    }
}

[CreateAssetMenu]
public class GridConfigSO : ScriptableObject
{
    public int RowCount, ColCount;
    public List<int> MatchList;

    public Dictionary<string, int> _cards;
    public List<KeyValuePair<string, int>> _cardsDict;

    public void AddElements()
    {
        MatchList.Clear();

        for (int i = 1; i < RowCount * ColCount + 1; i++)
        {
            MatchList.Add(Mathf.RoundToInt((i + 1) / 2));
        }
    }

    public int GetNextElement(string cardName)
    {
        if (MatchList.Count > 0)
        {
            int index = Random.Range(0, MatchList.Count);
            var element = MatchList[index];
            MatchList.RemoveAt(index);

            _cards.Add(cardName, element);
            return element;
        }

        else
        {
            Debug.Log("Match List empty!", this);
            return 0;
        }
    }

    public bool ShowDict()
    {
        if (_cards != null && _cards.Count > 0)
        {
            _cardsDict.Clear();
            int count = 0;

            foreach (var item in _cards)
            {
                KeyValuePair<string, int> pair = new(item.Key, item.Value, "element" + count);
                _cardsDict.Add(pair);
                count++;
            }
        }

        if (_cardsDict.Count == _cards.Count)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
