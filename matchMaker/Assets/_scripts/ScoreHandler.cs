using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private GridConfigSO _gridConfig;

    [SerializeField, Header("listening to"), Space(2)]
    private StringEC_SO _cardnameChannel;
    [SerializeField, Header("broadcasting on"), Space(2)]
    private BoolEC_SO _evalChannel, _gameOverChannel;
    [SerializeField]
    private int _comboCount;
    [SerializeField]
    private string _firstSelected;

    private void Awake()
    {
        _comboCount = 1;
        _firstSelected = null;
    }
    private void OnEnable()
    {
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

                SaveDataHandler.CurrentStateData.UpdatePlayerData(_comboCount);
                _comboCount++;

                _evalChannel.RaiseEvent(true);
            }

            else
            {
                _evalChannel.RaiseEvent(false);
                _comboCount = 1;
            }

            _firstSelected = null;
        }

        if (SaveDataHandler.CurrentStateData.MoveCount >= (SaveDataHandler.CurrentStateData.ColCount * SaveDataHandler.CurrentStateData.RowCount) / 2)
        {
            _gameOverChannel.RaiseEvent(true);
        }
    }

    private void OnDisable()
    {
        _cardnameChannel.OnEventRaised -= CardnameChannel_OnEventRaised;
    }
}
