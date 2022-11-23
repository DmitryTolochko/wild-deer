using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public static void GenerateNew()
    {
        isMakingNewDeer = true;
    }

    private void Start() 
    {
        gameField = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")
            ?.GetComponent<PolygonCollider2D>();

        
        StartCoroutine(CreateDeer(PoolObjectType.Deer));
        StartCoroutine(CreateDeer(PoolObjectType.Deer));
        childDeers.ElementAt(0).GetComponent<Deer>().DeerGender = Gender.Female;
        childDeers.ElementAt(1).GetComponent<Deer>().DeerGender = Gender.Male;
    }

    private void Update() 
    {
        if (UIScript.StressLevel <= 0.25f)
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
                    print("1 создаем оленя");
                    BringTogetherParents();
                }
                else if (ChildrenCountForGeneration == 0 && IsSideDeerNeeded && SideDeerMessage == null)
                {
                    ChildrenCountForGeneration += 1;
                    isStoped = true;
                    print("2 вызов стороннего оленя");
                    ShowSideDeerMessage();
                    StartCoroutine(CreateDeer(PoolObjectType.Deer));
                    IsSideDeerNeeded = false;
                }
            }
            else if (parentDeers.Count < 2 
                && childDeers.Count < 2 
                && SideDeerMessage == null)
            {
                ChildrenCountForGeneration += 1;
                print("убит");
                isStoped = true;
                print("вызов стороннего оленя при убийстве");
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

        GameObject deer = PoolManager.Instance.GetPoolObject(type);
        StartCoroutine(deer.GetComponent<Deer>().GetOlder());
        SetGender(deer);
        PlaceDeer(deer);
        childDeers.Add(deer);
        deer.gameObject.GetComponent<Deer>().IsWaiting = false;
        deer.gameObject.SetActive(true);
        DeerCount += 1;
        FemaleCount += deer.GetComponent<Deer>().DeerGender == Gender.Female ? 1 : 0;
        MaleCount += deer.GetComponent<Deer>().DeerGender == Gender.Male ? 1 : 0;
        while (true) 
        {
            if (deer.GetComponent<Deer>().CurrentAge == Age.CanMakeChild)
            {
                parentDeers.Add(deer);
                childDeers.Remove(deer);
                break;
            }
            if (deer.GetComponent<Deer>().CurrentAge == Age.Dead)
            {
                childDeers.Remove(deer);
                break;
            }
            yield return new WaitForSecondsRealtime(0);
        }
        while (true) 
        {
            if (deer.GetComponent<Deer>().CurrentAge == Age.Adult
            || deer.GetComponent<Deer>().CurrentAge == Age.Dead)
            {
                parentDeers.Remove(deer);
                break;
            }
            yield return new WaitForSecondsRealtime(0);
        }
        while (deer.GetComponent<Deer>().CurrentAge != Age.Dead) 
            yield return new WaitForSecondsRealtime(0);

        DeerCount -= 1;    
        FemaleCount -= deer.GetComponent<Deer>().DeerGender == Gender.Female ? 1 : 0;
        MaleCount -= deer.GetComponent<Deer>().DeerGender == Gender.Male ? 1 : 0;
        //deer.gameObject.GetComponent<Deer>().Initialize();  
        PoolManager.Instance.CoolObject(deer, type);
    }

    private void SetGender(GameObject deer)
    {
        if (childDeers.Count == 0)
            deer.gameObject.GetComponent<Deer>().DeerGender = 
                UnityEngine.Random.Range(0, 1) > 0.5f ? Gender.Female : Gender.Male;
        else
            deer.gameObject.GetComponent<Deer>().DeerGender = 
                childDeers.Last().GetComponent<Deer>().DeerGender == Gender.Female ? Gender.Male : Gender.Female;
    }

    private void PlaceDeer(GameObject deer)
    {
        deer.GetComponent<Deer>().speed = 3;
        deer.GetComponent<Deer>().TargetPos = GenerateNewPosition();
        deer.transform.position = new Vector3(
            deer.GetComponent<Deer>().TargetPos.x, 
            deer.GetComponent<Deer>().TargetPos.y + 11, 
            0);
        // print(deer.transform.position);
    }

    public static Vector3 GenerateNewPosition()
    {
        var point = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0);
        while (Physics2D.OverlapCircle(point, 0f) != gameField)
            point = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0);

        return point;
    }

    private void ShowSideDeerMessage()
    {
        print(SideDeerMessagePrefab);
        SideDeerMessage = Instantiate(SideDeerMessagePrefab);
        Time.timeScale = 0;
    }

    public static void HideSideDeerMessage()
    {
        Destroy(SideDeerMessage);
        Time.timeScale = 1;
    }
}