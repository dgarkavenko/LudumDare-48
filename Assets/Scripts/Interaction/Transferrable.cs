using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transferrable : InteractableObject
{
    public override bool CanInteract(PlayerInteractionController interactionOwner)
    {
        return true;
    }

    public override void Interact(PlayerInteractionController interactionOwner)
    {
        interactionOwner.Transfer(this);
    }
}
