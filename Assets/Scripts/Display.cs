using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Display : InteractableObject, IRoomie
{
    public event Action<Display> OnEquipmentChanged;

    public AnimationCurve ZoomInCurve;
    public AnimationCurve ZoomOutCurve;

    public float ZoomInDuration;
    public float ZoomOutDuration;

    public Transform CameraTargetPosition;
    public Transform CameraDisappearPosition;

    public State CurrentState = State.Idle;
    public AudioClip FloppySound;
    public float Volume = 1;

    [SerializeField] private Transferrable.ETransferrableId _equipment;
    [SerializeField] private Transferrable.ETransferrableId _fullyEquipedFlag = (Transferrable.ETransferrableId.Floppy | Transferrable.ETransferrableId.Keyboard);

    [System.Serializable]
    public class EquipmentPosition
    {
        public Transferrable.ETransferrableId Id;
        public Transform Transform;
        public AudioClip Clip;
    }

    public EquipmentPosition[] EquipmentPositions;

    public Transferrable.ETransferrableId Equipment
    {
        get => _equipment;
        set
        {
            if (value == _equipment)
                return;

            _equipment = value;
            OnEquipmentChanged?.Invoke(this);
        }
    }

    public bool FullyEquiped => Equipment == _fullyEquipedFlag;
    public bool HasFloppy => (Equipment & Transferrable.ETransferrableId.Floppy) != 0;


    public LevelManager LevelManager => ParentRoom.LevelManager;
    public Camera PlayerCamera => ParentRoom.Camera;

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

        while (t < duration)
        {
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
        if (player.Burden != null && (_fullyEquipedFlag & player.Burden.Id) != 0)
            return true;

        return CurrentState == State.Idle && ParentRoom.PowerIsOn && HasFloppy;
    }


    public override void Interact(Player player)
    {
        if (!FullyEquiped)
        {
            AcceptRequiredItem(player.Burden);
            player.Burden = null;
        }
        else
            EnterUI();
    }

    public override void AcceptRequiredItem(Transferrable requiredItem)
    {
        if (requiredItem == null)
            return;

        if (requiredItem.Id == Transferrable.ETransferrableId.Floppy)
            GameManager.Instance.PlaySound(FloppySound,1);

        Equipment |= requiredItem.Id;

        for (int i = 0; i < EquipmentPositions.Length; i++)
        {
            if (EquipmentPositions[i].Id == requiredItem.Id)
            {
                requiredItem.transform.SetPositionAndRotation(EquipmentPositions[i].Transform.position, EquipmentPositions[i].Transform.rotation);
                return;
            }
        }

        requiredItem.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }

    public Room ParentRoom { get; set; }
}