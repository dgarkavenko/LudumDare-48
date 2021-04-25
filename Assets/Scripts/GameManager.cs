using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player ActivePlayer => ActiveRoom.Player;


    [SerializeField] private Room _activeRoom;

    public Room ActiveRoom
    {
        get => _activeRoom;

        set
        {
            if (_activeRoom != value)
            {
                _activeRoom.Player.enabled = false;
                value.Player.enabled = true;
                _activeRoom = value;
            }
        }
    }

    public List<Display> Displays;

    private int firstVisibleDisplay;

    private void Awake()
    {
        Instance = this;
        ActivePlayer.enabled = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Displays.Count > 0) {
            var lastDisplay = Displays[Displays.Count - 1];

            if (lastDisplay.CurrentState == Display.State.UI || lastDisplay.CurrentState == Display.State.ControllingNextRoom) {
                StartCoroutine(ZoomOutAndRemove());

                IEnumerator ZoomOutAndRemove()
                {
                    yield return lastDisplay.ZoomOut();
                    Displays.RemoveAt(Displays.Count - 1);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F4) && Displays.Count > 0) {
            var firstDisplay = Displays[firstVisibleDisplay];

            if (firstDisplay.CurrentState == Display.State.ControllingNextRoom) {
                StartCoroutine(MakeDisappear());

                IEnumerator MakeDisappear()
                {
                    yield return firstDisplay.MakeDisappear();
                    //firstDisplay.PlayerCamera.enabled = false;
                    firstVisibleDisplay++;
                }
            }
        }

        if (Input.GetMouseButton(1))
        {
            ActivePlayer.DropBurden();
        }
    }
}
