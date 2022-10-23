using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using Timer = Unity.VisualScripting.Timer;


enum Age 
{
    Newborn,
    Child,
    Adult,
    Elder,
    Dead
}

public class Deer : MonoBehaviour
{
    private const int speed = 2;
    private System.Random random = new System.Random();
    public const int a = 7;
    public const int b = 4;
    public Rigidbody2D rb;

    private float x;
    private float y;

    private Age currentAge;

    private DateTime time = new DateTime();

    void Start()
    {
        StartCoroutine(GetOlder());
        x = random.Next(-1, 2);
        y = random.Next(-1, 2);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    private IEnumerator GetOlder()
    {
        currentAge = Age.Newborn;
        print("Родился");
        yield return new WaitForSecondsRealtime(10);
        currentAge = Age.Child;
        print("Повзрослел");
        yield return new WaitForSecondsRealtime(10);
        currentAge = Age.Adult;
        print("Повзрослел");
        yield return new WaitForSecondsRealtime(10);
        currentAge = Age.Elder;
        print("Повзрослел");
        yield return new WaitForSecondsRealtime(10);
        currentAge = Age.Dead;
        print("Умер");
    }

    private bool IsInBounds(Vector3 position)
    {
        return Math.Pow(position.x, 2) / (a * a) + Math.Pow(position.y, 2) / (b * b) <= 1;
    }

    private void Move()
    {
        if (!IsInBounds(transform.position))
        {
            while (!IsInBounds(transform.position += new Vector3(x, y, 0) * speed * Time.deltaTime))
            {
                x = random.Next(-1, 2);
                y = random.Next(-1, 2);
            }
        }
        transform.position += new Vector3(x, y, 0) * speed * Time.deltaTime;
    }
}