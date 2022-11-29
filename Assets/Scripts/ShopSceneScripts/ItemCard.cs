using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInstance
{
    public string ItemName;
    public string Description;
    public int Price;
    public int InventoryCount;
    public Sprite Image;

    public ItemInstance(string itemName, string description, int price, int inventoryCount, Sprite image)
    {
        ItemName = itemName;
        Description = description;
        Price = price;
        InventoryCount = inventoryCount;
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

        buyButton.onClick.AddListener(delegate() { OnButtonClick(); });
    }

    public void ChangeItem(ItemInstance instance)
    {
        itemNameElement.text = instance.ItemName;
        itemImageElement.sprite = instance.Image;
        descriptionElement.text = instance.Description;
        inventoryCountElement.text = instance.InventoryCount.ToString();
        this.price = instance.Price;
        priceElement.text = price.ToString();

        GetAccesToButton();
    }

    public void GetAccesToButton()
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
    }
}
