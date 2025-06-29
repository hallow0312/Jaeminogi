using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{

    public string ItemName;
    public string GetInteractName()
    {
        return ItemName;
    }

    public abstract void Interact();
 
}