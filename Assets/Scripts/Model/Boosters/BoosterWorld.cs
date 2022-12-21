using System.Linq;
using Model.Inventory;
using ServiceInstances;
using Unity.VisualScripting;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BoosterWorld : MonoBehaviour
{
    private Collider2D gameFieldBounds;
    private BoxCollider2D boosterCollider;
    private Vector3 startPosition;
    private Vector3 mousePositionOffset;
    private Vector3 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);
    public BoosterType Type { get; private set; }
    private Timer capTimer;

    private void Start()
    {
        startPosition = transform.position;
        boosterCollider = GetComponent<BoxCollider2D>();
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
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        var trashCan = GameObject.Find("TrashCan");
        var toTrashCanDistance = Vector2.Distance(
            transform.position,
            trashCan.transform.position
        );

        if (toTrashCanDistance < 1)
        {
            Inventory.RemoveItem(Type);
        }

        var toReturnDistanceLimit = Type == BoosterType.ProtectiveCap ? -3 : -0.5;
        if (gameFieldBounds.Distance(boosterCollider).distance > toReturnDistanceLimit && Type != BoosterType.PinkTrap)
        {
            ReturnBoosterToInventory();
        }

        switch (Type)
        {
            case BoosterType.ProtectiveCap:
            {
                var toPlaceParent = GameObject.Find("GamePlay").transform;
                if (toPlaceParent.Find("ProtectiveCap(Clone)"))
                {
                    ReturnBoosterToInventory();
                    return;
                }

                var capTransform = Instantiate(
                    BoostersAssets
                        .Instance
                        .protectiveCapBoosterPrefab,
                    toPlaceParent);
                capTransform.position = new Vector3(0, 3.25f, 0);
                Inventory.UseBooster(Type);
                Destroy(gameObject);
                break;
            }
            case BoosterType.Food or BoosterType.Water or BoosterType.Medicines:
            {
                var minDistance = 10e9f;
                var deerToFeed = default(GameObject);
                foreach (var deer in GameModel.Deers)
                {
                    var a = transform.position;
                    var b = deer.transform.position;
                    var currentDistance = Vector2.Distance(a, b);
                    if (currentDistance < 1 && currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        deerToFeed = deer;
                    }
                }

                if (deerToFeed == null || deerToFeed.GetComponent<Deer>().BuffType == BuffType.No)
                {
                    ReturnBoosterToInventory();
                    return;
                }

                var deerComponent = deerToFeed.GetComponent<Deer>();

                var toUseBoosterType = GameModel.GetBoosterTypeByBuffType(deerComponent.BuffType);
                if (toUseBoosterType != Type)
                {
                    ReturnBoosterToInventory();
                    return;
                }

                switch (Type)
                {
                    case BoosterType.Food:

                        Inventory.UseBooster(Type);
                        deerComponent.StopBuff(BuffType.Hunger);
                        GameModel.StressLevel -= 0.05f;
                        Destroy(gameObject);
                        break;
                    case BoosterType.Water:
                        Inventory.UseBooster(Type);
                        deerComponent.StopBuff(BuffType.Thirsty);
                        GameModel.StressLevel -= 0.05f;
                        Destroy(gameObject);
                        break;
                    case BoosterType.Medicines:
                        Inventory.UseBooster(Type);
                        deerComponent.StopBuff(BuffType.Ill);
                        GameModel.StressLevel -= 0.05f;
                        Destroy(gameObject);
                        break;
                }

                break;
            }
            case BoosterType.PinkTrap:
            {
                var threatToBeat = GameModel.CurrentThreat;

                if (threatToBeat == null || Vector2.Distance(transform.position, threatToBeat.transform.position) > 1)
                {
                    ReturnBoosterToInventory();
                    return;
                }

                var threatComponent = threatToBeat.GetComponent<BaseThreat>();
                Inventory.UseBooster(Type);
                threatComponent.Status = ThreatStatus.Defeated;
                Destroy(gameObject);
                GameModel.CurrentThreat = null;
                break;
            }
        }
    }

    private void ReturnBoosterToInventory()
    {
        transform.Find("itemAmount").gameObject.SetActive(true);
        transform.position = startPosition;
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