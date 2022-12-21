using UnityEngine;

public class DeerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    public GameObject YoungDeer;
    public GameObject MaleDeer;
    public GameObject FemaleDeer;

    private Deer deer;

    private void Start() 
    {
        // spriteRenderer = YoungDeer.GetComponent<SpriteRenderer>();
        deer = GetComponent<Deer>();
    }
    private void Update() 
    {
        spriteRenderer.flipX = deer.TargetPos.x > transform.position.x;

        spriteRenderer.sortingOrder = (int)(transform.position.y * (-10));

        if (deer.Speed == 0 || !deer.IsSpawned)
            animator.SetFloat("Move", 0);
        else
            animator.SetFloat("Move", 1);
    }

    public void ChangeSprite(Age age, Gender gender)
    {
        if (spriteRenderer is null)
            spriteRenderer = YoungDeer.GetComponent<SpriteRenderer>();
        
        switch (age)
        {
            case Age.Child:
                YoungDeer.SetActive(true);
                MaleDeer.SetActive(false);
                FemaleDeer.SetActive(false);
                animator = YoungDeer.GetComponent<Animator>();
                break;
            case Age.Adult:
                YoungDeer.SetActive(false);
                
                animator = YoungDeer.GetComponent<Animator>();
                if (gender == Gender.Male)
                {
                    MaleDeer.SetActive(true);
                    animator = MaleDeer.GetComponent<Animator>();
                    spriteRenderer = MaleDeer.GetComponent<SpriteRenderer>();
                }
                else
                {
                    FemaleDeer.SetActive(true);
                    animator = FemaleDeer.GetComponent<Animator>();
                    spriteRenderer = FemaleDeer.GetComponent<SpriteRenderer>();
                }
                break;
        }
    }
}