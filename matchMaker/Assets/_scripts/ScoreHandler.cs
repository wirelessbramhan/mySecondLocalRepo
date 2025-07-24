using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private GridConfigSO _gridConfig;
    [SerializeField] private SaveDataHandler _dataHandler;

    [SerializeField, Header("listening to"), Space(2)]
    private StringEC_SO _cardnameChannel;
    [SerializeField, Header("broadcasting on"), Space(2)]
    private BoolEC_SO _evalChannel;
    [SerializeField]
    private int _comboCount;
    [SerializeField]
    private string _firstSelected;

    private void Awake()
    {
        _comboCount = 0;
        _firstSelected = null;
    }
    private void OnEnable()
    {
        _evalChannel.OnEventRaised += EvalChannel_OnEventRaised;
        _cardnameChannel.OnEventRaised += CardnameChannel_OnEventRaised;
    }

    private void CardnameChannel_OnEventRaised(string obj)
    {
        if (_firstSelected == null)
        {
            _firstSelected = obj;
        }

        else
        {
            if (_gridConfig.CardDict[_firstSelected] == _gridConfig.CardDict[obj])
            {
                _gridConfig.CardDict[_firstSelected] = -1;
                _gridConfig.CardDict[obj] = -1;
                _evalChannel.RaiseEvent(true);
            }

            else
            {
                _evalChannel.RaiseEvent(false);
            }

            _firstSelected = null;
        }
    }

    private void EvalChannel_OnEventRaised(bool obj)
    {
        if (obj)
        {
            _comboCount++;
        }
    }

    private void OnDisable()
    {
        _evalChannel.OnEventRaised -= EvalChannel_OnEventRaised;
    }
}
