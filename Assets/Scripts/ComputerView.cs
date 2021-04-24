using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ComputerView : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    [SerializeField] private Camera _uiCamera = default;
    [SerializeField] private int _resolutionWidth = 1024;
    [SerializeField] private int _resolutionHeight = 768;
    [SerializeField] private bool _isSwitchedOn = true; 
    
    private Renderer _renderer;
    private RenderTexture _renderTexture;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _renderTexture = new RenderTexture(_resolutionWidth, _resolutionHeight, 24);
        _renderer.material.mainTexture = _renderTexture;
        
        RunUI();
    }

    public void TurnOn()
    {
        _isSwitchedOn = true;
        
        RunUI();
    }

    public void RunUI()
    {
        if (!_isSwitchedOn)
            return;
        
        SwitchCamera(_uiCamera);
    }
    
    public void RunGameplay()
    {
        if (!_isSwitchedOn)
            return;

        SwitchCamera(_camera);
    }

    private void SwitchCamera(Camera activeCamera)
    {
        if (_uiCamera != null)
            _uiCamera.enabled = activeCamera == _uiCamera;
        
        _camera.enabled = activeCamera == _camera;
        activeCamera.targetTexture = _renderTexture;
    }
}