using System;
using System.Collections;
using UnityEngine;

public class ProtectiveCapBehavior : MonoBehaviour
{
    private const int ActingDelay = 45;

    void Start()
    {
        StartCoroutine(DisappearWithDelay(ActingDelay));
    }

    private IEnumerator DisappearWithDelay(int time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent<BaseThreat>(out var threat))
        {
            threat.Status = ThreatStatus.Defeated;
        }
    }
}