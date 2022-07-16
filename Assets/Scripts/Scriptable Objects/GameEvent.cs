using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Game Event", menuName ="Scriptable Objects/Game Event")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> Listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = Listeners.Count - 1; i >= 0; i--)
        {
            Listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!Listeners.Contains(listener))
        {
            Listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (Listeners.Contains(listener))
        {
            Listeners.Remove(listener);
        }
    }
}
