using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FirstPersonController))]

public class Player : MonoBehaviour
{
    [SerializeField] private FirstPersonController _fpc;

    public Transferrable Burden;
    public Vector3 HandsOffset;
    public Vector3 HandsPosition => _fpc.playerCamera.transform.position + _fpc.playerCamera.transform.rotation * HandsOffset;
    public Camera Camera => _fpc.playerCamera;

    public void OnValidate()
    {
        _fpc = GetComponent<FirstPersonController>();
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
    }

    public void OnDisable()
    {
        _fpc.enabled = false;
    }
}
