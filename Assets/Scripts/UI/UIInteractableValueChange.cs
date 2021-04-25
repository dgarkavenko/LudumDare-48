using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UIInteractableValueChange : UIInteractableButton
{
    public event Action<int> OnValueChanged;

    [SerializeField] private TextMeshProUGUI _value;
    [SerializeField] private Color _activeColor = new Color(0.9f, 0.4f, 0.4f, 1);
    private Color _defaultColor;
    
    private string[] _values;
    private int _currentValueIndex;
    
    private bool _active;

    public bool Active
    {
        get => _active;
        private set
        {
            if (_active == value)
                return;

            _active = value;
            _value.color = _active ? _activeColor : _defaultColor;
        }
    }
    
    protected override void Start()
    {
        _defaultColor = _value.color;
        base.Start();
    }

    public void Init(IEnumerable<string> values, int valueIndex)
    {
        _values = values.ToArray();
        _currentValueIndex = Mathf.Clamp(valueIndex, 0 , _values.Length);
        _value.text = _values[_currentValueIndex];
    }

    public override void OnUIMouseClicked()
    {
        base.OnUIMouseClicked();
        
        if (!IsInteractable)
            return;

        Active = _values != null && _values.Length > 1;
    }

    private void Update()
    {
        if (!IsInteractable)
            return;
        
        if (Input.GetMouseButtonDown(0) && _active && !OnMouseOver)
        {
            Active = false;
        }

        if (!Active || _values == null || _values.Length <= 1)
            return;
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _currentValueIndex++;
            if (_currentValueIndex >= _values.Length)
                _currentValueIndex = 0;

            ChangeValue();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _currentValueIndex--;
            if (_currentValueIndex < 0)
                _currentValueIndex = _values.Length;
            
            ChangeValue();
        }
    }

    private void ChangeValue()
    {
        _value.text = _values[_currentValueIndex];
        OnValueChanged?.Invoke(_currentValueIndex);
    }

    protected override void OnInteractableChanged()
    {
        var color = _value.color;
        color.a = IsInteractable ? 1 : 0.5f;
        _value.color = color;
        
        base.OnInteractableChanged();
    }
}
