using System.Collections;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public AnimationCurve _lerpCurve;
    [SerializeField] private bool _isFlipping, _flipped;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField, Header("listening to"), Space(2)] private BoolEC_SO _evalChannel;
    [SerializeField, Header("broadcasting on"), Space(2)] private FloatEC_SO _valueChannel;
    private int _value;

    private void OnEnable()
    {
        _evalChannel.OnEventRaised += EvalChannel_OnEventRaised;
    }

    private void EvalChannel_OnEventRaised(bool obj)
    {
        if (_flipped)
        {
            HideCell();
        }
    }

    private void OnDisable()
    {
        _evalChannel.OnEventRaised -= EvalChannel_OnEventRaised;
    }

    public void Init(int value, bool flip)
    {
        //set cell state for saving on init
        _value = value;
        _flipped = flip;

        //Hide text if card state is flipped
        if (transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out var cellText))
        {
            cellText.text = _value.ToString();
        }

        else
        {
            _text.text = _value.ToString();
        }

        if (!_flipped)
        {
            _text.gameObject.SetActive(false);
        }

        //trigger animation after data is loaded, basic pop in smoothly
        StartCoroutine(LerpCell(true));
    }

    public bool GetFlipStatus()
    {
        return _flipped;
    }

    private IEnumerator LerpCell(bool lerpIn, float duration = 1.0f)
    {
        float current = 0;
        float fraction = current / duration;

        while (fraction <= 1)
        {
            current += Time.deltaTime;
            fraction = current / duration;

            if (lerpIn)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, _lerpCurve.Evaluate(fraction));
            }

            else
            {
                transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, _lerpCurve.Evaluate(fraction));
            }

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

    private IEnumerator FlipCard(float duration = 0.5f)
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
        _flipped = true;

        yield return null;

        _valueChannel.RaiseEvent(_value);
    }

    public void HideCell()
    {
        transform.localScale = Vector3.zero;
    }
}
