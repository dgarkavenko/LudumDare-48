using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UIInteractableButton : UIInteractableElement
{
    public event Action OnButtonClicked; 
    
    private TextMeshProUGUI _tmpro;

    protected override void Start()
    {
        _tmpro = GetComponent<TextMeshProUGUI>();

        base.Start();
    }

    public override void OnUIMouseEnter()
    {
        base.OnUIMouseEnter();
        
        if (!IsInteractable)
            return;
        
        var color = _tmpro.color;
        color.a = 0.7f;
        _tmpro.color = color;
    }

    public override void OnUIMouseExit()
    {
        base.OnUIMouseExit();
        
        if (!IsInteractable)
            return;
        
        var color = _tmpro.color;
        color.a = 1f;
        _tmpro.color = color;
    }

    public override void OnUIMouseClicked()
    {
        if (!IsInteractable)
            return;
        
        ClickReaction();
        OnButtonClicked?.Invoke();
    }

    private async void ClickReaction()
    {
        var color = _tmpro.color;
        var newColor = color;
        newColor.g = 0.7f;

        _tmpro.color = newColor;

        await Task.Delay(50);
        
        if (_tmpro.color == newColor)
            _tmpro.color = color;
    }

    protected override void OnInteractableChanged()
    {
        var color = _tmpro.color;
        color.a = IsInteractable ? 1 : 0.5f;
        _tmpro.color = color;
        
        base.OnInteractableChanged();
    }
}
