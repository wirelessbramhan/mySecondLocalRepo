using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanelHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;

    // Update is called once per frame
    void Update()
    {
        if (SaveDataHandler.CurrentStateData != null && _score)
        {
            _score.text = "Score is : 0" + SaveDataHandler.CurrentStateData.Score;
        }
    }
}
