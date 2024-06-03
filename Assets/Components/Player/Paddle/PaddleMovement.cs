using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 4f;
    public float maxVelocity = 15f;

    private Rigidbody2D paddleRB;
    public PlayersEnum player;
    private string inputAxis = "Horizontal";
    private Vector2 initialPosition;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gameObject.transform.position;
        paddleRB = GetComponent<Rigidbody2D>();
        inputAxis = "Horizontal";
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            MobileMovement();
        }
    }
    void PCMovement()
    {
        float movement = Input.GetAxis(inputAxis);
        paddleRB.velocity = new Vector2(movement * speed, paddleRB.velocity.y);
    }
    private bool _isMoving = false;
    public bool IsMoving()
    {
        return _isMoving;
    }
    void MobileMovement()
    {
        // Handle screen touches.
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                _isMoving = true;
                paddleRB.velocity = new Vector2(
                    Math.Clamp((touch.deltaPosition.x / 16) * speed, -maxVelocity, maxVelocity),
                    paddleRB.velocity.y
                );
                // paddleRB.position=new Vector2(touch.position.x, paddleRB.velocity.y);
            }
            else
            {
                _isMoving = false;
                paddleRB.velocity = Vector2.zero;
            }
        }
    }

    public void Restart()
    {
        gameObject.transform.position = initialPosition;
        paddleRB.velocity = new Vector2(0, 0);
    }
    private bool isPaused = false;
    public void Pause()
    {
        paddleRB.velocity = Vector2.zero;
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }

    public Vector2 GetPosition()
    {
        return paddleRB.position;
    }
}
