using System.Collections;
using System.Collections.Generic;
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
        backButton.onClick.AddListener( delegate() { UnloadThisScene(); });

        previousButton = transform.Find("PreviousButton").gameObject.GetComponent<Button>();
        previousButton.onClick.AddListener(delegate() { MoveCardsLeft(); });

        nextButton = transform.Find("NextButton").gameObject.GetComponent<Button>();
        nextButton.onClick.AddListener(delegate() { MoveCardsRight(); });

        MoneyCountElement = GameObject.Find("MoneyCount").GetComponent<Text>();
        MoneyCountElement.text = GameModel.Balance.ToString();
        print(backButton);
        SetCards();
    }

    private void Update() 
    {
        if (IsMoneyChanged)
        {
            IsMoneyChanged = false;
            transform.Find("FirstItem").GetComponent<ItemCard>().GetAccesToButton();
            transform.Find("SecondItem").GetComponent<ItemCard>().GetAccesToButton();
            transform.Find("ThirdItem").GetComponent<ItemCard>().GetAccesToButton();
        }
    }

    private void UnloadThisScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void SetCards()
    {
        nonActiveCards.Enqueue(new ItemInstance(
            "Капкан", 
            "Поможет вам в борьбе с браконьерами, волками и другими врагами.",
            30,
            0,
            Resources.Load<Sprite>("ItemBlock1")
        ));

        nonActiveCards.Enqueue(new ItemInstance(
            "Еда", 
            "Нужна для того, чтобы кормить оленей.",
            30,
            0,
            Resources.Load<Sprite>("ItemBlock2")
        ));

        nonActiveCards.Enqueue(new ItemInstance(
            "Барьер", 
            "На время предотвращает все нападения.",
            100,
            0,
            Resources.Load<Sprite>("ItemBlock3")
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
