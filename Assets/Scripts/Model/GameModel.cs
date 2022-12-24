using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Model.Boosters;
using ServiceInstances;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Timer = System.Timers.Timer;

public class GameModel : MonoBehaviour
{
    public static HashSet<GameObject> Deers = new();
    public static GameObject CurrentThreat;
    public static List<TaskInstance> ActualTasks = new();
    public static HashSet<GameObject> FoodSpawned = new();
    public Dictionary<BoosterType, IBooster> Boosters { get; private set; }
    public static int Balance = 1000;
    public static Collider2D GameField;

    public static System.TimeSpan bestTime;

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

    public static async void ChangeStressAsync(float value)
    {
        if (value < 0 && StressLevel + value < 0) 
            return;
        if (value > 0 && StressLevel + value > 1)
            return;
        ChangeStress(value);
        await Task.Yield();
    }

    private static void ChangeStress(float value)
    {
        StressLevel += value;
    }

    private void Update()
    {
        GetBuffByStress();
        if (Time.timeScale == 1)
            ChangeStressAsync(0.00001f);
    }

    private void GetBuffByStress()
    {
        if (StressLevel >= 0.5f && !IsBuffAffixed)
        {
            IsBuffAffixed = true;
            var count = Deers.Count >= 3 ? 3 : Deers.Count;
            StartCoroutine(GetBuff(Random.Range(3, 10), count));
        }

        if (BuffedDeers.Count == 0)
            IsBuffAffixed = false;
    }

    private IEnumerator GetBuff(float time, int count)
    {
        for (var i = 0; i < count; i++)
            {
                var BuffType = default(BuffType);
                switch (Random.Range(0, 3))
                {
                    case 0:
                        BuffType = BuffType.Hunger;
                        break;
                    case 1:
                        BuffType = BuffType.Ill;
                        break;
                    case 2:
                        BuffType = BuffType.Thirsty;
                        break;
                }
                // Deers.ElementAt(i).GetComponent<Deer>().BuffType == BuffType.No
                if (Deers.Count != 0 && !BuffedDeers.Contains(Deers.ElementAt(i))
                && Deers.ElementAt(i).GetComponent<Deer>().BuffType == BuffType.No)
                {
                    StartCoroutine(Deers.ElementAt(i).GetComponent<Deer>().GetBuff(BuffType));
                    BuffedDeers.Add(Deers.ElementAt(i));
                }
                yield return new WaitForSeconds(time);
            }
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

    public static BoosterType GetBoosterTypeByBuffType(BuffType buffType)
    {
        return buffType switch
        {
            BuffType.Hunger => BoosterType.Food,
            BuffType.Ill => BoosterType.Medicines,
            BuffType.Thirsty => BoosterType.Water
        };
    }
}