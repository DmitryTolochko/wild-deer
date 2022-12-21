using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private BaseThreat myThreat;
    public Animator animator;

    private void Start() 
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        myThreat = GetComponent<BaseThreat>();
    }

    private void Update() 
    {
        if (myThreat.Status != ThreatStatus.Won)
            spriteRenderer.flipX =
                myThreat.TargetDeer.transform.position.x > transform.position.x;
        else
            spriteRenderer.flipX =
                myThreat.TargetDeer.transform.position.x < transform.position.x;

        spriteRenderer.sortingOrder = (int)(transform.position.y * (-10));

        animator.SetFloat("Move", (int)myThreat.Status);
    }
}
