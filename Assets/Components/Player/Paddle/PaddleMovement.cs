using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    public float speed = 7f;

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
        float movement = Input.GetAxis(inputAxis);
        paddleRB.velocity = new Vector2(movement * speed, paddleRB.velocity.y);

    }

    public void Restart()
    {
        gameObject.transform.position = initialPosition;
        paddleRB.velocity = new Vector2(0, 0);
    }

    public Vector2 GetPosition()
    {
        return paddleRB.position;
    }
}
