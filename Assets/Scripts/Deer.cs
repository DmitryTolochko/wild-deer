using System.Collections;
using UnityEngine;
using System.Linq;


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
    private float speed = 2;
    private bool isWaiting = false;

    private Rigidbody2D rb;
    public Vector3 Target_pos = new Vector3();   
    private Collider2D gameField; 

    public bool IsIll = false;
    public Age CurrentAge;
    public Gender DeerGender;
    public bool IsPairing = false;

    public void Start()
    {
        gameField = Resources.FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")
            ?.GetComponent<PolygonCollider2D>();

        rb = GetComponent<Rigidbody2D>();
        Target_pos = GenerateNewPosition();
        transform.position = GenerateNewPosition();
        StartCoroutine(GetOlder());

        // if (UnityEngine.Random.Range(0, 1) > 0.5f)
        //     DeerGender = Gender.Female;
        // else
        //     DeerGender = Gender.Male;
    }

    private void Update()
    {
        Move();
        if (IsIll)
        {
            //change sprite
            StartCoroutine(Infection());
        }
    }

    public void Reset() 
    {
        Target_pos = GenerateNewPosition();
        transform.position = GenerateNewPosition();
        StartCoroutine(GetOlder());

        if (UnityEngine.Random.Range(0, 1) > 0.5f)
            DeerGender = Gender.Female;
        else
            DeerGender = Gender.Male;
    }

    private IEnumerator GetOlder()
    {
        CurrentAge = Age.Newborn;
        print("Родился");
        print(DeerGender);
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Child;
        print("Ребёнок");
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Adult;
        print("Взрослый");
        yield return new WaitForSecondsRealtime(30);
        CurrentAge = Age.Elder;
        print("Пожилой");
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
        float distanceToTarget = Vector3.Distance(transform.localPosition, Target_pos);
        if (distanceToTarget < 0.5f && !isWaiting)
        {
            StartCoroutine(WaitAndChangeTargetPose(UnityEngine.Random.Range(0, 3)));
        }
        else if (!isWaiting)
            transform.position = Vector3.MoveTowards(transform.position, Target_pos, speed * Time.deltaTime);
    }

    private IEnumerator WaitAndChangeTargetPose(int time)
    {
        isWaiting = true;
        yield return new WaitForSecondsRealtime(time);
        Target_pos = GenerateNewPosition();
        speed = Random.Range(0.2f, 2f);
        isWaiting = false;
    }

    private Vector3 GenerateNewPosition()
    {
        var point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);
        while (!gameField.bounds.Contains(point))
            point = new Vector3(UnityEngine.Random.value * 8-4, UnityEngine.Random.value * 8-4, 0);

        return point;
    }

    public void FreezePosition()
    {
        isWaiting = !isWaiting;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Deer(Clone)" 
            && !other.gameObject.GetComponent<Deer>().IsPairing
            && !this.IsPairing)
            Target_pos = GenerateNewPosition();
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.name == "Deer(Clone)" 
            && other.gameObject.GetComponent<Deer>().IsPairing
            && this.IsPairing
            && this.isWaiting)
        {
            DeerSpawner.GenerateNew();
            this.isWaiting = false;
            other.gameObject.GetComponent<Deer>().IsPairing = false;
            this.IsPairing = false;
        }
    }

    private void OnMouseDown()
    {
        print("Ура");
    }
}