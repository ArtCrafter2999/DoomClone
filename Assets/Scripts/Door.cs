using System;
using DG.Tweening;
using UnityEngine;

public class Door : MonoBehaviour
{
    
    [SerializeField] private Transform door;
    [SerializeField] private Transform endPoint;
    private Vector3 _startPoint;
    private Vector3 _endPoint;
    [SerializeField] private float duration = 1f;
    [SerializeField] private bool autoClose;
    [SerializeField] private float autoCloseTime;
    public bool canOpen = true;
    [Header("Sound")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;
    
    private float _autoCloseTime;
    private bool _isOpened = false;
    private void Start()
    {
        _startPoint = transform.position;
        _endPoint = endPoint.position;
    }

    private void Update()
    {
        if(!autoClose || !_isOpened) return;
        _autoCloseTime -= Time.deltaTime;
        if(_autoCloseTime <= 0) Close();
    }

    public void Activate()
    {
        canOpen = true;
    }
    public void Deactivate()
    {
        canOpen = false;
    }

    public void Open()
    {
        if(_isOpened|| !canOpen) return;
        if(source != null && openClip != null) source.PlayOneShot(openClip);
        door.DOMove(_endPoint, duration);
        _autoCloseTime = autoCloseTime;
        _isOpened = true;
    }
    public void Close()
    {
        if (source != null && closeClip != null) source.PlayOneShot(closeClip);
        door.DOMove(_startPoint, duration);
        _isOpened = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(door.position, _isOpened ? _startPoint : endPoint.position);
    }
}
