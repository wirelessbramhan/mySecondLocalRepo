using UnityEngine;


[CreateAssetMenu(fileName = "AEC_New", menuName = "Event Channels/Audio", order = 1)]
public class AudioEC_SO : EventChannelSOBase<AudioClip>
{
    public AudioClip SFXClip;

    public override void RaiseEvent(AudioClip data)
    {
        base.RaiseEvent(data);
    }

    public void RaiseEvent()
    {
        base.RaiseEvent(SFXClip);
    }
}
