using TMPro;
using UnityEngine;

public class TextHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _score, _moves;    

    private void Update()
    {
        if (SaveDataHandler.CurrentStateData != null)
        {
            _score.text = SaveDataHandler.CurrentStateData.Score.ToString();
            _moves.text = SaveDataHandler.CurrentStateData.MoveCount.ToString();
        }
    }
}
