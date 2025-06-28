using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_INFO : UI_Base
{
    enum Texts
    {
        UI_INFO
    }
    public override bool Init()
    {
        if (base.Init() == false) return false;
        BindText(typeof(Texts));
        ResetText();
        return true;
    }
    public void ResetText()
    {
        GetText((int)Texts.UI_INFO).text = "";
    }
    public void SetInfo(string info)
    {
        GetText((int)Texts.UI_INFO).text = info;
    }
}
