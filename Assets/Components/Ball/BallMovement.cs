using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Mathematics;

public class BallMovement : MonoBehaviour
{
    public float maxSpeed;
    public float speed;
    private float currentSpeed;
    public float velocityMultiplier;
    public BallStateEnum ballState;
    private Rigidbody2D ballRigidbody;
    BallAudio ballAudio;
    public GameManager gameManager;
    System.Random random = new();
    public string launchAxis = "LaunchBall";

    // Start is called before the first frame update
    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody2D>();
        ballAudio = GetComponent<BallAudio>();
        random = new System.Random();
        StartMatch(gameManager.playerInTurn);
    }

    public delegate void ExecuteBallUpdate();
    public ExecuteBallUpdate executeBallUpdate;

    // Update is called once per frame
    void Update()
    {
        executeBallUpdate();
    }

    void FollowingPlayerUpdate()
    {
        /*Vector2 newPosition = gameManager.players[gameManager.playerInTurn].paddleMovement.GetPosition();
        newPosition.y += 2;
        ballRigidbody.position = newPosition;*/
        bool launch = MobileLauch();
        if (launch == true)
        {
            Launch(gameManager.playerInTurn);
        }

    }
    private bool PCLaunch()
    {
        return Input.GetAxis(launchAxis) > 0;
    }
    private bool MobileLauch()
    {
        return Input.touchCount > 0 && gameManager.GetPlayerInTurn().paddleMovement.IsMoving();
    }

    void LaunchedUpdate()
    {
    }

    public void StartMatch(PlayersEnum player)
    {
        currentSpeed = speed;
        ballRigidbody.isKinematic = true;
        gameObject.transform.SetParent(gameManager.players[player].gameObject.transform);
        gameObject.transform.position = gameManager.players[player].ballTransform.position;
        executeBallUpdate = FollowingPlayerUpdate;
    }

    public void Restart(PlayersEnum player)
    {
        ballRigidbody.velocity = new Vector2(0, 0);
        StartMatch(player);
    }

    public void Launch(PlayersEnum player)
    {
        // Generate a random floating-point number between -1 and 1
        float yRandom = -1;
        float xRandom = (float)(random.NextDouble() * 2) - 1;

        if (player == PlayersEnum.PLAYER_1)
        {
            yRandom = 1;
        }

        ballRigidbody.isKinematic = false;
        gameObject.transform.SetParent(null);
        executeBallUpdate = LaunchedUpdate;

        UpdateVelocity(new Vector2(xRandom, yRandom).normalized);
    }
    void UpdateVelocity(Vector2 direction)
    {
        currentSpeed = Math.Min(currentSpeed * 1 + velocityMultiplier, maxSpeed);
        ballRigidbody.velocity = new Vector2(direction.x * currentSpeed, direction.y * currentSpeed);
        Debug.Log(("VELOCTY", ballRigidbody.velocity));
        Debug.Log(("DIRECTION", direction));
        Debug.Log(("CURRENT SPEED", currentSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            UpdateVelocity(new Vector2(
                (float)(ballRigidbody.velocity.x + random.NextDouble() * 10 - 5),
                ballRigidbody.velocity.y
            ).normalized);
            gameManager.SetCurrentPlayerInTurn(collision.gameObject.GetComponent<PaddleMovement>().player);
            ballAudio.onHitPaddle();
        }
        else if (collision.gameObject.CompareTag("ExploitableBlock"))
        {
            UpdateVelocity(new Vector2(
                (float)(ballRigidbody.velocity.x + random.NextDouble() * 10 - 5),
                ballRigidbody.velocity.y
            ).normalized);
            collision.gameObject.GetComponent<ExplotaibleBlock>().OnHit();
            Score(gameManager.playerInTurn);
            ballAudio.onHitWall();
        }
        else
        {
            ballAudio.onHitWall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("TRIGGER");
        if (collider.gameObject.CompareTag("DeadZone"))
        {
            Debug.Log("LOSELIVE");
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
}
