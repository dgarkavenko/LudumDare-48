using System;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceController : MonoBehaviour
{
    public event Action OnResumeClicked;
    
    [SerializeField] private UIInteractableButton _resumeButton;
    
    public static InterfaceController Current { get; private set; }

    [SerializeField] private Camera _uiCamera = default;
    [SerializeField] private RectTransform _cursor = default;
    [SerializeField] private RectTransform _uiRoot = default;
    [SerializeField] protected bool _mouseActive = true;
    [SerializeField] private UIInteractableElement[] _interactables = default;

    protected virtual bool InputActive => _mouseActive;
    
    public Camera UICamera => _uiCamera;

    private float _width;
    private float _height;

    protected virtual void Start()
    {
        _width = _uiRoot.sizeDelta.x;
        _height = _uiRoot.sizeDelta.y;

        if (_cursor != null)
            _cursor.SetAsLastSibling();
        
        if (_resumeButton != null)
            _resumeButton.OnButtonClicked += ResumeButtonClickedHandler;
    }

    public virtual void Init(Room currentRoom)
    {
        
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
    }

    public void RegisterUIElements(IEnumerable<UIInteractableElement> elements)
    {
        // _interactables.AddRange(elements);
    }

    public void RemoveUIElements(IEnumerable<UIInteractableElement> elements)
    {
        // foreach (var element in elements)
        //     _interactables.Remove(element);
    }

    protected virtual void Update()
    {
        MoveMouse();
        CheckInteractions();
    }

    private void MoveMouse()
    {
        if (!InputActive)
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
        var mouseClicked = Input.GetMouseButtonDown(0);
        
        foreach (var interactable in _interactables)
        {
            interactable.CheckMouseOver(_cursor.position);
            
            if (interactable.OnMouseOver && mouseClicked)
                interactable.OnUIMouseClicked();
            //if (interactable.IsCollides(_cursor.position))
            // TODO: onmouseover  
        }
    }

    public virtual bool HandleEnter()
    {
        return InputActive;
    }
    
    private void ResumeButtonClickedHandler()
    {
        OnResumeClicked?.Invoke();
    }
}
