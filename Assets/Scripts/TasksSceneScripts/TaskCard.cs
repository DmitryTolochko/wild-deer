using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCard : MonoBehaviour
{
    private Button getMoneyButton;
    private Text priceElement;
    private Text taskNameElement;
    private Image taskImageElement;
    private Text progressElement;
    private Image doneIconElement;

    public bool IsNotActual;
    public int Count = 2;
    public int DoneCount;

    private int price;

    private void Start() 
    {
        getMoneyButton = transform.Find("GetMoney").gameObject.GetComponent<Button>();
        priceElement = transform.Find("Money").GetComponent<Text>();
        taskNameElement = transform.Find("TaskName").GetComponent<Text>();
        taskImageElement = transform.Find("Image").GetComponent<Image>();
        progressElement = transform.Find("Progress").GetComponent<Text>();
        doneIconElement = transform.Find("DoneIcon").GetComponent<Image>();

        getMoneyButton.onClick.AddListener(delegate() { OnButtonClick(); });

        IsNotActual = true;
    }

    private void Update() 
    {
        // отладка
        ChangeProgress(DoneCount);
        if (DoneCount == Count)
        {
            doneIconElement.gameObject.SetActive(true);
            getMoneyButton.interactable = true;
            getMoneyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GetMoneyImg");
        }
    }

    public void ChangeTask(string taskName, int count, int price, Sprite image)
    {
        doneIconElement.gameObject.SetActive(false);

        IsNotActual = false;

        taskNameElement.text = taskName;
        this.price = price;
        priceElement.text = price.ToString();
        taskImageElement.sprite = image;
        Count = count;

        ChangeProgress(0);
        getMoneyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GetMoneyBlockedImg");
        getMoneyButton.interactable = false;
    }

    public void ChangeProgress(int doneCount)
    {
        DoneCount = doneCount;
        progressElement.text = DoneCount.ToString() + '/' + Count.ToString();
    }

    public void OnButtonClick()
    {
        IsNotActual = true;
        getMoneyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GetMoneyCollectedImg");
        getMoneyButton.interactable = false;

        GameModel.Balance += price;
        Tasks.MoneyCountElement.text = GameModel.Balance.ToString();
    }
}
