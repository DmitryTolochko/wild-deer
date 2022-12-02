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

        getMoneyButton.onClick.AddListener(delegate() { OnButtonClick(); });

        IsNotActual = true;
    }

    private void Update() 
    {
        // отладка
        ChangeProgress(DoneCount);
        if (DoneCount == Count)
        {
            getMoneyButton.interactable = true;
            getMoneyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GetMoneyButtonOn");
        }
    }

    public void ChangeTask(string taskName, int count, int price, Sprite image)
    {
        IsNotActual = false;

        taskNameElement.text = taskName;
        this.price = price;
        priceElement.text = price.ToString();
        taskImageElement.sprite = image;
        Count = count;

        ChangeProgress(0);
        getMoneyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("GetMoneyButtonOff");
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
        getMoneyButton.GetComponent<Image>().sprite = Resources.Load<Sprite>("DoneButton");
        getMoneyButton.interactable = false;

        GameModel.Balance += price;
        Tasks.MoneyCountElement.text = GameModel.Balance.ToString();
    }
}
