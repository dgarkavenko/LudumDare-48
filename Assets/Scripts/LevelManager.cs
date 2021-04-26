using UnityEngine;

[RequireComponent(typeof(ComputerView))]
public class LevelManager : MonoBehaviour, IRoomie
{
    private Room Room => ParentRoom;
    private ComputerView Computer => ParentRoom.PreviousRoom ? ParentRoom.PreviousRoom .ComputerView : null;

    [SerializeField] private ELevelState _currentState = ELevelState.UI;

    private void Init()
    {
        if (Computer != null)
        {
            Computer.OnUIShown += () => _currentState = ELevelState.UI;
            Computer.OnGameplayShown += () => { _currentState = ELevelState.Gameplay; };
        }
    }

    public void Activate()
    {
        enabled = true;

        if (Computer != null)
        {
            if (CurrentState == ELevelState.UI)
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

    public void SetComputerTurnOnStatus(bool isTurnedOn)
    {
        if (Computer == null)
            return;

        if (isTurnedOn)
            _currentState = ELevelState.UI;

        Computer.SetTurnedOnStatus(isTurnedOn);
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
            if (Computer != null)
                Computer.HandleEnter();
        }

        if (!GameManager.Instance.AnyDisplays)
            return;
        
        if (Input.GetKeyDown(KeyCode.Tab) 
            || (Input.GetMouseButtonDown(1) && ParentRoom.Player.Burden != null))
        {
            GameManager.Instance.ZoomOutDisplay();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {   
            if (CurrentState == ELevelState.Gameplay)
            {
                ShowUI();
                Room.Player.enabled = false;
            }
            else
            {
                RunGameplay();
                Room.Player.enabled = true;
            }
        }
    }


    private Room _parentRoom;
    public Room ParentRoom
    {
        get => _parentRoom;
        set
        {
            _parentRoom = value;
            Init();
        }
    }

    public ELevelState CurrentState => _currentState;
}

public enum ELevelState
{
    UI,
    Gameplay,
    TurnedOff
}
