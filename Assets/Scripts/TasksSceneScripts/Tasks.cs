using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum TaskType
{
    CollectFood,
    DealWithThreat,
    WaterDeer,
    FeedDeer,
    GainDeers,
    ApplyTrap
}

public class Tasks : MonoBehaviour
{
    private List<TaskCard> cards = new List<TaskCard>();

    public static Text MoneyCountElement;

    private Button backButton;
    private bool IsReloaded;

    private void Start()
    {
        backButton = transform.Find("BackButton").gameObject.GetComponent<Button>();
        backButton.onClick.AddListener(delegate() { UnloadThisScene(); });

        MoneyCountElement = transform.Find("MoneyCount").GetComponent<Text>();
        MoneyCountElement.text = GameModel.Balance.ToString();

        cards.Add(transform.Find("TaskFirst").GetComponent<TaskCard>());
        cards.Add(transform.Find("TaskSecond").GetComponent<TaskCard>());
        cards.Add(transform.Find("TaskThird").GetComponent<TaskCard>());
        ReloadTasksCards();
    }

    private void Update()
    {
        foreach (var card in cards)
        {
            if (card.IsNotActual)
                ReloadTasksCards();
        }
    }

    public void ReloadTasksCards()
    {
        for (var i = 0; i < 3; i++)
        {
            if (GameModel.ActualTasks.Count == 3 && GameModel.ActualTasks[i].IsNotActual)
            {
                GameModel.ActualTasks[i] = GetNewTask();
            }
            else if (GameModel.ActualTasks.Count == 0)
            {
                GameModel.ActualTasks.Add(GetNewTask());
                GameModel.ActualTasks.Add(GetNewTask());
                GameModel.ActualTasks.Add(GetNewTask());
            }

            cards[i].ChangeTask(GameModel.ActualTasks[i]);
        }
    }

    public void UnloadThisScene()
    {
        SceneManager.UnloadSceneAsync("TasksScene");
        Time.timeScale = 1;
    }

    private static TaskInstance GetNewTask()
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                return new TaskInstance(TaskType.CollectFood, "Собрать ягель", Random.Range(1, 10), 0, 6,
                    Resources.Load<Sprite>("DeerIcon"));
            case 1:
                return new TaskInstance(TaskType.DealWithThreat, "Справиться с угрозой", Random.Range(1, 3), 0, 200,
                    Resources.Load<Sprite>("DeerIcon"));
            case 2:
                return new TaskInstance(TaskType.WaterDeer, "Напоить оленя", Random.Range(1, 5), 0, 50,
                    Resources.Load<Sprite>("DeerIcon"));
            case 3:
                return new TaskInstance(TaskType.FeedDeer, "Накормить оленя", Random.Range(1, 5), 0, 50,
                    Resources.Load<Sprite>("DeerIcon"));
            case 4:
                return new TaskInstance(TaskType.GainDeers, "Увеличьте популяцию", Random.Range(1, 5), 0, 25,
                    Resources.Load<Sprite>("DeerIcon"));
            default:
                return new TaskInstance(TaskType.ApplyTrap, "Примените капкан", Random.Range(1, 4), 0, 20,
                    Resources.Load<Sprite>("DeerIcon"));
        }
    }
}