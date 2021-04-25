using System.Collections;
using System.Linq;
using UnityEngine;

public class Display : InteractableObject, IRoomie
{
    public AnimationCurve ZoomInCurve;
    public AnimationCurve ZoomOutCurve;

    public float ZoomInDuration;
    public float ZoomOutDuration;

    public Transform CameraTargetPosition;
    public Transform CameraDisappearPosition;

    public State CurrentState = State.Idle;

    public LevelManager LevelManager;
    public Camera PlayerCamera;

    private (Vector3, Quaternion) original;
    private RaycastHit[] raycastHits = new RaycastHit[32];
    private LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask(LayerMask.LayerToName(this.gameObject.layer));
    }


    private void Update()
    {
        // if (CurrentState != State.Idle)
        //     return;
        //
        // var raycastHitsCount = Physics.RaycastNonAlloc(
        //     origin: PlayerCamera.transform.position,
        //     results: raycastHits,
        //     direction: PlayerCamera.transform.forward,
        //     maxDistance: 2.2f,
        //     layerMask: layerMask,
        //     queryTriggerInteraction: QueryTriggerInteraction.Collide
        // );
        //
        // for (var i = 0; i < raycastHitsCount; i++)
        // {
        //     if (raycastHits[i].transform == this.transform)
        //     {
        //         if (CurrentState == State.Idle && Input.GetKeyDown(KeyCode.E))
        //             EnterUI();
        //
        //         break;
        //     }
        // }
    }

    public void EnterUI()
    {
        StartCoroutine(ZoomIn());

        IEnumerator ZoomIn()
        {
            LevelManager.Deactivate();
            CurrentState = State.ZoomingIn;
            original = (PlayerCamera.transform.position, PlayerCamera.transform.rotation);

            yield return Zoom(
                ZoomInDuration,
                ZoomInCurve,
                from: original,
                to: (CameraTargetPosition.position, CameraTargetPosition.rotation)
            );

            CurrentState = State.UI;
            Cursor.lockState = CursorLockMode.None;

            ControlNextRoom();
        }
    }

    public void ControlNextRoom()
    {
        CurrentState = State.ControllingNextRoom;
        GameManager.Instance.Displays.Add(this);
        GameManager.Instance.Descend();
    }

    public IEnumerator MakeDisappear()
    {
        yield return Zoom(
            ZoomInDuration,
            ZoomInCurve,
            from: (CameraTargetPosition.position, CameraTargetPosition.rotation),
            to: (CameraDisappearPosition.position, CameraDisappearPosition.rotation)
        );
    }

    public IEnumerator ZoomOut()
    {
        CurrentState = State.ZoomingOut;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Instance.Ascend();

        yield return Zoom(
            ZoomOutDuration,
            ZoomOutCurve,
            from: (PlayerCamera.transform.position, PlayerCamera.transform.rotation),
            to: original
        );

        CurrentState = State.Idle;
    }

    private IEnumerator Zoom(float duration, AnimationCurve curve, (Vector3, Quaternion) from, (Vector3, Quaternion) to)
    {
        var (sourcePosition, sourceRotation) = from;
        var (targetPosition, targetRotation) = to;
        var t = 0f;

        while (t < duration) {
            var value = curve.Evaluate(t / duration);
            PlayerCamera.transform.position = Vector3.Lerp(sourcePosition, targetPosition, value);
            PlayerCamera.transform.rotation = Quaternion.Lerp(sourceRotation, targetRotation, value);

            yield return null;

            t += Time.deltaTime;
        }
    }

    public enum State
    {
        Idle,
        ZoomingIn,
        UI,
        ControllingNextRoom,
        ZoomingOut
    }

    public override bool CanInteract(Player player)
    {
        return base.CanInteract(player) && CurrentState == State.Idle && ParentRoom.PowerIsOn;
    }

    public override void Interact(Player player)
    {
        if (RequiresItem)
            base.Interact(player);
        else
            EnterUI();
    }

    public override void AcceptRequiredItem(Transferrable requiredItem)
    {
        RequiresItem = null;
        requiredItem.transform.position = transform.position;
    }

    public Room ParentRoom { get; set; }
}
