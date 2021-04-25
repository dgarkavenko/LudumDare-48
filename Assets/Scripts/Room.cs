using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool StartingRoom;

    public LevelManager LevelManager;
    public Player Player;
    public Camera Camera => Player.Camera;
    public List<Component> TurnOn;
}
