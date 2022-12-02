using Model.Inventory;
using UnityEngine;
using UnityEngine.UI;
using Color = System.Drawing.Color;


public class InventoryUI : MonoBehaviour
{
    private Transform itemSlotContainer;
    public static InventoryUI Instance { get; private set; }
    public float Offset = 125f;


    private void Awake()
    {
        Instance = this;
        itemSlotContainer = transform.Find("itemSlotContainer");
    }

    private void Start()
    {
        Inventory.ItemAdded += RefreshInventoryItems;
        Inventory.BoosterUsed += RefreshInventoryItems;
        RefreshInventoryItems();
    }

    public void RefreshInventoryItems()
    {
        var boosterPrefab = BoostersAssets.Instance.boosterPrefab;
        foreach (var child in itemSlotContainer)
        {
            var temp = child as Transform;
            if (temp != boosterPrefab)
            {
                Destroy(temp.gameObject);
            }
        }

        var x = 0;
        foreach (var item in Inventory.GetAllItems())
        {
            var itemType = item.Type;
            var instance = Instantiate(boosterPrefab, itemSlotContainer);
            instance.GetComponent<BoosterWorld>().SetBoosterType(itemType);
            var itemSlotRectTransform = instance.GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition = new Vector2(x * Offset, 0);
            var image = itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite = BoostersAssets.GetSprite(itemType);
            var text = itemSlotRectTransform.Find("itemAmount").GetComponent<Text>();
            text.text = item.Amount > 1 ? $"x{item.Amount.ToString()}" : string.Empty;
            text.color = new UnityEngine.Color(223, 235, 231);
            x++;
        }
    }
}