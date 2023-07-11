using UnityEngine;
using UnityEngine.Events;

public class TriggerPickUp : PickUpBase
{
    [SerializeField] private UnityEvent events;
    protected override bool PickUp()
    {
        events.Invoke();
        return true;
    }
}