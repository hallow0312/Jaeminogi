using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interact : UI_Base
{
    enum GameObjects
    {
        ItemUI
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObjects));
        return true; 
    }

}
