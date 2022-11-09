using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DeerHerd : MonoBehaviour
{
    private IEnumerator GenerateRoutine(PoolObjectType type)
    {
        GameObject ob = PoolManager.Instance.GetPoolObject(type);

        // ob.transform.position = new Vector2(Random.Range(-2f, 2f), Random.Range(-3f, 3f));
        ob.gameObject.SetActive(true);
        
        while (ob.tag != "Dead") 
            yield return new WaitForSecondsRealtime(0);
        PoolManager.Instance.CoolObject(ob, type);
    }

    private void Start() 
    {
        StartCoroutine(GenerateRoutine(PoolObjectType.Deer));
    }
}
