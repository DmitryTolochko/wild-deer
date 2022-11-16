using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FoodItem : MonoBehaviour
{
    public bool isCollected = false;
    public bool isCollectable = false;
    public event Action<FoodItem> ItemCollected;

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
        ItemCollected.Invoke(this);
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
