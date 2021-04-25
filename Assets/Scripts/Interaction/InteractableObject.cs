using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    private Outline _outline;

    public static Color InteractiveColor = new Color(1f, 1f, 1f);
    public static Color InteractiveWithRequirements = new Color(1, 0.4f, 0);


    public virtual void OnValidate()
    {
    }

    public void Focus(bool on, Player activePlayer)
    {
        if (on && _outline == null)
        {
            _outline = gameObject.AddComponent<Outline>();
        }

        else if (_outline && !on)
        {
            Destroy(_outline);
            _outline = null;
        }

        if (on)
        {
            var canInteract = CanInteract(activePlayer);
            _outline.OutlineColor = canInteract ? InteractiveColor : InteractiveWithRequirements;
        }
    }

    public virtual bool CanInteract(Player player)
    {
        return true;
    }

    public virtual void Interact(Player player)
    {
    }

    public abstract void AcceptRequiredItem(Transferrable requiredItem);
}


public interface IRoomie
{
    Room ParentRoom { get; set; }
}