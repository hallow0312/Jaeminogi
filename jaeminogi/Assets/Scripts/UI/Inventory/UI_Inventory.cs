using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UI_Inventory : UI_Base
    {
        public enum GameObjects
        {
            InventoryContent,

            MouseFollower,
    
        }

        [SerializeField] RectTransform _contentPanel;

        [SerializeField] MouseFollower _mouseFollower;
        [SerializeField] int _inventorySize = 21;

        List<UI_InventoryItem> _listOfUiItems = new List<UI_InventoryItem>();

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnstartDragging;
        public event Action<int, int> OnSwapItems;

        int _currentlyDraggedItemIndex = -1;

        public override bool Init()
        {
            if (base.Init() == false) return false;
            BindObject(typeof(GameObjects));

            _contentPanel = Utils.FindChild(GetObject((int)GameObjects.InventoryContent), "BG", true).GetComponent<RectTransform>();
            _mouseFollower = GetObject((int)GameObjects.MouseFollower)?.GetComponent<MouseFollower>();
            _mouseFollower?.Init();

            InitializeInventoryUI(_inventorySize);
            Hide();
            return true;
        }
            

        public void InitializeInventoryUI(int inventorySize)
        {
            int count = 0;
            for (int i = 0; i < inventorySize; i++)
            {
                Managers.Resource.Instantiate("Slot", callback: (obj) =>
                {

                    UI_InventoryItem uiItem = obj.GetComponent<UI_InventoryItem>();
                    uiItem.transform.SetParent(_contentPanel);

                    uiItem.transform.localScale = Vector3.one;
                    if (uiItem == null)
                    {
                        Debug.LogError($"[InstantiateUIItem] {obj}에서 UIInventoryItem을 찾을 수 없습니다.");
                        return;
                    }

                    _listOfUiItems.Add(uiItem);

                    // 이벤트 핸들링
                    uiItem.OnItemClicked += HandleItemSelection;
                    uiItem.OnItemBeginDrag += HandleBeginDrag;
                    uiItem.OnItemDroppedOn += HandleSwap;
                    uiItem.OnItemEndDrag += HandleEndDrag;
                    uiItem.OnRightMouseButtonClick += HandleShowItemActions;

                    uiItem.Initialize();

                    count++;
                    if (count == inventorySize)
                        Hide();
                });
            }
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (_listOfUiItems.Count > itemIndex)
                _listOfUiItems[itemIndex].SettingSlot(itemImage, itemQuantity);
        }

        private void HandleShowItemActions(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1)
            {
                ResetDraggedItem();
                return;
            }

            OnItemActionRequested?.Invoke(index);
        }

        private void HandleEndDrag(UI_InventoryItem item)
        {
            _mouseFollower?.StopFollowing();
        }

        private void HandleSwap(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1)
            {
                ResetDraggedItem();
                return;
            }
            OnSwapItems?.Invoke(_currentlyDraggedItemIndex, index);
        }

        private void ResetDraggedItem()
        {
            _mouseFollower?.StopFollowing();
            _currentlyDraggedItemIndex = -1;
        }

        private void HandleBeginDrag(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1) return;

            _currentlyDraggedItemIndex = index;
            HandleItemSelection(item);
            OnstartDragging?.Invoke(index);
        }
        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            _mouseFollower?.StartFollowing(sprite, quantity);
        }

        private void HandleItemSelection(UI_InventoryItem item)
        {
            int index = _listOfUiItems.IndexOf(item);
            if (index == -1) return;

            OnDescriptionRequested?.Invoke(index);
        }
        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void ResetSelection()
        {
           
            DeselectAllItems();
        }
        private void DeselectAllItems()
        {
            foreach (var item in _listOfUiItems)
                item?.Deselect();

        }
      
        public void Hide()
        {
            gameObject.SetActive(false);
           
        }

        public void ResetAllItems()
        {
            _listOfUiItems.RemoveAll(item => item == null);
            foreach (var item in _listOfUiItems)
            {
                item.ResetSlot();
                item.Deselect();
            }
        }
    }
}
