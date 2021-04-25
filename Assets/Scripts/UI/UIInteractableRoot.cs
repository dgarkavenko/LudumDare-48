using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInteractableRoot : MonoBehaviour
{
    [SerializeField] private InterfaceController _controller = default;
    [SerializeField] private UIInteractableElement[] _interactableElements = default;

    private void OnEnable()
    {
        _controller.RegisterUIElements(_interactableElements);
    }

    private void OnDisable()
    {
        _controller.RemoveUIElements(_interactableElements);
    }
}
