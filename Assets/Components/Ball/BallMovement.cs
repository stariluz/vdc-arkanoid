using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using Stariluz.GameControl;
using UnityEngine.UI;

public class BallMovement : MonoBehaviour
{

    public float maxSpeed;
    public float speed;
    private float currentSpeed;
    public float velocityMultiplier;
    public BallStateEnum ballState;
    private Rigidbody2D ballRigidbody;
    public BallAudio ballAudio;
    public GameManager gameManager;
    System.Random random = new();
    public delegate void ExecuteBallUpdate();
    public ExecuteBallUpdate executeBallUpdate;
    public BallLaunchAddListenerBehaviour launchAddListener;
    public BallLaunchRemoveListenerBehaviour launchRemoveListener;
    public BallStartBehaviour ballStartBehaviour;

    BallMovement()
    {
        launchAddListener = new BallLaunchAddListenerBehaviour(this);
        launchRemoveListener = new BallLaunchRemoveListenerBehaviour(this);
        ballStartBehaviour = new BallStartBehaviour(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        random = new System.Random();
        InitBall();
    }
    void OnDisable()
    {
        launchRemoveListener.ExecuteBehaviour();
    }

    public void InitBall()
    {
        StartState(gameManager.playerInTurn);
    }
    // Update is called once per frame
    void Update()
    {
        executeBallUpdate();
    }

    void FollowingPlayerUpdate()
    {
        Vector2 newPosition = gameManager.players[gameManager.playerInTurn].paddleMovement.GetPosition();
        newPosition.y += 2;
        ballRigidbody.position = newPosition;
    }

    void LaunchedUpdate()
    {
    }

    void PauseUpdate()
    {
    }
    bool isFirstLaunch = false;
    public void StartGameLevel(PlayersEnum player)
    {
        isFirstLaunch = true;
        StartState(player);
    }
    public void StartState(PlayersEnum player)
    {
        launchAddListener.ExecuteBehaviour();
        ballStartBehaviour.ExecuteBehaviour();
        currentSpeed = speed;
        ballRigidbody.isKinematic = true;
        gameObject.transform.SetParent(gameManager.players[player].gameObject.transform);
        gameObject.transform.position = gameManager.players[player].ballTransform.position;
        executeBallUpdate = FollowingPlayerUpdate;
    }

    public void Restart(PlayersEnum player)
    {
        ballRigidbody.velocity = new Vector2(0, 0);
        StartState(player);
    }

    private Vector2 savedVelocity;
    private ExecuteBallUpdate savedUpdate;
    private bool isPaused = false;
    public void Pause()
    {
        savedVelocity = ballRigidbody.velocity;
        ballRigidbody.velocity = Vector2.zero;
        savedUpdate = executeBallUpdate;
        executeBallUpdate = PauseUpdate;
        isPaused = true;
    }
    public void Resume()
    {
        ballRigidbody.velocity = savedVelocity;
        executeBallUpdate = savedUpdate;
        isPaused = false;
    }
    public void Launch()
    {
        launchRemoveListener.ExecuteBehaviour();
        if (isFirstLaunch)
        {
            isFirstLaunch = false;
            gameManager.FirstLaunchBall();
        }

        ballRigidbody.isKinematic = false;
        gameObject.transform.SetParent(null);
        executeBallUpdate = LaunchedUpdate;

        float yRandom = 1;
        float xRandom = (float)(random.NextDouble() * 2) - 1;
        UpdateVelocity(RandomizeDirectionOnLaunch(new Vector2(xRandom, yRandom)).normalized);
    }
    void UpdateVelocity(Vector2 direction)
    {
        currentSpeed = Math.Min(currentSpeed * 1 + velocityMultiplier, maxSpeed);
        ballRigidbody.velocity = new Vector2(direction.x * currentSpeed, direction.y * currentSpeed);
        // Debug.Log(("VELOCTY", ballRigidbody.velocity));
        // Debug.Log(("DIRECTION", direction));
        // Debug.Log(("CURRENT SPEED", currentSpeed));
    }
    public float randomizeTrajectoryRange = 5;
    private Vector2 RandomizeDirectionOnLaunch(Vector2 vector)
    {
        return new Vector2(
            0.3f * vector.x
            + 0.4f * gameManager.players[PlayersEnum.PLAYER_1].paddleMovement.rb.velocity.normalized.x
            + 0.3f * randomizeTrajectoryRange * ((float)random.NextDouble() * 2 - 1),
            vector.y
        );
    }
    private Vector2 RandomizeDirectionOnPaddle(Vector2 vector)
    {
        Vector2 normalized = vector.normalized;
        Debug.Log(normalized.x);
        if (normalized.x > -0.3 && normalized.x < 0.3)
        {
            normalized.x = Math.Sign(normalized.x) * (1 + (float)random.NextDouble() * 2);
            Debug.Log(("CORRECTED X", normalized.x));
        }
        if (normalized.y > -0.3 && normalized.y < 0.3)
        {
            normalized.y = Math.Sign(normalized.y) * (1 + (float)random.NextDouble() * 2);
            Debug.Log(("CORRECTED Y", normalized.x));
        }
        return new Vector2(
            0.6f * normalized.x
            + 0.2f * gameManager.players[PlayersEnum.PLAYER_1].paddleMovement.rb.velocity.normalized.x
            + 0.2f * randomizeTrajectoryRange * ((float)random.NextDouble() * 2 - 1),
            normalized.y
        );
    }
    private Vector2 RandomizeDirection(Vector2 vector)
    {
        return new Vector2(
            0.6f * vector.x
            + 0.4f * randomizeTrajectoryRange * ((float)random.NextDouble() * 2 - 1),
            vector.y
        );
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            UpdateVelocity(RandomizeDirectionOnPaddle(ballRigidbody.velocity).normalized);
            ballAudio.OnHitPaddle();
        }
        else if (collision.gameObject.CompareTag("ExploitableBlock"))
        {
            UpdateVelocity(RandomizeDirection(ballRigidbody.velocity).normalized);
            collision.gameObject.GetComponent<ExplotaibleBlock>().OnHit();
            ballAudio.OnHitWall();
            Score(gameManager.playerInTurn);
        }
        else
        {
            ballAudio.OnHitWall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("DeadZone"))
        {
            LoseLive();
        }
    }

    void LoseLive()
    {
        gameManager.LoseLive();
        Restart(gameManager.playerInTurn);
    }

    void Score(PlayersEnum player)
    {
        gameManager.Score(player);
    }

    [SerializeField]
    KeyListener keyListener;
    void HandleKeyDown(KeyCode keyCode)
    {
        if (keyCode == KeyCode.Space && !isPaused) Launch();
    }
    [SerializeField]
    Button launchButton;
    void HandleUITouch()
    {
        // Debug.Log("CLICKED");
        if (!isPaused)
        {
            launchButton.gameObject.SetActive(false);
            Launch();
        }
    }
    public class BallLaunchAddListenerBehaviour : MultiplatformBehaviour
    {
        private BallMovement gameObject;
        public BallLaunchAddListenerBehaviour(BallMovement gameObject)
        {
            this.gameObject = gameObject;
        }
        public override void PCBehaviour()
        {
            gameObject.keyListener.OnKeyDown += gameObject.HandleKeyDown;
        }
        public override void TouchMobileBehaviour()
        {
            gameObject.launchButton.onClick.AddListener(gameObject.HandleUITouch);
        }
        public override void ScreenButtonsBehaviour()
        {
            gameObject.launchButton.onClick.AddListener(gameObject.HandleUITouch);
        }
    }
    public class BallLaunchRemoveListenerBehaviour : MultiplatformBehaviour
    {
        private BallMovement gameObject;
        public BallLaunchRemoveListenerBehaviour(BallMovement gameObject)
        {
            this.gameObject = gameObject;
        }

        public override void PCBehaviour()
        {
            gameObject.keyListener.OnKeyDown -= gameObject.HandleKeyDown;
        }
        public override void TouchMobileBehaviour()
        {
            gameObject.launchButton.onClick.RemoveListener(gameObject.HandleUITouch);
        }
        public override void ScreenButtonsBehaviour()
        {
            gameObject.launchButton.onClick.RemoveListener(gameObject.HandleUITouch);
        }
    }
    public class BallStartBehaviour : MultiplatformBehaviour
    {
        private BallMovement gameObject;
        public BallStartBehaviour(BallMovement gameObject)
        {
            this.gameObject = gameObject;
        }

        public override void PCBehaviour()
        {
        }
        public override void TouchMobileBehaviour()
        {
            gameObject.launchButton.gameObject.SetActive(true);
        }
        public override void ScreenButtonsBehaviour()
        {
            gameObject.launchButton.gameObject.SetActive(true);
        }
    }
}
