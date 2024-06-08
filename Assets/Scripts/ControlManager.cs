using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stariluz.GameControl;

public class ControlsManager : MonoBehaviour
{
    public PaddleMovement paddleMovement;

    public ControlsEnum selectedControl = ControlsEnum.PC;
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        paddleMovement.movementInput.SetBehaviourToExecute(selectedControl);
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
                    camera.orthographicSize = 19f;
                    break;
                }
            case ControlsEnum.Touch:
                {
                    camera.orthographicSize = 15f;
                    break;
                }
            case ControlsEnum.ScreenButtons:
                {
                    camera.orthographicSize = 15f;
                    break;
                }
        }
    }
}