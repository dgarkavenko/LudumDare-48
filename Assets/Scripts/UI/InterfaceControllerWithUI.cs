using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceControllerWithUI : InterfaceController
{
    [SerializeField] private GameObject _enterFloppyScreen = default;
    [SerializeField] private GameObject _loadingScreen = default;
    [SerializeField] private GameObject _mainScreen = default;
    [SerializeField] private GameObject _noInputWarning = default;

    private bool _isMainScreen;

    protected override bool InputActive => base.InputActive && _isMainScreen;

    // [SerializeField] private UIInteractableButton _loadButton = default;
    // [SerializeField] private UIInteractableButton _saveButton = default;

    protected override void Start()
    {
        // _loadButton.OnButtonClicked += LoadButtonClickedHandler;
        // _saveButton.OnButtonClicked += SaveButtonClickedHandler;
        
        base.Start();
    }

    public override void Init(Room currentRoom)
    {
        base.Init(currentRoom);

        var display = currentRoom.Display;
        
        if (currentRoom.Display.FullyEquiped)
        {
            _mainScreen.SetActive(true);
        }
        else
        {
            display.OnEquipmentChanged += DisplayEquipmentChangedHandler;
            _mouseActive = (int)(display.Equipment & Transferrable.ETransferrableId.Keyboard) != 0;
            _noInputWarning.SetActive(!_mouseActive);
            _isMainScreen = (int)(display.Equipment & Transferrable.ETransferrableId.Floppy) != 0;
            _mainScreen.SetActive(_isMainScreen);
            _enterFloppyScreen.SetActive(!_isMainScreen);
        }
    }

    private void DisplayEquipmentChangedHandler(Display display)
    {
        var floppyReady = (int)(display.Equipment & Transferrable.ETransferrableId.Floppy) != 0;
        if (floppyReady)
            StartCoroutine(LoadProgramCoroutine());

        if (!_mouseActive)
        {
            var inputReady = (int)(display.Equipment & Transferrable.ETransferrableId.Keyboard) != 0;
            if (inputReady)
            {
                _mouseActive = true;
                _noInputWarning.SetActive(false);
            }
        }
    }

    private IEnumerator LoadProgramCoroutine()
    {
        _enterFloppyScreen.SetActive(false);
        _loadingScreen.SetActive(true);
        
        yield return new WaitForSeconds(5);
        
        _loadingScreen.SetActive(false);
        _mainScreen.SetActive(true);
        _isMainScreen = true;
    }
}
