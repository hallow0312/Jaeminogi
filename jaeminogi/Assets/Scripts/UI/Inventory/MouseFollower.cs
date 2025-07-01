using Inventory.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // Input System ���

public class MouseFollower : MonoBehaviour, IDragHandler
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private UI_InventoryItem _inventoryItem;

    private bool _isFollowing;

    public void Init()
    {
        _isFollowing = false;


        _canvas.worldCamera = Camera.main;

        _inventoryItem = GetComponentInChildren<UI_InventoryItem>();
        _inventoryItem.Initialize();
        if (_inventoryItem == null)
        {
            Debug.LogError("MouseFollower: UIInventoryItem�� �������� �ʽ��ϴ�! �ν����Ϳ��� Ȯ���ϼ���.");

        }
        StopFollowing();
    }

    void Update()
    {
        if (!_isFollowing) return;
        Vector2 mousePos = Mouse.current.position.ReadValue(); // ���콺 ��ǥ ��������
        transform.position = mousePos; //  ���� ��ġ �Ҵ�!
    }




    public void StartFollowing(Sprite sprite, int quantity)
    {
        gameObject.SetActive(true);
        _isFollowing = true;
        _inventoryItem.SettingSlot(sprite, quantity);
    }

    public void StopFollowing()
    {
        gameObject.SetActive(false);
        _isFollowing = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        RectTransform rectTransform = transform as RectTransform;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            (RectTransform)_canvas.transform,
            eventData.position,
            _canvas.worldCamera,
            out localPoint);

        rectTransform.anchoredPosition = localPoint;
    }

}
