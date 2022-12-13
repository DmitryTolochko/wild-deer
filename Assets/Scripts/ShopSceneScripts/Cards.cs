using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Model.Inventory;
using ServiceInstances;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
    private Queue<ItemInstance> nonActiveCards = new Queue<ItemInstance>();

    public static Text MoneyCountElement;
    public static bool IsMoneyChanged;

    private Button backButton;
    private Button previousButton;
    private Button nextButton;

    private void Start()
    {
        backButton = transform.Find("BackButton").gameObject.GetComponent<Button>();
        backButton.onClick.AddListener(delegate() { UnloadThisScene(); });

        previousButton = transform.Find("PreviousButton").gameObject.GetComponent<Button>();
        previousButton.onClick.AddListener(delegate() { MoveCardsLeft(); });

        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        nextButton.onClick.AddListener(delegate() { MoveCardsRight(); });

        MoneyCountElement = transform.Find("MoneyCount").GetComponent<Text>();
        MoneyCountElement.text = GameModel.Balance.ToString();
        print(backButton);
        SetCards();
    }

    private void Update()
    {
        if (IsMoneyChanged)
        {
            IsMoneyChanged = false;
            transform.Find("FirstItem").GetComponent<ItemCard>().GetAccessToButton();
            transform.Find("SecondItem").GetComponent<ItemCard>().GetAccessToButton();
            transform.Find("ThirdItem").GetComponent<ItemCard>().GetAccessToButton();
        }
    }

    private void UnloadThisScene()
    {
        SceneManager.UnloadSceneAsync("ShopScene");
        Time.timeScale = 1;
    }

    private void SetCards()
    {
        nonActiveCards.Enqueue(new ItemInstance(
            "Еда",
            "Нужна для того, чтобы кормить оленей.",
            20,
            0,
            BoosterType.Food,
            Resources.Load<Sprite>("FoodIcon")
        ));

        nonActiveCards.Enqueue(new ItemInstance(
            "Капкан",
            "Поможет вам в борьбе с браконьерами, волками и другими врагами.",
            50,
            0,
            BoosterType.PinkTrap,
            Resources.Load<Sprite>("TrapIcon")
        ));

        nonActiveCards.Enqueue(new ItemInstance(
            "Барьер",
            "На время предотвращает все нападения.",
            150,
            0,
            BoosterType.ProtectiveCap,
            Resources.Load<Sprite>("ProtectiveCapIcon")
        ));

        nonActiveCards.Enqueue(new ItemInstance(
            "Лекарство",
            "Лечит оленя.",
            50,
            0,
            BoosterType.Medicines,
            Resources.Load<Sprite>("MedicinesIcon")
        ));

        nonActiveCards.Enqueue(new ItemInstance(
            "Вода",
            "Служит для утоления жажды оленя.",
            25,
            0,
            BoosterType.Water,
            Resources.Load<Sprite>("WaterIcon")
        ));

        MoveCardsRight();
    }

    private void MoveCardsLeft()
    {
        for (var i = 0; i < nonActiveCards.Count - 1; i++)
        {
            var card = nonActiveCards.Dequeue();
            nonActiveCards.Enqueue(card);
        }

        var cards = nonActiveCards.ToArray();
        transform.Find("FirstItem").GetComponent<ItemCard>().ChangeItem(cards[0]);
        transform.Find("SecondItem").GetComponent<ItemCard>().ChangeItem(cards[1]);
        transform.Find("ThirdItem").GetComponent<ItemCard>().ChangeItem(cards[2]);
    }

    private void MoveCardsRight()
    {
        var card = nonActiveCards.Dequeue();
        nonActiveCards.Enqueue(card);
        var cards = nonActiveCards.ToArray();
        transform.Find("FirstItem").GetComponent<ItemCard>().ChangeItem(cards[0]);
        transform.Find("SecondItem").GetComponent<ItemCard>().ChangeItem(cards[1]);
        transform.Find("ThirdItem").GetComponent<ItemCard>().ChangeItem(cards[2]);
    }
}