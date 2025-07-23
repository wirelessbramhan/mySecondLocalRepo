using System.Collections;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public AnimationCurve _lerpCurve;
    [SerializeField] private bool _isFlipping, _flipped;
    [SerializeField] private TextMeshProUGUI _text;

    public bool GetFlipStatus()
    {
        return _flipped;
    }

    public void Init(string text)
    {
        if (transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out var cellText))
        {
            cellText.text = text;
        }

        else
        {
            _text.text = text;
            _text.gameObject.SetActive(false);
        }

        //set flipped status for saving on init
        _flipped = false;

        //trigger animation after data is loaded, basic pop in smoothly
        StartCoroutine(LerpCell());
    }

    private IEnumerator LerpCell(float duration = 1.0f)
    {
        float current = 0;
        float fraction = current / duration;

        while (fraction <= 1)
        {
            current += Time.deltaTime;
            fraction = current / duration;

            transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _lerpCurve.Evaluate(fraction));
            yield return null;
        }
    }

    public void Flip()
    {
        //Avoids redundant flip calls

        if (!_flipped)
        {
            //spamming the click should not disrupt lerp in progress

            if (!_isFlipping)
            {
                StartCoroutine(FlipCard());
            }
        }
    }

    private IEnumerator FlipCard(float duration = 1.0f)
    {
        _isFlipping = true;

        float current = 0;
        float fraction = current / duration;

        while (fraction <= 1)
        {
            current += Time.deltaTime;
            fraction = current / duration;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 180, 0), _lerpCurve.Evaluate(fraction));
            yield return null;
        }

        _isFlipping = false;

        yield return null;

        _flipped = true;
    }
}
