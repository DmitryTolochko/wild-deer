using System;
using System.Linq;
using System.Numerics;
using ServiceInstances;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Booster : MonoBehaviour
{
    public BoosterType type { get; }
    private bool isPlaced;
    private Collider2D gameFieldBounds;
    private Vector3 startPosition;
    private Vector3 mousePositionOffset;
    private Vector3 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);


    private void Start()
    {
        startPosition = transform.position;
        gameFieldBounds = Resources
            .FindObjectsOfTypeAll<GameObject>()
            .FirstOrDefault(x => x.name == "GameField")!
            .GetComponent<PolygonCollider2D>();
    }

    private void OnMouseDown()
    {
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        if (!isPlaced)
            transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }

    private void OnMouseUp()
    {
        if (gameFieldBounds.Distance(GetComponent<BoxCollider2D>()).distance < -0.5)
        {
            Debug.Log("Penis");
            isPlaced = true;
            return;
        }

        transform.position = startPosition;
    }
}