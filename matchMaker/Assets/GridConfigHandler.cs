using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridConfigHandler : MonoBehaviour
{
    Vector2 _gridStats;
    [SerializeField] private Button _playButton;
    [SerializeField] private TextMeshProUGUI _blockedText;
    [SerializeField] private GridConfigSO _gridConfig;

    public void SetRow(string row)
    {
        _gridStats.x = int.Parse(row);
    }

    public void SetColumn(string column)
    {
       _gridStats.y = int.Parse(column);
    }

    public void SetStats()
    {
        if ((_gridStats.x * _gridStats.y) % 2 == 0)
        {
            _gridConfig.RowCount = (int)_gridStats.x;
            _gridConfig.ColCount = (int)_gridStats.y;
        }

        _playButton.gameObject.SetActive((_gridStats.x * _gridStats.y) % 2 == 0);
        _blockedText.gameObject.SetActive(!((_gridStats.x * _gridStats.y) % 2 == 0));
    }
}
