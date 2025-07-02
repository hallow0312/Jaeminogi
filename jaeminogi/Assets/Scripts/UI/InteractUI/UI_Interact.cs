using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Interact : UI_Base
{
    enum GameObjects
    {
        ItemUI
    }
    UI_ItemInfo _itemInfo;
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindObject(typeof(GameObjects));
        return true; 
    }
    void SettingComponent()
    {
        _itemInfo=GetObject((int)GameObjects.ItemUI).GetComponent<UI_ItemInfo>();   
    }
    public void ShowItemInfo(Sprite sprite, string name)
    {
        if(GetObject((int)GameObjects.ItemUI).activeSelf==false)
           GetObject((int)GameObjects.ItemUI).SetActive(true);

        _itemInfo.SetItemInfo(sprite, name);   
    }
    public void HideItemInfo()
    {
        if (GetObject((int)GameObjects.ItemUI).activeSelf == true)
            _itemInfo.ResetInfo();
    }
   
    
}
