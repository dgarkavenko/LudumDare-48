using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class Room : MonoBehaviour
{
    public LevelManager LevelManager;
    public ComputerView ComputerView;
    public Display Display;
    public Player Player;
    public Camera Camera => Player.Camera;
    public List<Component> TurnOn;
    public Room NextRoom;
    public AudioMixerSnapshot MixerSnapshot;


    public float TimeSpentInRoom;
    private int _upcomingEventIndex = 0;
    public RoomEvent[] RoomEvents;

    public Turnable[] PowerRequirement;

    private bool _isDirty = true;
    private bool _turnables = false;
    public bool PowerIsOn
    {
        get
        {
            if (_isDirty)
                _turnables = PowerRequirement.Length == 0 || PowerRequirement.All(x => x.State == Turnable.ETurnableState.On);

            return _turnables;
        }
    }

    [HideInInspector]
    public Room PreviousRoom;

    public void Init(Room previousRoom)
    {
        PreviousRoom = previousRoom;
        Player = GetComponentInChildren<Player>();
        Display = GetComponentInChildren<Display>();
        ComputerView = GetComponentInChildren<ComputerView>();
        LevelManager = GetComponentInChildren<LevelManager>();

        foreach (var roomie in new[] {(IRoomie) Player, Display, ComputerView, LevelManager})
            if (roomie != null)
                roomie.ParentRoom = this;

        foreach (var turnable in PowerRequirement)
            turnable.StateChangedAction += StateChangedAction;

        NextRoom?.Init(this);
    }

    private void StateChangedAction(Turnable.ETurnableState obj)
    {
        _isDirty = true;

        ComputerView.SetTurnedOnStatus(PowerIsOn);
        // LevelManager.SetComputerTurnOnStatus(PowerIsOn);

        if (!PowerIsOn)
        {
            if (Display == GameManager.Instance.Displays.Last())
                GameManager.Instance.ZoomOutDisplay();
        }
    }

    public void Focus(bool on)
    {
        Player.enabled = on;

        if (on)
            LevelManager.Activate();
        else
            LevelManager.Deactivate();
    }

    public void Update()
    {
        if (this == GameManager.Instance.ActiveRoom)
            TimeSpentInRoom += Time.deltaTime;

        for (int i = _upcomingEventIndex; i < RoomEvents.Length; i++)
        {
            if (TimeSpentInRoom > RoomEvents[i].TimeSinceRoomActive)
            {
                RoomEvents[i].Invoke();
                _upcomingEventIndex++;
            }
            else
            {
                break;
            }
        }

    }

    [System.Serializable]
    public class RoomEvent
    {
        public float TimeSinceRoomActive;
        public Turnable Turnable;
        public Turnable.ETurnableState TargetState;
        public AudioSource Source;

        public void Invoke()
        {
            Turnable.State = TargetState;
            Source.Play();
        }
    }
}