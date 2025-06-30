using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    int _index = 20;
    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();

    GameObject _fadeImage;

    public UI_Scene SceneUI { get; set; }
    

    private GameObject _uiRoot;

    public GameObject UIRoot
    {
        get
        {
            if (_uiRoot == null)
            {
                _uiRoot = GameObject.Find("UI_Root");
                if (_uiRoot == null)
                {
                    _uiRoot = new GameObject { name = "UI_Root" };

                }
            }
            return _uiRoot;
        }
    }

    public void SetCanvas(GameObject obj, bool sort = true)
    {
        Canvas canvas = Utils.GetOrAddComponent<Canvas>(obj);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;
        canvas.sortingOrder = sort ? _index++ : 0;
    }

    public void ShowSceneUI<T>(string key = null, Action<T> callback = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(key))
            key = typeof(T).Name;

        Managers.Resource.Instantiate(key, UIRoot.transform, (obj) =>
        {
            T sceneUI = Utils.GetOrAddComponent<T>(obj);
            SceneUI = sceneUI;
            callback?.Invoke(sceneUI);
        });
    }

    public void ShowPopupUI<T>(string key = null, Transform parent = null, Action<T> callback = null) where T : UI_PopUp
    {
        if (string.IsNullOrEmpty(key))
            key = typeof(T).Name;

        Managers.Resource.Instantiate(key, null, (obj) =>
        {
            T popup = Utils.GetOrAddComponent<T>(obj);
            _popupStack.Push(popup);
            obj.transform.SetParent(parent != null ? parent : UIRoot.transform, false);
            callback?.Invoke(popup);

        });

    }

    public void ClosePopupUI(UI_PopUp popup)
    {
        if (_popupStack.Count == 0 || _popupStack.Peek() != popup)
        {
            Debug.Log("Close Popup Failed!");
            return;
        }
        ClosePopupUI();
    }
  

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        _index--;
    }

    public void CloseAllPopUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
        SceneUI = null;
    }

    public bool HasPopUpUI()
    {
        return _popupStack.Count > 0;
    }

    public void ClearUI()
    {
        CloseAllPopUI();

        
        if (SceneUI != null)
        {
            Destroy(SceneUI.gameObject);
            SceneUI = null;
        }
     
        CloseAllPopUI();
    }
}
