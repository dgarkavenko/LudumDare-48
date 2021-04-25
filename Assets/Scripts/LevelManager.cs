using UnityEngine;

[RequireComponent(typeof(ComputerView))]
public class LevelManager : MonoBehaviour, IRoomie
{
    public static LevelManager Current { get; private set; }

    private Room Room => ParentRoom;
    private ComputerView Computer => ParentRoom.PreviousRoom ? ParentRoom.PreviousRoom .ComputerView : null;

    private ELevelState _currentState = ELevelState.UI;

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
        Current = this;

        if (Computer != null)
            Computer.ShowUI();
        else
            Room.Player.enabled = true;
    }

    public void Deactivate()
    {
        enabled = false;
        Room.Player.enabled = false;

        if (Computer != null && Computer.InterfaceController != null)
            Computer.InterfaceController.enabled = false;
    }

    public void TurnOnVisually()
    {
        if (Computer != null)
            Computer.TurnOn();
    }

    public void ShowUI()
    {
        if (Computer != null)
            Computer.ShowUI();
    }

    public void RunGameplay()
    {
        if (Computer != null)
            Computer.ShowGameplay();
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
    Gameplay
}
