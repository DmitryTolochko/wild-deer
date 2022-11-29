using System.Collections;
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
    private HashSet<TaskCard> cards = new HashSet<TaskCard>();

    //public static int MoneyCount = 0;
    public static Text MoneyCountElement;

    private Button backButton;

    private void Start() 
    {
        backButton = transform.Find("BackButton").gameObject.GetComponent<Button>();
        backButton.onClick.AddListener( delegate() { UnloadThisScene(); });

        MoneyCountElement = GameObject.Find("MoneyCount").GetComponent<Text>();
        MoneyCountElement.text = GameModel.Balance.ToString();

        cards.Add(transform.Find("TaskFirst").GetComponent<TaskCard>());
        cards.Add(transform.Find("TaskSecond").GetComponent<TaskCard>());
        cards.Add(transform.Find("TaskThird").GetComponent<TaskCard>());
    }

    private void Update() 
    {
        foreach (var card in cards)
        {
            if (card.IsNotActual)
                GetNewTask(card);
        }
    }

    public void UnloadThisScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    private void GetNewTask(TaskCard card)
    {
        switch (Random.Range(0, 6))
        {
            case 0:
                card.ChangeTask("Собрать ягель", Random.Range(1, 10), 6, Resources.Load<Sprite>("DeerIcon"));
                break;
            case 1:
                card.ChangeTask("Справиться с угрозой", Random.Range(1, 3), 10, Resources.Load<Sprite>("DeerIcon"));
                break;
            case 2:
                card.ChangeTask("Напоить оленя", Random.Range(1, 5), 15, Resources.Load<Sprite>("DeerIcon"));
                break;
            case 3:
                card.ChangeTask("Накормить оленя", Random.Range(1, 5), 11, Resources.Load<Sprite>("DeerIcon"));
                break;
            case 4:
                card.ChangeTask("Увеличте популяцию", Random.Range(1, 5), 25, Resources.Load<Sprite>("DeerIcon"));
                break;
            case 5:
                card.ChangeTask("Примените капкан", Random.Range(1, 4), 6, Resources.Load<Sprite>("DeerIcon"));
                break;
        }
    }
}
