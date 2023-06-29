using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance = null;
    public HasHealth health;
    
    [Header("Sound")] 
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip shootClip;
    
    [Header("Camera")] 
    [SerializeField] private GameObject cameraObject;
    [SerializeField] private Transform cameraPosition;
    private Camera _camera;
    private float _xRotation = 0f;
    private float _playerMovesSin = 0f;

    [Header("Movement")]
    [SerializeField] private float speed = 4;
    [Range(0.5f, 5)]
    [SerializeField] private float sensitivity = 5;

    [Header("Jump")] 
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform groundPoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundPointSize = 0.5f;
    private bool isOnGround => Physics.CheckSphere(groundPoint.position, groundPointSize, groundLayer);

    [Header("Shoot")] 
    [SerializeField] private GameObject shootPoint;
    [SerializeField] private Animator shotgunAnimator;
    [SerializeField] private float reloadSpeed = 5.6f;
    private float _reloadCoolDown = 0;
    
    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = cameraObject.GetComponent<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        Move();
        Jump();
        Look();
        if(Input.GetKeyDown(KeyCode.E)) Interact();
        if (Input.GetMouseButtonDown(0) && _reloadCoolDown <= 0) Shoot();
        _reloadCoolDown -= Time.deltaTime;

    }

    private void Move()
    {
        _moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * speed;

        var moveDir = transform.right * _moveInput.x + transform.forward * _moveInput.y;

        _rigidbody.velocity = moveDir + Vector3.up * _rigidbody.velocity.y;

        if (moveDir != Vector3.zero) _playerMovesSin += Time.deltaTime;
    }

    private void Jump()
    {
        if(!(Input.GetKeyDown(KeyCode.Space) && isOnGround)) return;
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void Look()
    {
        var mouseX = Input.GetAxis("Mouse X") * sensitivity;
        var mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        _xRotation -= mouseY;
        
        transform.Rotate(Vector3.up * mouseX);
        cameraObject.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        if (isOnGround)cameraObject.transform.localPosition = cameraPosition.localPosition + Vector3.up * Mathf.Sin(10*_playerMovesSin)/6.5f;
    }
    private void Interact()
    {
        var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out var hit, 1f)) return;
        hit.transform.GetComponent<Interactable>()?.Interact();
    }

    private void Shoot()
    {
        source.PlayOneShot(shootClip);
        shotgunAnimator.SetTrigger("Shoot");
        _reloadCoolDown = reloadSpeed;
        
        var ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (!Physics.Raycast(ray, out var hit)) return;
            
        print(hit.transform.name);
        shootPoint.transform.position = hit.point;

        if (!hit.transform.TryGetComponent<HasHealth>(out var health)) return;
        health.TakeDamage(1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundPoint.position, groundPointSize);
    }
}
