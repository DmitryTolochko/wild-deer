using Model.Inventory;
using ServiceInstances;
using UnityEngine;
using UnityEngine.UI;

public class ItemInstance
{
    public string ItemName;
    public string Description;
    public int Price;
    public int InventoryCount;
    public Sprite Image;
    public BoosterType BoosterType;

    public ItemInstance(
        string itemName,
        string description,
        int price,
        int inventoryCount,
        BoosterType boosterType,
        Sprite image)
    {
        ItemName = itemName;
        Description = description;
        Price = price;
        InventoryCount = inventoryCount;
        BoosterType = boosterType;
        Image = image;
    }
}

public class ItemCard : MonoBehaviour
{
    private Button buyButton;
    private Text priceElement;
    private Text itemNameElement;
    private Image itemImageElement;
    private Text descriptionElement;
    private Image lockIconElement;
    private Text inventoryCountElement;
    private BoosterType boosterType;

    private int price;

    private void Start()
    {
        buyButton = transform.Find("Buy").gameObject.GetComponent<Button>();
        priceElement = transform.Find("Buy").transform.Find("Price").GetComponent<Text>();
        itemNameElement = transform.Find("ItemName").GetComponent<Text>();
        itemImageElement = transform.Find("Image").GetComponent<Image>();
        descriptionElement = transform.Find("Description").GetComponent<Text>();
        lockIconElement = transform.Find("Buy").transform.Find("LockIcon").GetComponent<Image>();
        inventoryCountElement = transform.Find("InventoryCount").GetComponent<Text>();

        buyButton.onClick.AddListener(OnButtonClick);
    }

    public void ChangeItem(ItemInstance instance)
    {
        boosterType = instance.BoosterType;
        itemNameElement.text = instance.ItemName;
        itemImageElement.sprite = instance.Image;
        descriptionElement.text = instance.Description;
        inventoryCountElement.text =
            "В инвентаре: " + (Inventory.TryGetItem(boosterType, out var item) ? item.Amount : 0);
        price = instance.Price;
        priceElement.text = price.ToString();
        GetAccessToButton();
    }

    public void GetAccessToButton()
    {
        if (GameModel.Balance < price)
        {
            buyButton.interactable = false;
            lockIconElement.gameObject.SetActive(true);
        }
        else
        {
            buyButton.interactable = true;
            lockIconElement.gameObject.SetActive(false);
        }
    }

    public void OnButtonClick()
    {
        GameModel.Balance -= price;
        Cards.MoneyCountElement.text = GameModel.Balance.ToString();
        Cards.IsMoneyChanged = true;
        Inventory.AddItem(boosterType);
        inventoryCountElement.text = "В инвентаре: " + 
                                     (Inventory.TryGetItem(boosterType, out var item) 
                                         ? item.Amount 
                                         : 0);
        //inventoryCountElement.text = "В инвентаре: " + instance.InventoryCount.ToString();
    }
}