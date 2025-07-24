using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class GridView : MonoBehaviour
{
    [SerializeField] private ViewDataSO _viewData;
    [SerializeField] private GridConfigSO _config;
    [SerializeField] private RectTransform _sectionPrefab;
    [SerializeField] private GridCell _cell;
    
#if UNITY_EDITOR

    //OnValidate should not be triggered outside editor, too mnay redundant calls
    private void OnValidate()
    {
        Setup();
    }

#endif

    

    private IEnumerator Start()
    {
        Init();
        yield return null;
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
        //Init Config Data
        _config.AddElements();
        _config._cards = new Dictionary<string, int>();
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
                string cardValue = _config.GetNextElement(cardName).ToString();
                
                cell.gameObject.name = cardName;
                cell.Init(cardValue, false, 10.0f);
                count++;
            }
        }

        if (!_config.ShowDict())
        {
            Debug.Log("Dict not serialized!!");
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
    }

    #region old code

    //protected virtual void Configure()
    //{
    //    //Init Config Data
    //    _config.AddElements();
    //    _config._cards = new Dictionary<string, int>();
    //    //number cells for matching
    //    int count = 0;

    //    //Spawn Cells, adding to config
    //    for (int i = 0; i < _config.RowCount; i++)
    //    {
    //        RectTransform section = Instantiate(_sectionPrefab, transform);
    //        section.gameObject.name = "row" + i;

    //        for (int j = 0; j < _config.ColCount; j++)
    //        {
    //            GridCell cell = Instantiate(_cell, section.transform);
    //            string cardName = "card" + count;
    //            string cardValue = _config.GetNextElement(cardName).ToString();


    //            //insert value as key first
    //            if (_config._cards.ContainsKey(cardValue))
    //            {
    //                if (_config._cards.Remove(cardValue, out string first))
    //                {
    //                    if (_config._cards.TryAdd(first, cardName))
    //                    {
    //                        Debug.Log(_config._cards.Count);
    //                    }
    //                }
    //            }

    //            else
    //            {
    //                _config._cards.Add(cardValue, cardName);
    //            }

    //            cell.gameObject.name = cardName;
    //            cell.Init(cardValue, false);
    //            count++;
    //        }
    //    }

    //    if (!_config.ShowDict())
    //    {
    //        Debug.Log("Dict not serialized!!");
    //    }
    //}

    #endregion
}
