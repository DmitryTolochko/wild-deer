using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model.Boosters;
using ServiceInstances;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    public List<Deer> Deers { get; private set; }
    public Dictionary<BoosterType, IBooster> Boosters { get; private set; }
    public int Count => Deers.Count;
    public static int Balance = 1000;
    //public int DeersStressLevel => Deers.Sum(x => x.StressLevel);

    private List<GameObject> deersGameObjects;

    public static Collider2D GameField;


    private void Start()
    {
        GameField = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")
            ?.GetComponent<PolygonCollider2D>();
        /*LoadStatistics();*/
        var kek = GameObject.Find("GameField");
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

    private void LoadStatistics()
    {
        var loadedGame = JsonUtility
            .FromJson<GameModel>(File.ReadAllText($"{Application.streamingAssetsPath}/progress.json"));
        Deers = loadedGame.Deers;
        Boosters = loadedGame.Boosters;
        //Balance = loadedGame.Balance;
    }

    private void SaveStatistics()
    {
        File.WriteAllText($"{Application.streamingAssetsPath}/progress.json", JsonUtility.ToJson(this));
    }


}
