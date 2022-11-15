using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeerSpawner : MonoBehaviour
{
    private static bool isMakingNewDeer = false;
    private int previousStressLevel = 0;

    public int StressLevel = 0;
    public static HashSet<GameObject> deers = new HashSet<GameObject>();
    public IEnumerator GenerateRoutine(PoolObjectType type)
    {
        GameObject deer = PoolManager.Instance.GetPoolObject(type);
        deer.gameObject.SetActive(true);
        deers.Add(deer);
        while (deer.GetComponent<Deer>().CurrentAge != Age.Dead) 
            yield return new WaitForSecondsRealtime(0);
        
        deers.RemoveWhere(x => x.GetComponent<Deer>().CurrentAge == Age.Dead);
        deer.gameObject.GetComponent<Deer>().Start();
        PoolManager.Instance.CoolObject(deer, type);
    }

    private void Start() 
    {
        StartCoroutine(GenerateRoutine(PoolObjectType.Deer));
        StartCoroutine(GenerateRoutine(PoolObjectType.Deer));
        deers.ElementAt(0).GetComponent<Deer>().DeerGender = Gender.Female;
        deers.ElementAt(1).GetComponent<Deer>().DeerGender = Gender.Male;
    }

    private void Update() 
    {
        if (deers.Count > 1 && StressLevel < 20 && Random.Range(0, 1000) == 0)
        {
            for(var i = 0; i < deers.Count - 1; i++)
            {
                var firstDeer = deers.ElementAt(i).GetComponent<Deer>();
                if (firstDeer.CurrentAge != Age.Adult || firstDeer.IsIll || firstDeer.IsPairing)
                    continue;
                for (var j = i + 1; j < deers.Count; j++)
                {
                    var secondDeer = deers.ElementAt(j).GetComponent<Deer>();
                    if (secondDeer.CurrentAge == Age.Adult 
                        && !secondDeer.IsIll
                        && !secondDeer.IsPairing
                        && secondDeer.DeerGender != firstDeer.DeerGender)
                        {
                            secondDeer.FreezePosition();
                            firstDeer.Target_pos = secondDeer.transform.position;
                            print(firstDeer.Target_pos);
                            print(secondDeer.transform.position);
                            firstDeer.IsPairing = true;
                            secondDeer.IsPairing = true;
                            break;
                        }
                }
            }
        }

        if (previousStressLevel != StressLevel)
        {
            previousStressLevel = StressLevel;
        }
        if (isMakingNewDeer)
        {
            isMakingNewDeer = false;
            StartCoroutine(GenerateRoutine(PoolObjectType.Deer));
        }
    }

    public static void GenerateNew()
    {
        isMakingNewDeer = true;
    }
}
