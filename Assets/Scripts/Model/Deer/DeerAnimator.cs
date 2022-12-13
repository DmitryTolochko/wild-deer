using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimator : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;

    private void Start() 
    {
        SpriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }
    private void Update() 
    {
        SpriteRenderer.flipX = 
            GetComponent<Deer>().TargetPos.x > transform.position.x;

        SpriteRenderer.sortingOrder = (int)(transform.position.y * (-10));
    }

    public void ChangeSprite(Age age, Gender gender)
    {
        switch (age)
        {
            case Age.Child:
                SpriteRenderer.sprite = 
                Resources.Load<Sprite>("YoungDeer");
                break;
            case Age.Adult:
                SpriteRenderer.sprite = 
                gender == Gender.Male 
                ? Resources.Load<Sprite>("MaleDeer")
                : Resources.Load<Sprite>("FemaleDeer");
                break;
        }
    }
}
