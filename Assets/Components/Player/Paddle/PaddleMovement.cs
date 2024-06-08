using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stariluz.GameControl;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 4f;
    public float maxVelocity = 15f;

    private Rigidbody2D rb;
    public PlayersEnum player;
    private string inputAxis = "Horizontal";
    private Vector2 initialPosition;
    public PaddleMovementBehaviour movementInput;

    PaddleMovement(){
        movementInput = new PaddleMovementBehaviour(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
        inputAxis = "Horizontal";
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused)
        {
            movementInput.ExecuteBehaviour();
        }
    }
    private bool _isMoving = false;
    public bool isMoving
    {
        get => _isMoving;
        set => _isMoving = value;
    }

    public void Restart()
    {
        gameObject.transform.position = initialPosition;
        rb.velocity = new Vector2(0, 0);
    }
    private bool isPaused = false;
    public void Pause()
    {
        rb.velocity = Vector2.zero;
        isPaused = true;
    }
    public void Resume()
    {
        isPaused = false;
    }

    public Vector2 GetPosition()
    {
        return rb.position;
    }


    public class PaddleMovementBehaviour : MultiplatformBehaviour<int>
    {
        private PaddleMovement gameObject;
        public PaddleMovementBehaviour(PaddleMovement gameObject)
        {
            this.gameObject = gameObject;
        }
        public override int PCBehaviour()
        {
            float movement = Input.GetAxis(gameObject.inputAxis);
            if (movement != 0)
            {
                gameObject.isMoving = true;
                gameObject.rb.velocity = new Vector2(movement * gameObject.speed, gameObject.rb.velocity.y);
            }
            else
            {
                gameObject.isMoving = false;
            }
            return 0;
        }

        public override int TouchMobileBehaviour()
        {
            // Handle screen touches.
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Moved)
                {
                    gameObject.isMoving = true;
                    gameObject.rb.velocity = new Vector2(
                        Math.Clamp((touch.deltaPosition.x / 16) * gameObject.speed, -gameObject.maxVelocity, gameObject.maxVelocity),
                        gameObject.rb.velocity.y
                    );
                    // rb.position=new Vector2(touch.position.x, rb.velocity.y);
                }
                else
                {
                    gameObject.isMoving = false;
                    gameObject.rb.velocity = Vector2.zero;
                }
            }
            return 0;
        }
        public override int ScreenButtonsBehaviour()
        {
            throw new System.NotImplementedException();
            // return 0;
        }
    }
}
