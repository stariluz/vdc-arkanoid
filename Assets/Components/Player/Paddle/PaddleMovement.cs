using System;
using UnityEngine;
using Stariluz.GameControl;

public class PaddleMovement : MonoBehaviour
{
    public float speedFactor = 4f;
    public float unspeedRate = 1f;
    public float velocity = 0f;
    public float maxVelocity = 15f;

    public Rigidbody2D rb;
    public PlayersEnum player;
    private Vector2 initialPosition;
    [SerializeReference]
    public PaddleMovementBehaviour movementInput = new PaddleMovementBehaviour();

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gameObject.transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    float lastMovementX = 0;
    void Update()
    {
        if (!isPaused)
        {
            float movementX = movementInput.ExecuteBehaviour();
            if (movementX != 0)
            {
                lastMovementX = movementX;
                isMoving = true;
                rb.velocity = new Vector2(
                    Math.Clamp(lastMovementX * speedFactor + rb.velocity.x, -maxVelocity, maxVelocity),
                    rb.velocity.y
                );
            }
            else
            {
                isMoving = false;
                rb.velocity = new Vector2(
                    Math.Clamp(
                        rb.velocity.x != 0 ? rb.velocity.x / (1 + unspeedRate / 10) : rb.velocity.x,
                        -maxVelocity, maxVelocity
                    ),
                    rb.velocity.y
                );
            }
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


    [Serializable]
    public class PaddleMovementBehaviour : MultiplatformBehaviour<float>
    {
        public string inputAxis = "Horizontal";
        public override float PCBehaviour()
        {
            return Input.GetAxis(inputAxis);
        }

        public JoysticController joysticController;
        public override float TouchMobileBehaviour()
        {
            return joysticController.movement.x;
        }
        public override float ScreenButtonsBehaviour()
        {
            Vector2 movement = joysticController.movement;
            return movement.x;
        }
    }

}
