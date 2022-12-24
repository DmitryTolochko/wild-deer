using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class DeerSpawner : MonoBehaviour
{
    private static bool isMakingNewDeer = false;
    private static bool isStoped = false;
    private int ChildrenCountForGeneration = 2;
    private bool IsSideDeerNeeded;
    private int GenerationNumber = 0;

    private HashSet<GameObject> parentDeers = new HashSet<GameObject>();
    private HashSet<GameObject> childDeers = new HashSet<GameObject>();

    public static int DeerCount = 0;
    public static int FemaleCount = 0;
    public static int MaleCount = 0;

    private static Collider2D gameField;

    public GameObject SideDeerMessagePrefab;
    private static GameObject SideDeerMessage;

    public static event Action DeerSpawned;

    public static event Action<Deer> SomeDeerDead;

    public static void GenerateNew()
    {
        isMakingNewDeer = true;
    }

    private void Start()
    {
        gameField = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")
            ?.GetComponent<PolygonCollider2D>();


        // StartCoroutine(CreateDeer(PoolObjectType.Deer));
        // StartCoroutine(CreateDeer(PoolObjectType.Deer));
        // childDeers.ElementAt(0).GetComponent<Deer>().DeerGender = Gender.Female;
        // childDeers.ElementAt(1).GetComponent<Deer>().DeerGender = Gender.Male;
    }

    private void Update()
    {
        if (!TrainScript.IsOn && GameModel.StressLevel <= 0.3f && GameModel.Deers.Count < 14)
        {
            if (ChildrenCountForGeneration == 0
                && childDeers.Count == 0
                && IsSideDeerNeeded == false)
            {
                GenerationNumber += 1;
                print(GenerationNumber);
                ChildrenCountForGeneration = Random.Range(1, 3);
                IsSideDeerNeeded = ChildrenCountForGeneration == 1;
            }

            if (CheckIfTwoParents() && !isStoped)
            {
                if (ChildrenCountForGeneration > 0)
                {
                    isStoped = true;
                    BringTogetherParents();
                }
                else if (ChildrenCountForGeneration == 0 && IsSideDeerNeeded && SideDeerMessage == null)
                {
                    ChildrenCountForGeneration += 1;
                    isStoped = true;
                    ShowSideDeerMessage();
                    StartCoroutine(CreateDeer(PoolObjectType.Deer));
                    IsSideDeerNeeded = false;
                }
            }
            else if (parentDeers.Count < 2
                     && childDeers.Count < 2
                     && SideDeerMessage == null
                     && Random.Range(1, 200) == 1)
            {
                ChildrenCountForGeneration += 1;
                isStoped = true;
                ShowSideDeerMessage();
                StartCoroutine(CreateDeer(PoolObjectType.Deer));
                IsSideDeerNeeded = false;
            }
        }

        if (isMakingNewDeer)
        {
            isMakingNewDeer = false;
            StartCoroutine(CreateDeer(PoolObjectType.Deer));
        }
    }

    private bool CheckIfTwoParents()
    {
        return parentDeers.Count == 2;
    }

    private void BringTogetherParents()
    {
        parentDeers.ElementAt(1).GetComponent<Deer>().FreezePosition();
        parentDeers.ElementAt(0).GetComponent<Deer>().TargetPos =
            parentDeers.ElementAt(1).GetComponent<Deer>().transform.position;

        parentDeers.ElementAt(1).GetComponent<Deer>().IsPairing = true;
        parentDeers.ElementAt(0).GetComponent<Deer>().IsPairing = true;
    }

    private IEnumerator CreateDeer(PoolObjectType type)
    {
        ChildrenCountForGeneration -= 1;
        isStoped = false;

        var deer = PoolManager.Instance.GetPoolObject(type);
        var deerScript = deer.GetComponent<Deer>();

        StartCoroutine(deerScript.GetOlder());
        SetGender(deer, deerScript);
        PlaceDeer(deer, deerScript);
        deerScript.IsSpawned = false;
        childDeers.Add(deer);
        GameModel.Deers.Add(deer);
        deerScript.IsWaiting = false;
        deerScript.ResetLifeBar();
        deer.transform.Find("Shadow").GetComponent<ShadowScript>().IsSpawning = true;
        deer.SetActive(true);
        DeerCount += 1;
        DeerSpawned?.Invoke();
        FemaleCount += deerScript.DeerGender == Gender.Female ? 1 : 0;
        MaleCount += deerScript.DeerGender == Gender.Male ? 1 : 0;
        while (true)
        {
            if (deerScript.CurrentAge == Age.CanMakeChild)
            {
                parentDeers.Add(deer);
                childDeers.Remove(deer);
                break;
            }

            if (deerScript.CurrentAge == Age.Dead)
            {
                childDeers.Remove(deer);
                break;
            }

            yield return new WaitForSecondsRealtime(0);
        }

        while (true)
        {
            if (deerScript.CurrentAge == Age.Adult
                || deerScript.CurrentAge == Age.Dead)
            {
                parentDeers.Remove(deer);
                break;
            }

            yield return new WaitForSecondsRealtime(0);
        }

        while (deerScript.CurrentAge != Age.Dead)
            yield return new WaitForSecondsRealtime(0);

        DeerCount -= 1;
        FemaleCount -= deerScript.DeerGender == Gender.Female ? 1 : 0;
        MaleCount -= deerScript.DeerGender == Gender.Male ? 1 : 0;
        SomeDeerDead?.Invoke(deerScript);
        //deer.gameObject.GetComponent<Deer>().Initialize();  
        PoolManager.Instance.CoolObject(deer, type);
        GameModel.Deers.Remove(deer);
    }

    private void SetGender(GameObject deer, Deer deerScript)
    {
        if (childDeers.Count == 0)
            deerScript.DeerGender =
                UnityEngine.Random.Range(0, 1) > 0.5f ? Gender.Female : Gender.Male;
        else
            deerScript.DeerGender =
                childDeers.Last().GetComponent<Deer>().DeerGender == Gender.Female ? Gender.Male : Gender.Female;

        //deer.GetComponent<DeerAnimator>().ChangeSprite(deerScript.CurrentAge, deerScript.DeerGender);
    }

    private void PlaceDeer(GameObject deer, Deer deerScript)
    {
        deerScript.Speed = 3;
        deerScript.TargetPos = GenerateNewPosition();
        deer.transform.position = new Vector2(
            deerScript.TargetPos.x,
            deerScript.TargetPos.y + 11);
        // print(deer.transform.position);
    }

    public static Vector2 GenerateNewPosition()
    {
        var point = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f));
        while (Physics2D.OverlapCircle(point, 0f) != gameField)
            point = new Vector2(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f));
        return point;
    }

    private void ShowSideDeerMessage()
    {
        SideDeerMessage = Instantiate(SideDeerMessagePrefab);
        Time.timeScale = 0;
    }

    public static void HideSideDeerMessage()
    {
        Destroy(SideDeerMessage);
        Time.timeScale = 1;
    }
}