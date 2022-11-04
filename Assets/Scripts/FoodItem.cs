using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public Collider2D GameField; 
    public Collider2D FoodField; 

    private bool isCollected = false;
    // Start is called before the first frame update
    void Start()
    {
        var point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
        while (GameField.bounds.Contains(point) && !FoodField.bounds.Contains(point))
        {
            point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
        }
        transform.position = point;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCollected)
        {
            print("Собрал");
        }
    }

    private void OnMouseDown() 
    {
        isCollected = true;
    }
}
