using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

namespace Inventory.UI
{
    public class UI_InventoryItem : UI_Base, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        enum GameObjects
        {
            BG,
        }

        enum Images
        {
            ItemImage
        }
        enum Texts
        {
            ItemCount
        }
        [SerializeField] Sprite _emptyImage;

        public event Action<UI_InventoryItem> OnItemClicked, OnItemDroppedOn
             , OnItemBeginDrag, OnItemEndDrag, OnRightMouseButtonClick;
        private bool empty = true;

        public void Initialize()
        {
            BindImage(typeof(Images));
            BindText(typeof(Texts));

            ResetSlot();
            Deselect();
        }
        public void Deselect()
        {

        }

        public void ResetSlot()
        {

            GetImage((int)Images.ItemImage).sprite = _emptyImage;
            GetText((int)Texts.ItemCount).text = "";
            empty = true;

        }
        public void SettingSlot(Sprite sprite, int quantity)
        {
            GetImage((int)Images.ItemImage).gameObject.SetActive(true);
            GetImage((int)Images.ItemImage).sprite = sprite;
            GetText((int)Texts.ItemCount).text = quantity + "";
            
            empty = false;
        }
    


        
        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        

        public void OnDrag(PointerEventData eventData)
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) OnRightMouseButtonClick?.Invoke(this); //정보보기 
            else OnItemClicked?.Invoke(this); //단지 선택하는거 
        }
    }
}