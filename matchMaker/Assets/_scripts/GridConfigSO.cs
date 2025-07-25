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
    public List<int> MatchList = new();

    public Dictionary<string, int> CardDict;
    //public List<KeyValuePair<string, int>> Cards;

    public void AddElements()
    {
        MatchList = new List<int>();

        for (int i = 1; i <= RowCount * ColCount; i++)
        {
            MatchList.Add(Mathf.RoundToInt((i + 1) / 2));
        }

        CardDict = new Dictionary<string, int>();

        //Shuffle draw order (Marble Bag)
        for (int i = 0; i < RowCount * ColCount; i++)
        {
            int index = Random.Range(0, MatchList.Count);
            var element = MatchList[index];
            string cardName = "card" + i;
            MatchList.RemoveAt(index);


            CardDict.Add(cardName, element);
        }
    }

    public void AddElements(int rowCount, int colCount)
    {
        RowCount = rowCount; 
        ColCount = colCount;

        MatchList = new List<int>();

        for (int i = 1; i <= RowCount * ColCount; i++)
        {
            MatchList.Add(Mathf.RoundToInt((i + 1) / 2));
        }

        CardDict = new Dictionary<string, int>();

        //Shuffle draw order (Marble Bag)
        for (int i = 0; i < RowCount * ColCount; i++)
        {
            int index = Random.Range(0, MatchList.Count);
            var element = MatchList[index];
            string cardName = "card" + i;
            MatchList.RemoveAt(index);


            CardDict.Add(cardName, element);
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

    //public int GetNextElement(string cardName)
    //{
    //    if (MatchList.Count > 0)
    //    {
    //        int index = Random.Range(0, MatchList.Count);
    //        var element = MatchList[index];
    //        MatchList.RemoveAt(index);

    //        CardDict.Add(cardName, element);
    //        return element;
    //    }

    //    else
    //    {
    //        Debug.Log("Match List empty!", this);
    //        return 0;
    //    }
    //}

    public List<KeyValuePair<string, int>> ReadDict()
    {
        if (CardDict != null && CardDict.Count > 0)
        {
            List<KeyValuePair<string, int>> cards = new List<KeyValuePair<string, int>>();
            int count = 0;

            foreach (var item in CardDict)
            {
                KeyValuePair<string, int> pair = new(item.Key, item.Value, "element" + count);
                cards.Add(pair);
                count++;
            }

            return cards;
        }

        else
        {
            return null;
        }
    }

    public bool WriteToDict(List<KeyValuePair<string, int>> cards)
    {
        if (cards.Count > 0 && cards[0] != null)
        {
            CardDict = new Dictionary<string, int>();
            int count = 0;

            for (int i = 0; i < cards.Count; i++)
            {
                if (CardDict.TryAdd(cards[i].Key, cards[i].Value))
                {
                    count++;
                }
            }

            return count == cards.Count;
        }

        else
        {
            return false;
        }
    }
}
