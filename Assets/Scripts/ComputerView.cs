using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ComputerView : MonoBehaviour, IRoomie
{
    public event Action OnUIShown;
    public event Action OnGameplayShown;

    private Camera Camera => ParentRoom.NextRoom.Camera;
    [SerializeField] private int _resolutionWidth = 1024;
    [SerializeField] private int _resolutionHeight = 768;
    public InterfaceController InterfaceController = default;

    private Renderer _renderer;
    private RenderTexture _renderTexture;

    public bool IsSwitchedOn { get; private set; }

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderTexture = new RenderTexture(_resolutionWidth, _resolutionHeight, 24);
        _renderer.material.SetTexture("_EmissionMap", _renderTexture);        
        InterfaceController.OnResumeClicked += ResumeHandler;
        InterfaceController.Init(ParentRoom);

        ShowUI(false);
    }

    private void ResumeHandler()
    {
        ShowGameplay();
    }

    public void SetTurnedOnStatus(bool turnedOn)
    {
        IsSwitchedOn = turnedOn;
        ShowUI(false);
    }

    public void ShowUI(bool activateInput = true)
    {
        if (!IsSwitchedOn)
            return;

        if (InterfaceController != null)
        {
            SwitchCamera(InterfaceController.UICamera);
            if (activateInput)
                InterfaceController.Activate();
            OnUIShown?.Invoke();
        }
        else
            ShowGameplay();
    }

    public void HandleEnter()
    {
        if (!InterfaceController.HandleEnter())
            return;

        // ShowGameplay();
    }
    
    public void ShowGameplay()
    {
        if (!IsSwitchedOn)
            return;

        SwitchCamera(Camera);
        if (InterfaceController != null)
            InterfaceController.Deactivate();
        OnGameplayShown?.Invoke();
    }

    private void SwitchCamera(Camera activeCamera)
    {
        if (InterfaceController != null)
            InterfaceController.UICamera.enabled = activeCamera == InterfaceController.UICamera;

        Camera.enabled = activeCamera == Camera;
        activeCamera.targetTexture = _renderTexture;
    }

    private void Init()
    {
        IsSwitchedOn = ParentRoom.PowerIsOn;
    }
    
    private Room parentRoom;
    
    public Room ParentRoom
    {
        get => parentRoom;
        set
        {
            parentRoom = value;
            Init();
        }
    }
}