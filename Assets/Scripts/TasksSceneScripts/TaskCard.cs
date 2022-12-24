using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Model.Inventory;
using ServiceInstances;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TaskInstance
{
    public TaskType TaskType;
    public string TaskName;
    public bool IsNotActual;
    public int Count;
    public int DoneCount;
    public int Price;
    public Sprite Image;

    public TaskInstance(TaskType taskType, string taskName, int count, int doneCount, int price, Sprite image)
    {
        TaskType = taskType;
        TaskName = taskName;
        IsNotActual = false;
        Count = count;
        DoneCount = doneCount;
        Price = price;
        Image = image;

        HandleTaskEvent();
    }

    private void HandleTaskEvent()
    {
        switch (TaskType)
        {
            case TaskType.ApplyTrap:
                Inventory.BoosterUsed += _ => IncrementDoneCount();
                break;
            case TaskType.CollectFood:
                FoodWorld.FoodCollected += _ => IncrementDoneCount();
                break;
            case TaskType.FeedDeer:
                Deer.SomeDearFed += IncrementDoneCount;
                break;
            case TaskType.GainDeers:
                DeerSpawner.DeerSpawned += IncrementDoneCount;
                break;
            case TaskType.WaterDeer:
                Deer.SomeDeerDrank += IncrementDoneCount;
                break;
            case TaskType.DealWithThreat:
                BaseThreat.ThreatDefeated += IncrementDoneCount;
                break;
            /*case TaskType.HealDeer:
                Deer.DeerHealed += IncrementDoneCount;
                break;*/
        }
    }


    private void IncrementDoneCount()
    {
        if (DoneCount < Count)
        {
            DoneCount++;
        }
    }
}

public class TaskCard : MonoBehaviour
{
    private TaskInstance Instance;
    private Button getMoneyButton;
    private Text priceElement;
    private Text taskNameElement;
    private Image taskImageElement;
    private Text progressElement;

    public bool IsNotActual;
    public int Count;
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
    }

    private void Update()
    {
        if (Instance == null)
            IsNotActual = true;
        ChangeProgress();
    }

    public void ChangeTask(TaskInstance instance)
    {
        IsNotActual = false;
        Instance = instance;
        taskNameElement.text = instance.TaskName;
        price = instance.Price;
        priceElement.text = price.ToString();
        taskImageElement.sprite = instance.Image;
        Count = instance.Count;
        DoneCount = instance.DoneCount;

        ChangeProgress();
    }

    private void ChangeProgress()
    {
        if (Instance != null)
            Instance.DoneCount = Instance.DoneCount < DoneCount ? DoneCount : Instance.DoneCount;
        progressElement.text = DoneCount.ToString() + '/' + Count.ToString();
        getMoneyButton.interactable = DoneCount >= Count;
    }

    private void OnButtonClick()
    {
        IsNotActual = true;
        Instance.IsNotActual = true;
        getMoneyButton.interactable = false;

        GameModel.Balance += price;
        Tasks.MoneyCountElement.text = GameModel.Balance.ToString();
        Instance = null;
    }
}