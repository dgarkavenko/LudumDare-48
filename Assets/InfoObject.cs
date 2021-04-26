using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoObject : InteractableObject
{
    public override void AcceptRequiredItem(Transferrable requiredItem)
    {
        throw new System.NotImplementedException();
    }

    public string Description;
}
