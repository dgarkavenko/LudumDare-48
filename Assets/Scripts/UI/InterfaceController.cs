using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;
using Cursor = UnityEngine.Cursor;

public class InterfaceController : MonoBehaviour
{
    public static InterfaceController Current { get; private set; }

    [SerializeField] private Camera _uiCamera = default;
    [SerializeField] private RectTransform _cursor = default;
    [SerializeField] private RectTransform _uiRoot = default;
    [SerializeField] private bool _mouseActive = true;

    public Camera UICamera => _uiCamera;

    private float _width;
    private float _height;

    private Vector3[] _elementCorners = new Vector3[4];
    private readonly List<UIInteractableElement> _interactables = new List<UIInteractableElement>();

    private void Start()
    {
        _width = _uiRoot.sizeDelta.x;
        _height = _uiRoot.sizeDelta.y;

        _cursor.gameObject.SetActive(_mouseActive);
        if (_mouseActive)
            _cursor.SetAsLastSibling();
    }

    private void OnEnable()
    {
        Current = this;
    }

    private void OnDisable()
    {
        if (Current == this)
            Current = null;
    }

    public void Activate()
    {
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
    }

    public void SetVisibleStatus(bool isTurnedOn)
    {
        _uiRoot.gameObject.SetActive(isTurnedOn);
        Deactivate();
    }

    public void RegisterUIElements(IEnumerable<UIInteractableElement> elements)
    {
        _interactables.AddRange(elements);
    }

    public void RemoveUIElements(IEnumerable<UIInteractableElement> elements)
    {
        foreach (var element in elements)
            _interactables.Remove(element);
    }

    protected virtual void Update()
    {
        MoveMouse();
        CheckInteractions();
    }

    private void MoveMouse()
    {
        if (!_mouseActive)
            return;
        
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var position = _cursor.anchoredPosition;
        position += mouseDelta;
        position.x = Mathf.Clamp(position.x, 0, _width);
        position.y = Mathf.Clamp(position.y, 0, _height);
        _cursor.anchoredPosition = position;
    }

    private void CheckInteractions()
    {
        foreach (var interactable in _interactables)
        {
            //if (interactable.IsCollides(_cursor.position))
              // TODO: onmouseover  
        }
    }

    public virtual bool HandleEnter()
    {
        return true;
    }
}
