using System;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Vector3 cursorPosition = new Vector3();
    private const int mouseSpeed = 200;
    private const int buttonSpeed = 3;
    private Vector2 newPos = new Vector2();
    private bool isMoving = false;

    public Rigidbody2D rb;

    void Update()
    {
        rb.rotation = 0;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPos = new Vector2(cursorPosition.x, cursorPosition.y);
            isMoving = true;
        }

        if (isMoving)
        {
            rb.MovePosition (Vector2.MoveTowards (transform.position, newPos, mouseSpeed * Time.deltaTime));
            if (Vector2.Distance(transform.position, newPos)<= 0.01)
				isMoving = false;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * buttonSpeed;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * buttonSpeed;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * buttonSpeed;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * buttonSpeed;
        }
    }
}