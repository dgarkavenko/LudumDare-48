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
        Display = GetComponentInChildren<Display>();
        ComputerView = GetComponentInChildren<ComputerView>();
        LevelManager = GetComponentInChildren<LevelManager>();

        foreach (var roomie in new[] {(IRoomie) Player, Display, ComputerView, LevelManager})
            if (roomie != null)
                roomie.ParentRoom = this;

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