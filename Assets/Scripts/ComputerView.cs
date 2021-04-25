using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ComputerView : MonoBehaviour
{
    public event Action OnUIShown;
    public event Action OnGameplayShown;
    
    [SerializeField] private Camera _camera = default;
    [SerializeField] private Camera _uiCamera = default;
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
        
        ShowUI();
    }

    public void TurnOn()
    {
        _isSwitchedOn = true;
        ShowUI();
    }

    public void ShowUI()
    {
        if (!_isSwitchedOn)
            return;

        if (_uiCamera != null)
        {
            SwitchCamera(_uiCamera);
            if (InterfaceController != null)
                InterfaceController.Activate();
            OnUIShown?.Invoke();
        }
        else
            ShowGameplay();
    }
    
    public void ShowGameplay()
    {
        if (!_isSwitchedOn)
            return;

        SwitchCamera(_camera);
        if (InterfaceController != null)
            InterfaceController.Deactivate();
        OnGameplayShown?.Invoke();
    }

    private void SwitchCamera(Camera activeCamera)
    {
        if (_uiCamera != null)
            _uiCamera.enabled = activeCamera == _uiCamera;
        
        _camera.enabled = activeCamera == _camera;
        activeCamera.targetTexture = _renderTexture;
    }
}