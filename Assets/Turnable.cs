using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turnable : InteractableObject
{
    public enum ETurnableState
    {
        On,
        Off,
        Transition
    }

    [SerializeField] private ETurnableState _state = ETurnableState.Off;
    public bool OnlyTurnsOn;
    public float TransitionTime;
    public Action<ETurnableState> StateChangedAction;
    private Collider[] _colliders;


    public override void OnValidate()
    {
        base.OnValidate();
        _colliders = GetComponentsInChildren<Collider>();
    }

    public ETurnableState State
    {
        get => _state;
        set
        {
            if (_state == value)
                return;

            _state = value;
            StateChanged();
        }
    }

    private void StateChanged()
    {
        StateChangedAction?.Invoke(State);

        if (State == ETurnableState.Off)
            foreach (var c in _colliders)
                c.enabled = true;
        else
            foreach (var c in _colliders)
                c.enabled = false;
    }

    public override void AcceptRequiredItem(Transferrable requiredItem)
    {
        throw new System.NotImplementedException();
    }

    public override void Interact(Player player)
    {
        if (OnlyTurnsOn)
            State = ETurnableState.On;
        else
            StartCoroutine(AnimateTurn());
        base.Interact(player);
    }

    public IEnumerator AnimateTurn()
    {
        var targetState = _state == ETurnableState.On ? ETurnableState.Off : ETurnableState.On;
        State = ETurnableState.Transition;
        while (true)
        {
            yield return new WaitForSeconds(2);
            break;
        }

        State = ETurnableState.On;
    }

    public override bool CanInteract(Player player)
    {
        return base.CanInteract(player) && State != ETurnableState.Transition;
    }
}
