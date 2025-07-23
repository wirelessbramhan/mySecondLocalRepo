using System;
using UnityEngine;

public abstract class EventChannelSOBase<T> : ScriptableObject
{
    public event Action<T> OnEventRaised;

    public virtual void RaiseEvent(T data)
    {
        OnEventRaised?.Invoke(data);
    }
}
