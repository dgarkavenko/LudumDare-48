using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ComputerView : MonoBehaviour
{
    public event Action OnUIShown;
    public event Action OnGameplayShown;

    private Camera Camera => GameManager.Instance.NextRoom.Camera;
    [SerializeField] private int _resolutionWidth = 1024;
    [SerializeField] private int _resolutionHeight = 768;
    [SerializeField] private bool _isSwitchedOn = true;
    public InterfaceController InterfaceController = default;

    private Renderer _renderer;
    private RenderTexture _renderTexture;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderTexture = new RenderTexture(_resolutionWidth, _resolutionHeight, 24);
        _renderer.material.mainTexture = _renderTexture;

        ShowUI(false);
    }

    public void TurnOn()
    {
        _isSwitchedOn = true;
        ShowUI(false);
    }

    public void ShowUI(bool activateInput = true)
    {
        if (!_isSwitchedOn)
            return;

        if (InterfaceController != null)
        {
            SwitchCamera(InterfaceController.UICamera);
            if (activateInput)
                InterfaceController.enabled = true;
            OnUIShown?.Invoke();
        }
        else
            ShowGameplay();
    }

    public void ShowGameplay()
    {
        if (!_isSwitchedOn)
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
}