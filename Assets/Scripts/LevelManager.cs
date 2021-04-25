using UnityEngine;

[RequireComponent(typeof(ComputerView))]
public class LevelManager : MonoBehaviour, IRoomie
{
    private Room Room => ParentRoom;
    private ComputerView Computer => ParentRoom.PreviousRoom ? ParentRoom.PreviousRoom .ComputerView : null;

    [SerializeField] private ELevelState _currentState = ELevelState.UI;

    private void Start()
    {
        if (Computer != null)
        {
            Computer.OnUIShown += () => _currentState = ELevelState.UI;
            Computer.OnGameplayShown += () =>
            {
                _currentState = ELevelState.Gameplay;
                Room.Player.enabled = true;
            };
        }
    }

    public void Activate()
    {
        enabled = true;

        if (Computer != null)
        {
            if (_currentState == ELevelState.UI)
                Computer.ShowUI();
            else
                RunGameplay();
                // Computer.HandleEnter();
        }
    }

    public void Deactivate()
    {
        enabled = false;
        Room.Player.enabled = false;

        if (Computer != null && Computer.InterfaceController != null)
            Computer.InterfaceController.Deactivate();
    }

    public void SetComputerTurnOnStatus(bool status)
    {
        if (Computer == null)
            return;

        if (status)
            _currentState = ELevelState.UI;
        
        Computer.SetTurnedOnStatus(status);
    }

    public void ShowUI()
    {
        if (Computer != null)
            Computer.ShowUI();
    }

    public void RunGameplay()
    {
        if (Computer != null)
            Computer.HandleEnter();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            RunGameplay();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.Instance.AnyDisplays)
                return;

            if (_currentState == ELevelState.Gameplay
                && Computer != null
                && Computer.InterfaceController != null)
            {
                ShowUI();
                Room.Player.enabled = false;
            }
            else
            {
                GameManager.Instance.ZoomOutDisplay();
            }
        }
    }

    public Room ParentRoom { get; set; }
}

public enum ELevelState
{
    UI,
    Gameplay,
    TurnedOff
}
