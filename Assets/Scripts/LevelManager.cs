using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Current { get; private set; }

    [SerializeField] private Room _room;
    [SerializeField] private ComputerView _computer;

    private ELevelState _currentState = ELevelState.UI;

    private void Start()
    {
        if (_computer != null)
        {
            _computer.OnUIShown += () => _currentState = ELevelState.UI;
            _computer.OnGameplayShown += () =>
            {
                _currentState = ELevelState.Gameplay;
                _room.Player.enabled = true;
            };
        }
    }

    public void Activate()
    {
        enabled = true;
        Current = this;

        if (_computer != null)
            _computer.ShowUI();
        else
            _room.Player.enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
        _room.Player.enabled = false;
        
        if (_computer != null && _computer.InterfaceController != null)
            _computer.InterfaceController.enabled = false;
    }
    
    public void TurnOnVisually()
    {
        if (_computer != null)
            _computer.TurnOn();
    }
    
    public void ShowUI()
    {
        if (_computer != null)
            _computer.ShowUI();
    }

    public void RunGameplay()
    {
        if (_computer != null)
            _computer.ShowGameplay();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RunGameplay();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentState == ELevelState.Gameplay && !_room.StartingRoom)
            {
                ShowUI();
                _room.Player.enabled = false;
            }
            else if (GameManager.Instance.AnyDisplays)
            {
                GameManager.Instance.ZoomOutDisplay();
            }
        }
    }
}

public enum ELevelState
{
    UI,
    Gameplay
}
