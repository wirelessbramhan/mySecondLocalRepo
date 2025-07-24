using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class GridView : MonoBehaviour
{
    [SerializeField] private ViewDataSO _viewData;
    [SerializeField] private GridConfigSO _config;
    [SerializeField] private RectTransform _sectionPrefab;
    [SerializeField] private GridCell _cell;
    private CanvasGroup _group;
    [SerializeField] private float _hideDelay = 5.0f;

#if UNITY_EDITOR

    //OnValidate should not be triggered outside editor, too mnay redundant calls
    private void OnValidate()
    {
        Setup();
    }

#endif

    public void SpawnCards()
    {
        StartCoroutine(StartSpawn());
    }

    private IEnumerator StartSpawn()
    {
        Init();
        yield return new WaitForSeconds(_hideDelay);
        _group.interactable = true;
    }

    /// <summary>
    /// Single function call for settting up UI in Editor
    /// </summary>
    [ContextMenu("Initialize View")]
    private void Init()
    {
        Setup();
        Configure();
    }

    protected virtual void Configure()
    {
        LoadCards();
    }

    private void LoadCards()
    {
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
                string cardName = "card" + count;
                string cardValue = _config.GetElement(cardName).ToString();

                cell.gameObject.name = cardName;
                cell.Init(cardValue, false, _hideDelay);
                count++;
            }
        }
    }

    protected virtual void Setup()
    {
        if (_viewData)
        {
            //Use ViewData for UI config, happy case before trying to grab the component
            if (TryGetComponent<VerticalLayoutGroup>(out var rootVLG))
            {
                rootVLG.padding = _viewData.Padding;
                rootVLG.spacing = _viewData.Spacing;

                for (int i = 0; i < rootVLG.transform.childCount; i++)
                {
                    if (rootVLG.transform.GetChild(i).TryGetComponent<HorizontalLayoutGroup>(out var section))
                    {
                        section.spacing = _viewData.Spacing;
                    }
                }
            }
        }

        else
        {
            Debug.Log("ViewData not assigned", this);
        }

        if (!TryGetComponent(out _group))
        {
            Debug.Log("canvasGroup not found", this);
        }

        else
        {
            _group.alpha = 1;
            _group.blocksRaycasts = true;
            _group.interactable = false;
        }
    }
}
