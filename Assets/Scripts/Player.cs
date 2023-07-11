using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HasHealth health;
    
    [Header("Sound")] 
    public AudioSource source;
    
    
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

    private Rigidbody _rigidbody;
    private Vector2 _moveInput;

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
        _xRotation = Mathf.Clamp(_xRotation, -90, 90);
        
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundPoint.position, groundPointSize);
    }
}