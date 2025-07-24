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
    private List<int> MatchList;

    public Dictionary<string, int> CardDict;
    public List<KeyValuePair<string, int>> Cards;

    public void AddElements()
    {
        MatchList.Clear();

        for (int i = 1; i < RowCount * ColCount + 1; i++)
        {
            MatchList.Add(Mathf.RoundToInt((i + 1) / 2));
        }

        CardDict = new Dictionary<string, int>();

        //Shuffle draw order (Marble Bag)
        for (int i = 0; i < MatchList.Count; i++)
        {
            int index = Random.Range(0, MatchList.Count);
            var element = MatchList[index];
            MatchList.RemoveAt(index);

            CardDict.Add("card" + i, element);
        }
    }

    public int GetElement(string cardName)
    {
        if (CardDict.ContainsKey(cardName))
        {
            return CardDict[cardName];
        }

        return 0;
    }

    public int GetNextElement(string cardName)
    {
        if (MatchList.Count > 0)
        {
            int index = Random.Range(0, MatchList.Count);
            var element = MatchList[index];
            MatchList.RemoveAt(index);

            CardDict.Add(cardName, element);
            return element;
        }

        else
        {
            Debug.Log("Match List empty!", this);
            return 0;
        }
    }

    public bool ReadDict()
    {
        if (CardDict != null && CardDict.Count > 0)
        {
            Cards.Clear();
            int count = 0;

            foreach (var item in CardDict)
            {
                KeyValuePair<string, int> pair = new(item.Key, item.Value, "element" + count);
                Cards.Add(pair);
                count++;
            }
        }

        if (Cards.Count == CardDict.Count)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool WriteToDict()
    {
        CardDict = new Dictionary<string, int>();

        foreach (var card in Cards)
        {
            CardDict.Add(card.Key, card.Value);
        }

        if (Cards.Count == CardDict.Count)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
