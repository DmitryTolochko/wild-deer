using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class Deer : MonoBehaviour
{
    public float speed;
    public bool IsWaiting = false;

    private Rigidbody2D rb;
    public Vector3 TargetPos = new Vector3();   

    public bool IsIll = false;
    public Age CurrentAge;
    public Gender DeerGender;
    public bool IsPairing = false;
    public bool IsAVictim = false;

    public bool WantToEat = false;
    public bool WantToDrink = false;
    private Slider TimerBar;
    private float valuePerSecond = 0;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Find("Timer").GetComponent<Canvas>().worldCamera = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "Main Camera")
            ?.GetComponent<Camera>();
        
        transform.Find("Timer").gameObject.SetActive(false);
        TimerBar = transform.Find("Timer").transform.Find("Slider").GetComponent<Slider>();
    }

    private void Update()
    {
        Move();
        if (IsIll)
        {
            //change sprite
            StartCoroutine(Infection());
        }
        if (valuePerSecond != 0)
            ChangeTimerBar();
    }

    public IEnumerator GetOlder()
    {
        CurrentAge = Age.Child;
        print("Ребёнок");
        yield return new WaitForSecondsRealtime(60);
        CurrentAge = Age.CanMakeChild;
        print("Должен сделать детей");
        yield return new WaitForSecondsRealtime(25);
        CurrentAge = Age.Adult;
        print("Взрослый");
        yield return new WaitForSecondsRealtime(515);
        CurrentAge = Age.Dead;
        print("Умер");
    }

    private IEnumerator Infection()
    {
        transform.Find("Timer").gameObject.SetActive(true);
        valuePerSecond = 0.1f;
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Dead;
        print("Умер от заражения");
        StopCoroutine(GetOlder());
        StopCoroutine(Infection());
        
        IsIll = false;
        valuePerSecond = 0;
        transform.Find("Timer").gameObject.SetActive(false);
        TimerBar.value = 1;
    }

    private void ChangeTimerBar()
    {
        TimerBar.value -= valuePerSecond * Time.deltaTime;
    }

    private void Move()
    {
        float distanceToTarget = Vector3.Distance(transform.localPosition, TargetPos);
        if (distanceToTarget < 0.5f && !IsWaiting)
        {
            StartCoroutine(WaitAndChangeTargetPose(UnityEngine.Random.Range(0, 3)));
        }
        else if (!IsWaiting)
            transform.position = Vector3.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
    }

    private IEnumerator WaitAndChangeTargetPose(int time)
    {
        IsWaiting = true;
        yield return new WaitForSecondsRealtime(time);
        TargetPos = DeerSpawner.GenerateNewPosition();
        speed = Random.Range(0.2f, 2f);
        IsWaiting = false;
    }

    public void FreezePosition()
    {
        IsWaiting = !IsWaiting;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Deer(Clone)" 
            && !other.gameObject.GetComponent<Deer>().IsPairing
            && !this.IsPairing)
            TargetPos = DeerSpawner.GenerateNewPosition();
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.name == "Deer(Clone)" 
            && other.gameObject.GetComponent<Deer>().IsPairing
            && this.IsPairing
            && this.IsWaiting)
        {
            DeerSpawner.GenerateNew();
            this.IsWaiting = false;
            other.gameObject.GetComponent<Deer>().IsPairing = false;
            this.IsPairing = false;
        }
    }

    private void OnMouseDown()
    {
        print("Ура");
    }
}