using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class InterfaceControllerWithUI : InterfaceController
{
    [SerializeField] private GameObject _enterFloppyScreen = default;
    [SerializeField] private GameObject _loadingScreen = default;
    [SerializeField] private GameObject _mainScreen = default;
    [SerializeField] private GameObject _noInputWarning = default;
    [SerializeField] private int _loadTimeSec = 5;

    private bool _isMainScreen;
    private bool _floppyInserted;

    protected override bool InputActive => base.InputActive && _isMainScreen;
    private Room _currentRoom;

    [SerializeField] private UIInteractableButton _loadButton = default;
    [SerializeField] private UIInteractableButton _saveButton = default;

    protected override void Start()
    {
        _loadButton.OnButtonClicked += LoadButtonClickedHandler;
        // _saveButton.OnButtonClicked += SaveButtonClickedHandler;
        
        base.Start();
    }

    private void LoadButtonClickedHandler()
    {
        _loadingScreen.SetActive(true);
        _mainScreen.SetActive(false);
        _isMainScreen = false;
        
        WaitForReload();
    }

    private async void WaitForReload()
    {
        var loadStartTime = Time.time;
        
        await _currentRoom.Reload();

        var loadTime = Time.time - loadStartTime;
        if (loadTime < _loadTimeSec)
            await Task.Delay((int)((loadTime - _loadTimeSec) * 1000f));
        
        _loadingScreen.SetActive(false);
        _mainScreen.SetActive(true);
        _isMainScreen = true;
    }

    public override void Init(Room currentRoom)
    {
        base.Init(currentRoom);

        _currentRoom = currentRoom;
        IsSwitchedOff = !_currentRoom.PowerIsOn;

        if (_currentRoom.Display.FullyEquiped && !IsSwitchedOff)
        {
            _mainScreen.SetActive(true);
            return;
        }
        
        _currentRoom.Display.OnEquipmentChanged += DisplayEquipmentChangedHandler;
        _mouseActive = (int)(_currentRoom.Display.Equipment & Transferrable.ETransferrableId.Keyboard) != 0;
        _noInputWarning.SetActive(!_mouseActive);
        _isMainScreen = (int)(_currentRoom.Display.Equipment & Transferrable.ETransferrableId.Floppy) != 0
            && !IsSwitchedOff;

        _mainScreen.SetActive(_isMainScreen);
        _enterFloppyScreen.SetActive(!_isMainScreen);
        _uiRoot.gameObject.SetActive(!IsSwitchedOff);
    }

    private void DisplayEquipmentChangedHandler(Display display)
    {
        _floppyInserted = (int)(display.Equipment & Transferrable.ETransferrableId.Floppy) != 0;
        if (!_isTurningOn && !_isMainScreen && _floppyInserted && !IsSwitchedOff)
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
        
        yield return new WaitForSeconds(_loadTimeSec);
        
        _loadingScreen.SetActive(false);
        _mainScreen.SetActive(true);
        _isMainScreen = true;
        
        if (_mouseActive && _currentRoom.NextRoom != null)
            _currentRoom.NextRoom.LevelManager.RunGameplay();
    }

    private bool _isTurningOn;
    
    private IEnumerator TurnOn()
    {
        _isTurningOn = true;
        _loadingScreen.SetActive(true);
        yield return new WaitForSeconds(_loadTimeSec);

        _isTurningOn = false;
        if (_floppyInserted)
        {
            StartCoroutine(LoadProgramCoroutine());
            yield break;
        }
        
        _loadingScreen.SetActive(false);
        _enterFloppyScreen.SetActive(true);
    }

    public override void SetVisibleStatus(bool isSwitchedOn)
    {
        if (!isSwitchedOn)
        {
            var a = 0;
            a++;
        }

        IsSwitchedOff = !isSwitchedOn;
        
        _mainScreen.SetActive(false);
        _enterFloppyScreen.SetActive(false);
        _uiRoot.gameObject.SetActive(isSwitchedOn);

        if (isSwitchedOn)
            StartCoroutine(TurnOn());

        base.SetVisibleStatus(isSwitchedOn);
    }
}
