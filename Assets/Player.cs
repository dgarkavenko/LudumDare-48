using System;
using System.Collections;
using System.Collections.Generic;
using cakeslice;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(FirstPersonController))]

public class Player : MonoBehaviour, IRoomie
{
    [SerializeField] private FirstPersonController _fpc;

    public Transferrable Burden;
    public Vector3 HandsOffset;
    public Vector3 HandsPosition => _fpc.playerCamera.transform.position + _fpc.playerCamera.transform.rotation * HandsOffset;
    public Camera Camera => _fpc.playerCamera;
    public OutlineEffect Outliner;
    public AudioListener AudioListener;

    private Rigidbody _rigidbody;

    public void OnValidate()
    {
        _fpc = GetComponent<FirstPersonController>();
        _rigidbody = GetComponent<Rigidbody>();
        Outliner = _fpc.playerCamera.GetComponent<OutlineEffect>();
        AudioListener = _fpc.playerCamera.GetComponent<AudioListener>();
    }

    public void LateUpdate()
    {
        Burden?.Carried(HandsPosition, Camera.transform.rotation);
    }

    public void DropBurden()
    {
        Burden?.Drop(this);
    }

    public void OnEnable()
    {
        _fpc.enabled = true;
        _rigidbody.isKinematic = false;
    }

    public void OnDisable()
    {
        _fpc.enabled = false;
        _rigidbody.isKinematic = true;
        _rigidbody.velocity = Vector3.zero;
    }

    public Room ParentRoom { get; set; }
}
