using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] private MouseInteractionDetector _interactionDetector = default;
    [SerializeField] private Transform _mainCharTransform = default;
    
    [SerializeField] private Image _targetMarkerObject = default;
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _interactionColor = Color.green;

    [SerializeField] private Vector3 _transferOffset = new Vector3(1, 1, 1);

    public Transferrable Transferrable { get; private set; }
    private Transform _transferTransform;

    private void Update()
    {
        _targetMarkerObject.color = _interactionDetector.AnyHitTarget
            ? _interactionColor
            : _defaultColor;

        if (_interactionDetector.AnyHitTarget && Input.GetMouseButtonDown(0))
            TryInteract();
        
        Transfer();
    }

    private void TryInteract()
    {
        var interactable = _interactionDetector.Interacteable;
        if (interactable.CanInteract(this))
            interactable.Interact(this);
    }

    private void Transfer()
    {
        if (_transferTransform == null)
            return;

        _transferTransform.position = _mainCharTransform.position + _mainCharTransform.rotation * _transferOffset;
    }

    public void Transfer(Transferrable transferring)
    {
        Transferrable = transferring;
        _transferTransform = Transferrable.transform;
        
        var transferCollider = Transferrable.GetComponent<Collider>();
        if (transferCollider != null)
            transferCollider.enabled = false;
    }
}
