using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractionController : MonoBehaviour
{

    public Player ActivePlayer => GameManager.Instance.ActivePlayer;

    [SerializeField] private Transform _mainCharTransform = default;

    [SerializeField] private Image _targetMarkerObject = default;
    [SerializeField] private Color _defaultColor = Color.white;
    [SerializeField] private Color _interactionColor = Color.green;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _radius;
    [SerializeField] private Vector3 _transferOffset = new Vector3(1, 1, 1);

    public Transferrable Transferrable { get; private set; }
    private Transform _transferTransform;
    private RaycastHit _hit;

    private InteractableObject _interactable;
    public InteractableObject Interactable
    {
        get => _interactable;
        set
        {
            _interactable = value;
            ActivePlayer.Outliner.Selection = _interactable;

            if (_interactable)
                ActivePlayer.Outliner.CanInteract = _interactable.CanInteract(ActivePlayer);
        }
    }

    public LayerMask InteractiveLayer;

    private void Update()
    {
        var _camera = ActivePlayer.Camera;

        var center = new Vector3(_camera.pixelWidth / 2, _camera.pixelHeight / 2, 0);
        var ray = _camera.ScreenPointToRay(center);

        var hit = Physics.SphereCast(ray, _radius, out _hit, _maxDistance, layerMask: InteractiveLayer);

        Interactable = hit ? _hit.transform.GetComponent<InteractableObject>() : null;

        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E)))
            if (Interactable)
                Interact();
            else
                ActivePlayer.DropBurden();

    }

    private void Interact()
    {
        if (Interactable.CanInteract(ActivePlayer))
            Interactable.Interact(ActivePlayer);
    }

}
