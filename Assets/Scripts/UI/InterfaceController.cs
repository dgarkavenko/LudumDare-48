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
    [SerializeField] private Camera _uiCamera = default;
    [SerializeField] private RectTransform _cursor = default;
    [SerializeField] private RectTransform _uiRoot = default;

    public Camera UICamera => _uiCamera;
    
    private float _width;
    private float _height;

    private Vector3[] _elementCorners = new Vector3[4]; 
    private readonly List<UIInteractableElement> _interactables = new List<UIInteractableElement>();

    private void Start()
    {
        _width = _uiRoot.sizeDelta.x;
        _height = _uiRoot.sizeDelta.y;
        _cursor.SetAsLastSibling();
    }

    public void Activate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
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

    private void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var position = _cursor.anchoredPosition;
        position += mouseDelta;
        position.x = Mathf.Clamp(position.x, 0, _width);
        position.y = Mathf.Clamp(position.y, 0, _height);
        _cursor.anchoredPosition = position;
        
        foreach (var interactable in _interactables)
        {
            if (interactable.IsCollides(_cursor.position))
                Debug.Log("We are here | " + _cursor.position);
        }
    }
}
