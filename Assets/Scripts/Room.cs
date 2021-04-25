using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public LevelManager LevelManager;
    public ComputerView ComputerView;
    public Display Display;
    public Player Player;
    public Camera Camera => Player.Camera;
    public List<Component> TurnOn;
    public Room NextRoom;

    [HideInInspector]
    public Room PreviousRoom;

    public void Init(Room previousRoom)
    {
        PreviousRoom = previousRoom;
        Player = GetComponentInChildren<Player>();
        LevelManager = GetComponentInChildren<LevelManager>();
        Display = GetComponentInChildren<Display>();
        ComputerView = GetComponentInChildren<ComputerView>();
        NextRoom?.Init(this);
    }

    public void Focus(bool on)
    {
        Player.enabled = on;

        if (on)
            LevelManager.Activate();
        else
            LevelManager.Deactivate();
    }
}
