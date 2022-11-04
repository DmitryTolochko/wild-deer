using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using Unity.VisualScripting;
using UnityEngine;
using Timer = Unity.VisualScripting.Timer;


public enum Age 
{
    Newborn,
    Child,
    Adult,
    Elder,
    Dead
}

public enum Gender
{
    Female,
    Male
}

public class Deer : MonoBehaviour
{
    private const int speed = 2;
    private System.Random random = new System.Random();
    private bool isSearchingNewTarget = false;

    public Rigidbody2D Rb;
    public Transform Target;   
    public Collider2D GameField; 
    public bool IsIll = false;
    public Age CurrentAge;
    public Gender DeerGender;

    public GameObject CurrentTarget;

    void Start()
    {
        transform.position = new Vector3(random.Next(-1, 2), random.Next(-1, 2), 0);
        StartCoroutine(GetOlder());
        GetTargetPosition(0);
        Rb = GetComponent<Rigidbody2D>();
        if (random.Next(0, 1) == 0)
            DeerGender = Gender.Female;
        else
            DeerGender = Gender.Male;
    }

    void Update()
    {
        Move();
        if (IsIll)
        {
            //change sprite
            StartCoroutine(Infection());
        }
        if (CurrentAge == Age.Dead)
            gameObject.tag = "Dead";
    }

    private IEnumerator GetOlder()
    {
        CurrentAge = Age.Newborn;
        print("Родился");
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Child;
        print("Повзрослел");
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Adult;
        print("Повзрослел");
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Elder;
        print("Повзрослел");
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Dead;
        print("Умер");
    }

    private IEnumerator Infection()
    {
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Dead;
        print("Умер от заражения");
        StopCoroutine(GetOlder());
        StopCoroutine(Infection());
        IsIll = false;
    }

    private void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.localPosition, Target.localPosition);
        if (distanceToTarget < 0.2f && !isSearchingNewTarget)
        {
            StartCoroutine(GetTargetPosition(1));
        }
        transform.position = Vector3.MoveTowards(transform.position, Target.localPosition, speed * Time.deltaTime);
    }

    private IEnumerator GetTargetPosition(int time)
    {
        isSearchingNewTarget = true;
        yield return new WaitForSecondsRealtime(UnityEngine.Random.value * time);

        var point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
        while (!GameField.bounds.Contains(point))
        {
            point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
        }
        Target.localPosition = point;
        isSearchingNewTarget = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Deer") || other.CompareTag("Dead"))
        {
            StartCoroutine(GetTargetPosition(0));
        }
    }

    private void OnMouseDown()
    {
        print("Ура");
    }
}