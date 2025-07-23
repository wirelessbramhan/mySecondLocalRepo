using UnityEngine;

public class GridView : MonoBehaviour
{
    [SerializeField] private ViewDataSO _viewData;
    [SerializeField] private RectTransform _sectionPrefab;
    [SerializeField] private GridCell _cell;
    // Start is called before the first frame update
    void Start()
    {
        int count = 0;

        for (int i = 0; i < _viewData.RowCount; i++)
        {
            RectTransform section = Instantiate(_sectionPrefab, transform);
            section.gameObject.name = "row" + i;

            for(int j = 0; j < _viewData.ColCount; j++)
            {
                GridCell cell = Instantiate(_cell, section.transform);
                
                cell.Init("cell" + count);
                count++;
            }
        }
    }
}
