using System;
using System.Linq;
using Model.Inventory;
using ServiceInstances;
using UnityEngine;
using UnityEngine.UI;

public class BoosterWorld : MonoBehaviour
{
    private bool isPlaced;
    private Collider2D gameFieldBounds;
    private BoxCollider2D boosterCollider;
    private Vector3 startPosition;
    private Vector3 mousePositionOffset;
    private Vector3 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public BoosterType Type { get; private set; }

    private Transform Test;

    private void Start()
    {
        startPosition = transform.position;
        boosterCollider = GetComponent<BoxCollider2D>();
        Test = Resources
            .FindObjectsOfTypeAll<Canvas>()
            .FirstOrDefault(x => x.name == "Canvas")?
            .transform;

        gameFieldBounds = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")!.GetComponent<PolygonCollider2D>();
    }

    public void SetBoosterType(BoosterType type)
    {
        Type = type;
    }

    private void OnMouseDown()
    {
        transform.Find("itemAmount").gameObject.SetActive(false);
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isPlaced)
            transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (gameFieldBounds.Distance(boosterCollider).distance < -0.5)
        {
            isPlaced = true;
            transform.SetParent(Test);

            Destroy(transform.Find("itemAmount").GetComponent<Text>());
            Inventory.UseBooster(Type);
            return;
        }

        transform.Find("itemAmount").gameObject.SetActive(true);
        transform.position = startPosition;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isPlaced)
        {
            return;
        }

        if (Type == BoosterType.ProtectiveCap)
        {
            return;
        }

        if (other.TryGetComponent<Deer>(out var deer))
        {
            switch (Type)
            {
                case BoosterType.Food:
                    deer.IsHungry = false;
                    Destroy(gameObject);
                    break;
                case BoosterType.Water:
                    deer.IsThirsty = false;
                    Destroy(gameObject);
                    break;
            }

            return;
        }

        if (other.TryGetComponent<BaseThreat>(out var threat))
        {
            if (threat.BoosterTypes.Contains(Type))
                threat.Status = ThreatStatus.Defeated;
            Destroy(gameObject);
        }

        if (other.TryGetComponent<KillerPoacher>(out var killerPoacher))
        {
            killerPoacher.Status = ThreatStatus.Defeated;
            Destroy(gameObject);
            return;
        }

        if (other.TryGetComponent<ArcticFox>(out var arcticFox))
        {
            arcticFox.Status = ThreatStatus.Defeated;
            Destroy(gameObject);
        }
    }
}


/*
using System;
using System.Linq;
using Model.Inventory;
using ServiceInstances;
using UnityEngine;
using UnityEngine.UI;

public class BoosterWorld : MonoBehaviour
{
    private bool isPlaced;
    private Collider2D gameFieldBounds;
    private BoxCollider2D boosterCollider;
    private Vector3 startPosition;
    private Vector3 mousePositionOffset;
    private Vector3 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    public BoosterType Type { get; private set; }

    private Transform Test;

    private void Start()
    {
        startPosition = transform.position;
        boosterCollider = GetComponent<BoxCollider2D>();
        Test = Resources
            .FindObjectsOfTypeAll<Canvas>()
            .FirstOrDefault(x => x.name == "Canvas")?
            .transform;

        gameFieldBounds = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")!.GetComponent<PolygonCollider2D>();
    }

    public void SetBoosterType(BoosterType type)
    {
        Type = type;
    }

    private void OnMouseDown()
    {
        transform.Find("itemAmount").gameObject.SetActive(false);
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isPlaced)
            transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (gameFieldBounds.Distance(boosterCollider).distance < -0.5)
        {
            isPlaced = true;
            transform.SetParent(Test);

            Destroy(transform.Find("itemAmount").GetComponent<Text>());
            Inventory.UseBooster(Type);
            return;
        }

        transform.Find("itemAmount").gameObject.SetActive(true);
        transform.position = startPosition;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!isPlaced)
        {
            return;
        }

        if (Type == BoosterType.ProtectiveCap)
        {
            return;
        }

        if (other.TryGetComponent<Deer>(out var deer))
        {
            switch (Type)
            {
                case BoosterType.Food:
                    deer.IsHungry = false;
                    Destroy(gameObject);
                    break;
                case BoosterType.Water:
                    deer.IsThirsty = false;
                    Destroy(gameObject);
                    break;
            }

            return;
        }

        if (other.TryGetComponent<BaseThreat>(out var threat))
        {
            if (threat.BoosterTypes.Contains(Type))
                threat.Status = ThreatStatus.Defeated;
            Destroy(gameObject);
        }
        if (other.TryGetComponent<KillerPoacher>(out var killerPoacher))
        {
            Debug.Log("ПОШЕЛ ОТСЮДА!");
            killerPoacher.Status = ThreatStatus.Defeated;
            Destroy(gameObject);
            return;
        }

        if (
            other.TryGetComponent<ArcticFox>(out var arcticFox))
        {
            Debug.Log("ПОШЕЛ ОТСЮДА!");
            arcticFox.Status = ThreatStatus.Defeated;
            Destroy(gameObject);
        }
    }
}
*/