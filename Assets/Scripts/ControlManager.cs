using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stariluz.GameControl;

public class ControlsManager : MonoBehaviour
{
    public ControlsEnum selectedControl = ControlsEnum.PC;
    [Header("References")]
    public PaddleMovement paddleMovement;
    public BallMovement ballMovement;
    public JoysticController joysticController;
    public Camera gameCamera;
    // Start is called before the first frame update
    void Start()
    {
        paddleMovement.movementInput.SetBehaviourToExecute(selectedControl);
        ballMovement.launchAddListener.SetBehaviourToExecute(selectedControl);
        ballMovement.launchRemoveListener.SetBehaviourToExecute(selectedControl);
        ballMovement.launchStart.SetBehaviourToExecute(selectedControl);
        joysticController.gameObject.SetActive(false);
        AdjustCamera(selectedControl);
    }
    void ChangeControls()
    {

    }
    private void AdjustCamera(ControlsEnum control)
    {
        switch (control)
        {
            case ControlsEnum.PC:
                {
                    gameCamera.orthographicSize = 19f;
                    break;
                }
            case ControlsEnum.Touch:
                {
                    gameCamera.orthographicSize = 15f;
                    break;
                }
            case ControlsEnum.ScreenButtons:
                {
                    gameCamera.orthographicSize = 15f;
                    break;
                }
        }
    }
}