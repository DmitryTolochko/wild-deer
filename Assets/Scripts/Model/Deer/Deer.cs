using System.Collections;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum BuffType 
{
    No,
    Ill,
    Hunger,
    Thirsty,
}

public class Deer : MonoBehaviour
{
    public float speed;
    public bool IsWaiting = false;

    private Rigidbody2D rb;
    public Vector2 TargetPos = new Vector2();
    private BuffType buffType;

    public bool IsPairing = false;
    public bool IsAVictim = false;

    public Age CurrentAge;
    public Gender DeerGender;

    private Slider timerBar;
    private Slider lifeBar;
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
        timerBar = transform.Find("DeerUI").transform.Find("Slider").GetComponent<Slider>();
        lifeBar = transform.Find("DeerUI").transform.Find("LifeBar").GetComponent<Slider>();
        lifeBar.value = 1;
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
        yield return new WaitForSecondsRealtime(60);
        GetComponent<DeerAnimator>().ChangeSprite(Age.Adult, DeerGender);
        CurrentAge = Age.CanMakeChild;
        print("Должен сделать детей");
        yield return new WaitForSecondsRealtime(25);
        CurrentAge = Age.Adult;
        print("Взрослый");
        yield return new WaitForSecondsRealtime(515);
        CurrentAge = Age.Dead;
        print("Умер");
    }

    public IEnumerator GetBuff(BuffType newBuff)
    {
        transform.Find("DeerUI").transform.Find("Slider").gameObject.SetActive(true);
        buffType = newBuff;

        switch (newBuff)
        {
            case BuffType.Hunger:
                valuePerSecond = 0.1f;
                yield return new WaitForSecondsRealtime(10);
                break;
            case BuffType.Ill:
                valuePerSecond = 0.06f;
                yield return new WaitForSecondsRealtime(15);
                break;
            case BuffType.Thirsty:
                valuePerSecond = 0.05f;
                yield return new WaitForSecondsRealtime(20);
                break;
        }
        if (buffType != BuffType.No)
            CurrentAge = Age.Dead;
        buffType = BuffType.No;
        ResetTimerBar();
    }

    public void StopBuff(BuffType newBuff)
    {
        StopCoroutine(GetBuff(newBuff));
        buffType = BuffType.No;
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
        float distanceToTarget = Vector2.Distance(transform.localPosition, TargetPos);
        // print(transform.position);
        // print(TargetPos);
        // print(distanceToTarget);
        if (distanceToTarget < 0.55f && !IsWaiting)
        {
            StartCoroutine(WaitAndChangeTargetPose(UnityEngine.Random.Range(0, 3)));
        }
        else if (!IsWaiting)
            transform.position = Vector2.MoveTowards(transform.position, TargetPos, speed * Time.deltaTime);
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