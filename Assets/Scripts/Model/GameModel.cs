using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model.Boosters;
using ServiceInstances;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameModel : MonoBehaviour
{
    public static HashSet<GameObject> Deers = new();
    public static HashSet<GameObject> Threats = new();
    public static List<TaskInstance> ActualTasks = new();
    public static HashSet<GameObject> FoodSpawned = new();
    public Dictionary<BoosterType, IBooster> Boosters { get; private set; }
    public static int Balance = 1000;
    public static Collider2D GameField;
    public static float StressLevel = 0;

    private bool IsBuffAffixed = false;
    public static HashSet<GameObject> BuffedDeers = new HashSet<GameObject>();

    private void Start()
    {
        StressLevel = 0f;
        GameField = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")
            ?.GetComponent<PolygonCollider2D>();
        /*LoadStatistics();*/
        var kek = GameObject.Find("GameField");
        TrainScript.IsOn = true;


        // PoolManager.FillPool(new PoolInfo
        // {
        //     amount = 15,
        //     container = GameObject.Find("DeerContainer"),
        //     prefab = GameObject.Find("DeerModel"),
        //     type = PoolObjectType.Deer
        // });
        /*var deersGameObjects = GameObject.Find("DeerModel(Clone)");
        foreach (var deerGameObject in deersGameObjects)
        {
            deerGameObject.GetComponentsInChildren<>();
            deerGameObject.SetActive(true);
        }*/
    }

    private void Update()
    {
        GetBuffByStress();
    }

    private void GetBuffByStress()
    {
        if (StressLevel >= 0.5f && !IsBuffAffixed)
        {
            IsBuffAffixed = true;
            var count = Deers.Count >= 3 ? 3 : Deers.Count;
            for (var i = 0; i < count; i++)
            {
                switch (Random.Range(0, 3))
                {
                    case 0:
                        StartCoroutine(Deers.ElementAt(i).GetComponent<Deer>().GetBuff(BuffType.Hunger));
                        break;
                    case 1:
                        StartCoroutine(Deers.ElementAt(i).GetComponent<Deer>().GetBuff(BuffType.Ill));
                        break;
                    case 2:
                        StartCoroutine(Deers.ElementAt(i).GetComponent<Deer>().GetBuff(BuffType.Thirsty));
                        break;
                }

                BuffedDeers.Add(Deers.ElementAt(i));
            }
        }

        if (BuffedDeers.Count == 0)
            IsBuffAffixed = false;
    }
    

    private void LoadStatistics()
    {
        var loadedGame = JsonUtility
            .FromJson<GameModel>(File.ReadAllText($"{Application.streamingAssetsPath}/progress.json"));
        //Deers = loadedGame.Deers;
        Boosters = loadedGame.Boosters;
        //Balance = loadedGame.Balance;
    }

    private void SaveStatistics()
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/progress.json", JsonUtility.ToJson(this));
    }
}