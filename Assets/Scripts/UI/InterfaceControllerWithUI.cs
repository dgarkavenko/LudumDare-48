using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceControllerWithUI : InterfaceController
{
    [SerializeField] private GameObject _enterFloppyScreen = default;
    [SerializeField] private GameObject _mainScreen = default;
    
    [SerializeField] private UIInteractableButton _resumeButton = default;
    [SerializeField] private UIInteractableButton _loadButton = default;
    [SerializeField] private UIInteractableButton _saveButton = default;

    protected override void Start()
    {
        _resumeButton.OnButtonClicked += ResumeButtonClickedHandler;
        _loadButton.OnButtonClicked += LoadButtonClickedHandler;
        _saveButton.OnButtonClicked += SaveButtonClickedHandler;
        
        base.Start();
    }

    private void SaveButtonClickedHandler()
    {
        
    }

    private void LoadButtonClickedHandler()
    {
        
    }

    private void ResumeButtonClickedHandler()
    {
        
    }
}
