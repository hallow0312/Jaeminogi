using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUpItem : InteractableObject,IPickable
{
    [SerializeField] int _itemID;
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
