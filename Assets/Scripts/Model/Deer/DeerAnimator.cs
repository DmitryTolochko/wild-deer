using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimator : MonoBehaviour
{
    private void Update() 
    {
        transform.Find("Sprite").GetComponent<SpriteRenderer>().flipX = 
            GetComponent<Deer>().TargetPos.x > transform.position.x;
    }

    public void ChangeSprite(Age age, Gender gender)
    {
        switch (age)
        {
            case Age.Child:
                transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = 
                Resources.Load<Sprite>("YoungDeer");
                break;
            case Age.Adult:
                transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = 
                gender == Gender.Male 
                ? Resources.Load<Sprite>("MaleDeer")
                : Resources.Load<Sprite>("FemaleDeer");
                break;
        }
    }
}
