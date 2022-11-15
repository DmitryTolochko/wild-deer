using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    
    public bool isCollected = false;

    public bool isCollectable = false;

    private Rigidbody2D rb;

    private void Start() 
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        StartCoroutine(Grow());
    }

    private void OnMouseDown() 
    {
        isCollected = true;
        print("Собрал");
    }

    private IEnumerator Grow()
    {
        yield return new WaitForSecondsRealtime(3);
        isCollectable = true;
        //меняется скин
    }

    public void ResetItem()
    {
        isCollected = false;
        isCollectable = false;
    }
}
