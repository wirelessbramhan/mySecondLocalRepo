using System.Collections;
using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public AnimationCurve _lerpCurve;
    [SerializeField] private bool _isFlipping, _flipped;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField, Header("broadcasting on"), Space(2)] private StringEC_SO _valueChannel;
    [SerializeField, Header("listening to"), Space(2)] private BoolEC_SO _evalChannel;
    private string _value;

    public void Init(string value, bool flip, float flipDelay)
    {
        //set cell state for saving on init
        _value = value;
        _flipped = flip;
        _isFlipping = false;

        //Hide text if card state is flipped
        if (transform.GetChild(0).TryGetComponent<TextMeshProUGUI>(out var cellText))
        {
            cellText.text = _value;
        }

        else
        {
            _text.text = _value;
        }

        //trigger animation after data is loaded, basic pop in smoothly
        StartCoroutine(LerpCell(true, 0.1f));

        //Show the card
        StartCoroutine(ShowCard(flipDelay));
    }

    #region flipping & evaluation

    private void OnEnable()
    {
        _evalChannel.OnEventRaised += EvalChannel_OnEventRaised;
    }

    private void EvalChannel_OnEventRaised(bool obj)
    {
        if (_flipped)
        {
            if (obj)
            {
                HideCell();
            }

            else
            {
                FlipCard();
            }
        }
    }

    private void OnDisable()
    {
        _evalChannel.OnEventRaised -= EvalChannel_OnEventRaised;
    }

    public void FlipCard()
    {
        _flipped = !_flipped;

        if (!_isFlipping)
        {
            //spamming the click should not disrupt lerp in progress
            StartCoroutine(FlipCard(_flipped));
        }
    }

    private IEnumerator FlipCard(bool flip, float duration = 0.5f)
    {
        _isFlipping = true;

        float current = 0;
        float fraction = current / duration;

        Quaternion targetrot = Quaternion.Euler(0, 180, 0);

        if (!flip)
        {
            targetrot = Quaternion.Euler(Vector3.zero);
        }

        //show text before lerp
        _text.gameObject.SetActive(flip);

        while (fraction <= 1)
        {
            current += Time.deltaTime;
            fraction = current / duration;

            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetrot, _lerpCurve.Evaluate(fraction));
            yield return null;
        }

        _isFlipping = false;

        yield return null;

        _valueChannel.RaiseEvent(gameObject.name);
    }

    #endregion

    private IEnumerator ShowCard(float delay)
    {
        transform.localRotation = Quaternion.Euler(0, 180, 0);

        yield return new WaitForSeconds(delay);

        StartCoroutine(FlipCard(false));
    }

    public void HideCell()
    {
        transform.localScale = Vector3.zero;
    }

    private IEnumerator LerpCell(bool lerpIn, float duration = 1.0f)
    {
        float current = 0;

        Vector3 targetScale = Vector3.one;

        if (!lerpIn)
        {
            targetScale = Vector3.zero;
        }

        while (transform.localScale != targetScale)
        {
            current += Time.deltaTime;
            float fraction = current / duration;

            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, _lerpCurve.Evaluate(fraction));

            yield return null;
        }
    }
}
