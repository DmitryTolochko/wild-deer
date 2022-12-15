using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start() 
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        if (GetComponent<BaseThreat>().Status != ThreatStatus.Won)
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX =
                GetComponent<BaseThreat>().TargetDeer.transform.position.x > transform.position.x;
        else
            transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX =
                GetComponent<BaseThreat>().TargetDeer.transform.position.x < transform.position.x;

        spriteRenderer.sortingOrder = (int)(transform.position.y * (-10));
    }
}
