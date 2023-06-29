using UnityEngine;


public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    
    [Header("Movement")]
    [SerializeField] protected float invokeRange;
    [SerializeField] protected float speed;
    protected bool _canMove = true;
    
    [Header("Sounds")]
    [SerializeField] protected AudioSource source;
    [SerializeField] protected AudioClip sight;
    [SerializeField] protected AudioClip damaged;
    [SerializeField] protected AudioClip dead;
    
    protected Rigidbody _rigidbody;
    protected CapsuleCollider _collider;
    protected HasHealth _health;
    
    protected virtual void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
        _health = GetComponent<HasHealth>();
        _health.damaged.AddListener(Hurt);
        _health.dead.AddListener(Death);
    }

    public abstract void Attack();
    
    public void DeathSound()
    {
        source.PlayOneShot(dead);
    }
    
    public void HurtSound()
    {
        print("hurtSound");
        source.PlayOneShot(damaged);
    }

    protected virtual void Hurt(float damage)
    {
        animator.SetTrigger("Damaged");
        _canMove = true;
    }
    
    protected virtual void Death(GameObject _)
    {
        animator.SetTrigger("Dead");
        _rigidbody.isKinematic = true;
        _health.enabled = false;
        _collider.height = 0;
        _collider.center = new Vector3(0, -0.5f, 0);
        // Destroy(gameObject);
    }
    
    
}