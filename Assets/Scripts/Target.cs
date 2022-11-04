using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public bool IsSearchingNewTarget = false;
    private bool isInField = false;

    private bool isReached = false;

    private void Start() 
    {
        StartCoroutine(GetTargetPosition(0));
    }

    private void Update() 
    {
        if (isReached)
        {
            StartCoroutine(GetTargetPosition(1));
            isReached = false;
        }
    }

    private IEnumerator GetTargetPosition(int time)
    {
        IsSearchingNewTarget = true;
        yield return new WaitForSecondsRealtime(UnityEngine.Random.value * time);

        var point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
        while (!isInField)
        {
            print("y");
            point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
            transform.position = point;
        }
        IsSearchingNewTarget = false;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        isInField = other.CompareTag("GameField");
        isReached = other.CompareTag("Deer");
    }

    private void OnTriggerExit(Collider other)
    {
        isInField = other.CompareTag("GameField");
    }
}
