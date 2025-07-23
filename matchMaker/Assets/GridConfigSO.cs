using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GridConfigSO : ScriptableObject
{
    public int RowCount, ColCount;
    public List<int> MatchList;
    public List<int> RandomList;

    public void AddElements()
    {
        MatchList.Clear();
        RandomList.Clear();

        for (int i = 1; i < RowCount * ColCount + 1; i++)
        {
            MatchList.Add(Mathf.RoundToInt((i + 1) / 2));
        }
    }

    public int GetNextElement()
    {
        if (MatchList.Count > 0)
        {
            int index = Random.Range(0, MatchList.Count);
            var element = MatchList[index];
            MatchList.RemoveAt(index);
          
            RandomList.Add(element);
            return element;
        }

        else
        {
            Debug.Log("Match List empty!", this);
            return 0;
        }
    }
}
