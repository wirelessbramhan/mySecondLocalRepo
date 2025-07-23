using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [Header("Listening to"), Space(2)]
    public AudioEC_SO SFXChannel;
    private AudioSource _speaker;

    private void OnEnable()
    {
        SFXChannel.OnEventRaised += SFXChannel_OnEventRaised;
    }

    private void SFXChannel_OnEventRaised(AudioClip obj)
    {
        if (_speaker)
        {
            _speaker.PlayOneShot(obj);
        }
    }

    private void OnDisable()
    {
        SFXChannel.OnEventRaised -= SFXChannel_OnEventRaised;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!TryGetComponent(out _speaker))
        {
            Debug.Log("speaker not bound!!", this);
        }
    }

}
