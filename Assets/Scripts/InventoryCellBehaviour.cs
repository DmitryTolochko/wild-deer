using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class InventoryCellBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector3 startPosition;
    private bool isDragged;
    private bool isInBounds;
    private bool isPlaced;
    private const int mouseSpeed = 500;
    public int Count { get; set; }
    void Start()
    {
        Count = 10;
        rb = GetComponent<Rigidbody2D>();
        var temp = transform.position;
        startPosition = new Vector3(temp.x, temp.y, 0);
    }
    
    void Update()
    {
        if (!isPlaced && isDragged) 
        {
            var cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var newPos = new Vector2(cursorPosition.x, cursorPosition.y);
            rb.MovePosition (Vector2.MoveTowards(transform.position, newPos,  mouseSpeed * Time.deltaTime));
        }
    }
    

    private void OnMouseDown()
    {
        isDragged = true;
    }

    private void OnMouseUp()
    {
        isDragged = false;
        if (isInBounds)
        {
            Debug.Log("PENIS");
            isPlaced = true;
        }
        else
        {
            transform.position = startPosition;
        }
    }
    
    
    private void OnTriggerStay2D(Collider2D other)
    {
        // collisionInfo.collider.CompareTag("GameField")
        // GameObject.FindWithTag("GameField")
        Debug.Log("PENIS");
        isInBounds = other.CompareTag("GameField");
    }

    private void OnTriggerEnter(Collider other)
    {
        isInBounds = other.CompareTag("GameField");
    }

    private void OnTriggerExit(Collider other)
    {
        isInBounds = false;
    }
}