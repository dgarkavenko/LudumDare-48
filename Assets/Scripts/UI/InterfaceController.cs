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

    private Vector3 _mousePosition;
    private RectTransform _cursorTransform;

    private void OnEnable()
    {
        _mousePosition = Input.mousePosition;
        _cursorTransform = _cursor.rectTransform;
        _width = _uiRoot.sizeDelta.x;
        _height = _uiRoot.sizeDelta.y;
    }

    private void Update()
    {
        var oldPosition = _mousePosition;
        _mousePosition = Input.mousePosition;
        var mouseDelta = 0.1f * (_mousePosition - oldPosition);
        var position = _cursorTransform.anchoredPosition;
        position += new Vector2(mouseDelta.x, mouseDelta.y);
        position.x = Mathf.Clamp(position.x, 0, _width);
        position.y = Mathf.Clamp(position.y, 0, _height);
        _cursorTransform.anchoredPosition = position;
    }
}
