using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Outline = cakeslice.Outline;

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
            if (_interactable == value)
                return;

            if (_interactable)
            {
                var o = _interactable.GetComponentInChildren<Outline>(true);

                if (o)
                {
                    GameObject.Destroy(o);
                    GameManager.Instance.ActivePlayer.Outliner.RemoveOutline(o);
                }
            }

            _interactable = value;

            if (_interactable)
            {
                var outlineNew = _interactable.gameObject.GetComponentInChildren<Outline>();

                if (outlineNew == null)
                {
                    var renderer = _interactable.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                        outlineNew = renderer.gameObject.AddComponent<Outline>();
                }

                if (outlineNew)
                {
                    outlineNew.color = _interactable.CanInteract(ActivePlayer) ? 1 : 0;
                    GameManager.Instance.ActivePlayer.Outliner.AddOutline(outlineNew);
                }

            }

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
