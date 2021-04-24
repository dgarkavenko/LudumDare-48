using UnityEngine;

public class MouseInteractionDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;
    
    public bool AnyHitTarget { get; private set; }
    public InteractableObject Interacteable { get; private set; }

    private RaycastHit _hit;

    private void Update()
    {
        var center = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        var ray = _camera.ScreenPointToRay(center);

        var anyHit = Physics.Raycast(ray, out _hit);

        Interacteable = anyHit ? _hit.transform.GetComponent<InteractableObject>() : null;
        AnyHitTarget = Interacteable != null;
    }
}
