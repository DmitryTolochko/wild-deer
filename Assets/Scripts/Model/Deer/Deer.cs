using System;
using System.Collections;
using UnityEngine;
using System.Linq;
using System.Timers;
using ServiceInstances;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BuffType
{
    No,
    Ill,
    Hunger,
    Thirsty,
}

public class Deer : MonoBehaviour
{
    public float Speed;
    public bool IsWaiting = false;

    private Rigidbody2D rb;
    public Vector2 TargetPos = new Vector2();
    public BuffType BuffType;

    public bool IsPairing = false;
    public bool IsAVictim = false;

    public Age CurrentAge;
    public Gender DeerGender;

    private Slider timerBar;
    private Slider lifeBar;
    private float valuePerSecond = 0;
    private Image buffImage;
    public static event Action DeerFed;
    public static event Action DeerHealed;
    public static event Action DeerDrank;

    private GameObject TargetPoint;

    public bool IsSpawned;
    
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.Find("DeerUI").GetComponent<Canvas>().worldCamera = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "Main Camera")
            ?.GetComponent<Camera>();

        transform.Find("DeerUI").gameObject.SetActive(true);
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
        buffImage = transform.Find("DeerUI").transform.Find("Slider").transform.Find("BuffSprite")
            .GetComponent<Image>();
        timerBar = transform.Find("DeerUI").transform.Find("Slider").GetComponent<Slider>();
        lifeBar = transform.Find("DeerUI").transform.Find("LifeBar").GetComponent<Slider>();
        lifeBar.value = 1;

        TargetPoint = transform.Find("TargetPoint").gameObject;
    }

    public void ResetLifeBar()
    {
        lifeBar = transform.Find("DeerUI").transform.Find("LifeBar").GetComponent<Slider>();
        lifeBar.value = 1;
    }

    private void Update()
    {
        Move();

        if (valuePerSecond != 0)
            ChangeTimerBar();

        if (lifeBar.value >= 0.0017f)
            lifeBar.value -= 0.0017f * Time.deltaTime;
    }

    public IEnumerator GetOlder()
    {
        CurrentAge = Age.Child;
        GetComponent<DeerAnimator>().ChangeSprite(CurrentAge, DeerGender);
        print("Ребёнок");
        yield return new WaitForSeconds(60);
        GetComponent<DeerAnimator>().ChangeSprite(Age.Adult, DeerGender);
        CurrentAge = Age.CanMakeChild;
        print("Должен сделать детей");
        yield return new WaitForSeconds(25);
        CurrentAge = Age.Adult;
        print("Взрослый");
        yield return new WaitForSeconds(515);
        CurrentAge = Age.Dead;
        print("Умер");
    }

    public IEnumerator GetBuff(BuffType newBuff)
    {
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(true);
        BuffType = newBuff;

        switch (newBuff)
        {
            case BuffType.Hunger:
                valuePerSecond = 0.1f;
                buffImage.sprite = Resources.Load<Sprite>("HungerBuff");
                yield return new WaitForSeconds(10);
                break;
            case BuffType.Ill:
                valuePerSecond = 0.06f;
                buffImage.sprite = Resources.Load<Sprite>("InfectionBuff");
                yield return new WaitForSeconds(15);
                break;
            case BuffType.Thirsty:
                valuePerSecond = 0.05f;
                buffImage.sprite = Resources.Load<Sprite>("WaterBuff");
                yield return new WaitForSeconds(20);
                break;
        }

        if (BuffType != BuffType.No)
        {
            CurrentAge = Age.Dead;
        }

        BuffType = BuffType.No;
        ResetTimerBar();
    }

    private IEnumerator GetBuff()
    {
        switch (Random.Range(0, 2))
        {
            case 0:
                StartCoroutine(GetBuff(BuffType.Hunger));
                break;
            case 1:
                StartCoroutine(GetBuff(BuffType.Thirsty));
                break;
        }

        GameModel.BuffedDeers.Add(this.gameObject);

        var start = Time.time;
        var timeInterval = Random.Range(10, 15);
        while (start + timeInterval != Time.time)
            yield return false;

        print("yes");
        StartCoroutine(GetBuff());
    }

    // public void StopBuff(BuffType newBuff)
    // {
    //     StopCoroutine(GetBuff(newBuff));
    //     BuffType = BuffType.No;
    //     GameModel.StressLevel -= GameModel.StressLevel < 0.1f ? GameModel.StressLevel : 0.1f;
    //     ResetTimerBar();
    // }

    public void StopBuff(BuffType newBuff)
    {
        StopCoroutine(GetBuff(newBuff));
        BuffType = BuffType.No;

        switch (newBuff)
        {
            case BuffType.Hunger:
                DeerFed?.Invoke();
                break;
            case BuffType.Ill:
                DeerHealed?.Invoke();
                break;
            case BuffType.Thirsty:
                DeerDrank?.Invoke();
                break;
        }

        GameModel.StressLevel -= GameModel.StressLevel < 0.1f ? GameModel.StressLevel : 0.1f;
        ResetTimerBar();
    }

    private void ChangeTimerBar()
    {
        timerBar.value -= valuePerSecond * Time.deltaTime;
    }

    private void ResetTimerBar()
    {
        valuePerSecond = 0;
        timerBar.value = 1;
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(false);
        GameModel.BuffedDeers.Remove(this.gameObject);
    }

    private void Move()
    {
        var distanceToTarget = Vector2.Distance(transform.localPosition, TargetPos);
        print(distanceToTarget);
        TargetPoint.transform.position = TargetPos;
        if (distanceToTarget < 0.65f && !IsWaiting)
        {
            StartCoroutine(WaitAndChangeTargetPose(UnityEngine.Random.Range(0, 3)));
        }
        else if (!IsWaiting)
            transform.position = Vector2.MoveTowards(transform.position, TargetPos, Speed * Time.deltaTime);
    }

    private IEnumerator WaitAndChangeTargetPose(int time)
    {
        IsWaiting = true;
        IsSpawned = true;
        Speed = 0;
        yield return new WaitForSeconds(time);
        TargetPos = DeerSpawner.GenerateNewPosition();
        Speed = Random.Range(0.65f, 1.7f);
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