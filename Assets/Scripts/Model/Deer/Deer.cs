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
    public bool IsHungry = false;
    public bool IsThirsty = false;
    public bool IsPairing = false;
    public bool IsAVictim = false;

    private bool flag = false;

    public Age CurrentAge;
    public Gender DeerGender;

    private Slider TimerBar;
    public Slider LifeBar;
    private float valuePerSecond = 0;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Find("DeerUI").GetComponent<Canvas>().worldCamera = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "Main Camera")
            ?.GetComponent<Camera>();
        
        transform.Find("DeerUI").gameObject.SetActive(true);
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
        TimerBar = transform.Find("DeerUI").transform.Find("Slider").GetComponent<Slider>();
        LifeBar = transform.Find("DeerUI").transform.Find("LifeBar").GetComponent<Slider>();
        LifeBar.value = 1;
    }

    public void ResetLifeBar()
    {
        LifeBar = transform.Find("DeerUI").transform.Find("LifeBar").GetComponent<Slider>();
        LifeBar.value = 1;
    }

    private void Update()
    {
        Move();
        
        if (IsIll)
            StartCoroutine(Infection());
        else if (!IsIll && flag)
        {
            StopCoroutine(Infection());
            flag = false;
        }
        if (IsHungry)
            StartCoroutine(GetHungry());
        else if (!IsHungry && flag)
        {
            StopCoroutine(GetHungry());
            flag = false;
        }
        if (IsThirsty)
            StartCoroutine(GetThirsty());
        else if (!IsThirsty && flag)
        {
            StopCoroutine(GetThirsty());
            flag = false;
        }

        if (valuePerSecond != 0)
            ChangeTimerBar();
        
        if (LifeBar.value >= 0.0017f)
            LifeBar.value -= 0.0017f * Time.deltaTime;
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
        flag = true;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(true);
        valuePerSecond = 0.1f;
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Dead;
        print("Умер от заражения");
        StopCoroutine(GetOlder());
        
        IsIll = false;
        valuePerSecond = 0;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
        TimerBar.value = 1;
        flag = false;
        StopCoroutine(Infection());
    }

    private IEnumerator GetHungry()
    {
        flag = true;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(true);
        valuePerSecond = 0.1f;
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Dead;
        print("Умер от голода");
        StopCoroutine(GetOlder());
        
        IsHungry = false;
        valuePerSecond = 0;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
        TimerBar.value = 1;
        flag = false;
        StopCoroutine(GetHungry());
    }

    private IEnumerator GetThirsty()
    {
        flag = true;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(true);
        valuePerSecond = 0.1f;
        yield return new WaitForSecondsRealtime(10);
        CurrentAge = Age.Dead;
        print("Умер от жажды");
        StopCoroutine(GetOlder());
        
        IsThirsty = false;
        valuePerSecond = 0;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
        TimerBar.value = 1;
        flag = false;
        StopCoroutine(GetThirsty());
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

    // public void Heal()
    // {
    //     StopCoroutine(Infection());
    //     IsIll = false;
    //     valuePerSecond = 0;
    //     transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
    //     TimerBar.value = 1;
    // }
}