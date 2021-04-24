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
    [SerializeField] private Image _cursor = default;
    [SerializeField] private RectTransform _uiRoot = default;

    private float _width;
    private float _height;
    
    private RectTransform _cursorTransform;

    private void Start()
    {
        _cursorTransform = _cursor.rectTransform;
        _width = _uiRoot.sizeDelta.x;
        _height = _uiRoot.sizeDelta.y;
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

    private void Update()
    {
        var mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        var position = _cursorTransform.anchoredPosition;
        position += mouseDelta;
        position.x = Mathf.Clamp(position.x, 0, _width);
        position.y = Mathf.Clamp(position.y, 0, _height);
        _cursorTransform.anchoredPosition = position;
    }
}
