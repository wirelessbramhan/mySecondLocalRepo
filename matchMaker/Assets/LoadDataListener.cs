using UnityEngine;
using UnityEngine.UI;

public class LoadDataListener : MonoBehaviour
{
    [SerializeField] private BoolEC_SO _loadDataChannel;
    [SerializeField] private Button _loadButton, _startButton;

    private void OnEnable()
    {
        _loadDataChannel.OnEventRaised += LoadDataChannel_OnEventRaised;
    }

    private void OnDisable()
    {
        _loadDataChannel.OnEventRaised -= LoadDataChannel_OnEventRaised;
    }

    private void LoadDataChannel_OnEventRaised(bool obj)
    {
        Debug.Log("load reports : " +  obj);
        _loadButton.gameObject.SetActive(obj);
        _startButton.gameObject.SetActive(!obj);
    }
}
