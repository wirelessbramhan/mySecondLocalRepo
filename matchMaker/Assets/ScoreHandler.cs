using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private GridConfigSO _gridConfig;
    [SerializeField, Header("listening to"), Space(2)]
    private FloatEC_SO _valueChannel;
    [SerializeField, Header("broadcasting on"), Space(2)]
    private BoolEC_SO _evalChannel;
    [SerializeField]
    private int _firstValue, _count;
    private void OnEnable()
    {
        _valueChannel.OnEventRaised += ValueChannel_OnEventRaised;
        _count = 0;
        _firstValue = 0;
    }

    private void ValueChannel_OnEventRaised(float obj)
    {
        _count++;

        if (_count % 2 != 0)
        {
            _firstValue = (int)obj;
        }

        else
        {
            if (_firstValue == (int)obj)
            {
                Debug.Log("correct choice!");
            }

            else
            {
                Debug.Log("wrong choice!");
            }

            _evalChannel.RaiseEvent(_firstValue == (int)obj);
        }
    }

    private void OnDisable()
    {
        _valueChannel.OnEventRaised -= ValueChannel_OnEventRaised;
    }
}
