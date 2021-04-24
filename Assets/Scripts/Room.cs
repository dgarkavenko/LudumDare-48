using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public bool StartingRoom;

    public LevelManager LevelManager;
    public FirstPersonController Player;
    public Camera Camera;
    public List<Component> TurnOn;
}
