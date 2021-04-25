using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class InteractableObject : MonoBehaviour
{
    private Outline _outline;
    public Outline Outline => _outline;

    public static Color InteractiveColor = new Color(1f, 1f, 1f);
    public static Color InteractiveWithRequirements = new Color(1, 0.4f, 0);

    public Transferrable RequiresItem;


    private void OnValidate()
    {
        if (_outline != null) return;
        _outline = GetComponent<Outline>();
        _outline.enabled = false;
    }

    public void Focus(bool on, Player activePlayer)
    {
        Outline.enabled = on;

        if (on)
        {
            var canInteract = CanInteract(activePlayer);
            Outline.OutlineColor = canInteract ? InteractiveColor : InteractiveWithRequirements;
            Outline.OutlineWidth = canInteract && RequiresItem != null ? 10 : 4;
        }
    }

    public virtual bool CanInteract(Player player)
    {
        return RequiresItem == null || player.Burden == RequiresItem;
    }

    public virtual void Interact(Player player)
    {
        if (RequiresItem && player.Burden == RequiresItem)
        {
            player.Burden = null;
            AcceptRequiredItem(RequiresItem);
        }
    }

    public abstract void AcceptRequiredItem(Transferrable requiredItem);
}
