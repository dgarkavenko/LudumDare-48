using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public abstract bool CanInteract(PlayerInteractionController interactionOwner);
    
    public abstract void Interact(PlayerInteractionController interactionOwner);
}
