using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private GridConfigSO _gridConfig;
    [SerializeField, Header("listening to"), Space(2)]
    private StringEC_SO _cardnameChannel;
    [SerializeField, Header("broadcasting on"), Space(2)]
    private BoolEC_SO _evalChannel;
    [SerializeField]
    private int _count, _value;
    [SerializeField]
    private string _first;

    private void OnEnable()
    {
        _cardnameChannel.OnEventRaised += CardChannel_OnEventRaised;
        _count = 0;
    }

    private void CardChannel_OnEventRaised(string cardName)
    {
        _count++;

        if (_count % 2 != 0)
        {
            if (_gridConfig._cards.ContainsKey(cardName))
            {
                _first = cardName;
            }
        }
        
        else
        {
            if (_gridConfig._cards.ContainsKey(cardName))
            {
                _evalChannel.RaiseEvent(_gridConfig._cards[_first] == _gridConfig._cards[cardName]);
            }
        }
        
    }

    private void OnDisable()
    {
        _cardnameChannel.OnEventRaised -= CardChannel_OnEventRaised;
    }

    //private IEnumerator StartComparison(string first)
    //{
    //    yield return new WaitUntil(() => _value != string.Empty);

    //    if (_value == first)
    //    {
            
    //    }

    //    _evalChannel.RaiseEvent(_value == first);
    //}
}
