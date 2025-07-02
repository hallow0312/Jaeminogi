using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class UI_ItemInfo : UI_Base
{
    enum Images
    {
        ItemImage
    }
    enum Texts
    {
        ItemName
    }
    [SerializeField]
    private Sprite _emptySprite; 
    public override bool Init()
    {
        if (base.Init() == false) return false;

        BindImage(typeof(Images));
        BindText(typeof(Texts));
        ResetInfo();
        return true;
    }
    public void SetItemInfo(Sprite sprite, string name)
    {
        if(sprite!=null)
        GetImage((int)Images.ItemImage).sprite = sprite;
        
        GetText((int)Texts.ItemName).name = name;
    }
    public void ResetInfo()
    {
        GetImage((int)Images.ItemImage).sprite = _emptySprite;
        GetText((int)Texts.ItemName).name = "";
        gameObject.SetActive(false);
    }
}

