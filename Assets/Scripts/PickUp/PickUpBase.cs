using UnityEngine;

public abstract class PickUpBase : MonoBehaviour
{
    protected Player Player;
    [SerializeField] protected AudioClip clip;
    protected abstract bool PickUp();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out Player)) return;
        if (!PickUp()) return;
        if (clip != null) Player.source.PlayOneShot(clip);
        Destroy(gameObject);
    }
}