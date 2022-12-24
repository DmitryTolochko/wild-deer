using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerAnimator : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;


    public GameObject YoungDeer;
    public GameObject MaleDeer;
    public GameObject FemaleDeer;
    private Deer deer;


    // volodya
    public GameObject Hearts;
    private Animator heartsAnimator;
    private readonly int deerSatisfied = Animator.StringToHash("DeerSatisfied");

    private void Start()
    {
        // spriteRenderer = YoungDeer.GetComponent<SpriteRenderer>();
        deer = GetComponent<Deer>();
        heartsAnimator = Hearts.GetComponent<Animator>();
        deer.DeerDrank += () => StartCoroutine(WaitForAnimEnding(1.03f));
        deer.DeerFed += () => StartCoroutine(WaitForAnimEnding(1.03f));
        deer.DeerHealed += () => StartCoroutine(WaitForAnimEnding(1.03f));
    }

    private void Update()
    {
        spriteRenderer.flipX = deer.TargetPos.x > transform.position.x;

        spriteRenderer.sortingOrder = (int) (transform.position.y * (-10));

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

    private IEnumerator WaitForAnimEnding(float animDuration)
    {
        heartsAnimator.SetInteger(deerSatisfied, 1);
        yield return new WaitForSeconds(animDuration);
        heartsAnimator.SetInteger(deerSatisfied, 0);
    }
}