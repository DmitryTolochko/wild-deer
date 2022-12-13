using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public enum TaskType
{
    CollectFood,
    DealWithThreat,
    WaterDeer,
    FeedDeer,
    GainDeers,
    ApplyTrap,
    //HealDeer
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
                GameModel.ActualTasks.Add(new TaskInstance(
                    TaskType.CollectFood, 
                    "Собрать ягель", 
                    1, 
                    1, 
                    200,
                    Resources.Load<Sprite>("DeerIcon")));

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

    private TaskInstance GetNewTask()
    {
        while (true)
        {
            var task = GetNextTask();
            if (GameModel.ActualTasks.Any(x => x.TaskType == task.TaskType))
                continue;
            return task;
        }
    }

    private static TaskInstance GetNextTask()
    {
        var num = 0;
        switch (Random.Range(0, 6))
        {
            case 0:
                num = Random.Range(1, 10);
                return new TaskInstance(TaskType.CollectFood, "Собрать ягель", num, 0, 50 * num,
                    Resources.Load<Sprite>("DeerIcon"));
            case 1:
                num = Random.Range(1, 3);
                return new TaskInstance(TaskType.DealWithThreat, "Справиться с угрозой", num, 0, 200 * num,
                    Resources.Load<Sprite>("DeerIcon"));
            case 2:
                num = Random.Range(1, 5);
                return new TaskInstance(TaskType.WaterDeer, "Напоить оленя", num, 0, 50 * num,
                    Resources.Load<Sprite>("DeerIcon"));
            case 3:
                num = Random.Range(1, 5);
                return new TaskInstance(TaskType.FeedDeer, "Накормить оленя", num, 0, 50 * num,
                    Resources.Load<Sprite>("DeerIcon"));
            case 4:
                num = Random.Range(1, 5);
                return new TaskInstance(TaskType.GainDeers, "Увеличьте популяцию", num, 0, 25 * num,
                    Resources.Load<Sprite>("DeerIcon"));
            // case 5:
            //     return new TaskInstance(TaskType.HealDeer, "Вылечить оленя", Random.Range(1, 2), 0, 50,
            //         Resources.Load<Sprite>("DeerIcon"));
            default:
                num = Random.Range(1, 4);
                return new TaskInstance(TaskType.ApplyTrap, "Примените капкан", num, 0, 20 * num,
                    Resources.Load<Sprite>("DeerIcon"));
        }
    }
}