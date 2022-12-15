using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite YoungDeer;
    public Sprite MaleDeer;
    public Sprite FemaleDeer;

    private Deer deer;

    private void Start() 
    {
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        deer = GetComponent<Deer>();
    }
    private void Update() 
    {
        spriteRenderer.flipX = deer.TargetPos.x > transform.position.x;

        spriteRenderer.sortingOrder = (int)(transform.position.y * (-10));
    }

    public void ChangeSprite(Age age, Gender gender)
    {
        if (spriteRenderer is null)
            spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        
        switch (age)
        {
            case Age.Child:
                print(YoungDeer);
                spriteRenderer.sprite = YoungDeer;
                break;
            case Age.Adult:
                spriteRenderer.sprite = 
                    gender == Gender.Male 
                        ? MaleDeer
                        : FemaleDeer;
                break;
        }
    }
}