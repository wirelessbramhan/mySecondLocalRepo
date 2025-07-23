using System.Collections;
using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField] private ViewDataSO _viewData;
    [SerializeField] private GridConfigSO _config;
    [SerializeField] private RectTransform _sectionPrefab;
    [SerializeField] private GridCell _cell;
    // Start is called before the first frame update
    
    IEnumerator Start()
    {
        //Init Config Data
        _config.AddElements();
        //number cells for matching
        int count = 0;

        //Spawn Cells, adding to config
        for (int i = 0; i < _config.RowCount; i++)
        {
            RectTransform section = Instantiate(_sectionPrefab, transform);
            section.gameObject.name = "row" + i;

            for (int j = 0; j < _config.ColCount; j++)
            {
                GridCell cell = Instantiate(_cell, section.transform);

                cell.Init(_config.GetNextElement(), false);
                count++;
            }
        }

        yield return null;
    }
}
