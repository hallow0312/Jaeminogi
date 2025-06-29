using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUpObject : InteractableObject,IPickable
{
    public void PickUp()
    {
        Debug.Log($"{ItemName}: PickUp");
        Destroy(gameObject);
    }

    public override void Interact()
    {
        PickUp();
    }

}
