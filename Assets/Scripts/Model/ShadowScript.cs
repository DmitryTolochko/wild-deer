using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    public bool IsSpawning = true;
    public GameObject TargetPoint;
    public GameObject MyObject;

    public float OffsetY;
    public float OffsetX;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (IsSpawning)
        {
            var distance = 0.0f;
            if (MyObject.TryGetComponent<Deer>(out var deer))
                distance = deer.DistanceToTarget;
            else if (MyObject.TryGetComponent<BaseThreat>(out var threat))
                distance = Vector2.Distance(
                MyObject.transform.position,
                TargetPoint.transform.position
            );
            
            var scale = (1 - distance / 11);
            transform.position = new Vector2(
                TargetPoint.transform.position.x - OffsetX,
                TargetPoint.transform.position.y - OffsetY
            );

            spriteRenderer.color = new Color(
                spriteRenderer.color.r,
                spriteRenderer.color.g,
                spriteRenderer.color.b,
                scale
            );

            transform.localScale = new Vector3(
                scale,
                scale,
                scale
            );

            if (distance <= 0.65f)
                IsSpawning = false;
        }
    }
}