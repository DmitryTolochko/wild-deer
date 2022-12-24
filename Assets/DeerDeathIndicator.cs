using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerDeathIndicator : MonoBehaviour
{
    public GameObject deerDeathIndicator;
    private GameObject currentDeadDeer;
    private Queue<GameObject> deerDeathIndicators;

    void Start()
    {
        deerDeathIndicators = new Queue<GameObject>();
        DeerSpawner.SomeDeerDead += deer =>
        {
            currentDeadDeer = deer.gameObject;
            deerDeathIndicators.Enqueue(Instantiate(
                deerDeathIndicator,
                currentDeadDeer.transform.position + new Vector3(0, 0.15f),
                Quaternion.identity));
            StartCoroutine(WaitBeforeDisappear(1f));
        };
    }

    private IEnumerator WaitBeforeDisappear(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(deerDeathIndicators.Dequeue());
    }
}