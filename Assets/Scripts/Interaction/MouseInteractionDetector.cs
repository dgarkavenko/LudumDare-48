using UnityEngine;

public class MouseInteractionDetector : MonoBehaviour
{
    [SerializeField] private Camera _camera = default;

    public bool AnyHitTarget { get; private set; }
    public InteractableObject Interactable { get; private set; }

    public LayerMask InteractiveLayer;
    private RaycastHit _hit;

    private void Update()
    {

    }
}
