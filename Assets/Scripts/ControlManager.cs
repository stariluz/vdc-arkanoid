using UnityEngine;
using Stariluz.GameControl;
using UnityEditor;

[ExecuteInEditMode] // This makes the script run even in the editor
public class ControlsManager : MonoBehaviour
{
    public ControlsEnum selectedControl = ControlsEnum.PC;

    [Header("References")]
    public PaddleMovement paddleMovement;
    public BallMovement ballMovement;
    public GameObject uITouch;
    public GameObject keyListener;
    public Camera gameCamera;

    // Called only in Play Mode
    void Start()
    {
        ApplyControlSettings();
    }

    // Called in Editor when something changes
    private void OnValidate()
    {
        // To avoid running code if not all references are assigned
        if (paddleMovement == null || ballMovement == null || gameCamera == null)
            return;

        ApplyControlSettings();
    }

    private void ApplyControlSettings()
    {
        // Schedule the method to be called in the next frame if we're in Editor Mode
        if (Application.isEditor && !EditorApplication.isPlaying)
        {
            EditorApplication.delayCall += () =>
            {
                paddleMovement.movementInput.SetBehaviourToExecute(selectedControl);
                ballMovement.launchAddListener.SetBehaviourToExecute(selectedControl);
                ballMovement.launchRemoveListener.SetBehaviourToExecute(selectedControl);
                ballMovement.ballStartBehaviour.SetBehaviourToExecute(selectedControl);

                // Important: don't destroy objects in editor mode! (it would break the scene)
                // Instead, enable/disable them.
                if (uITouch != null) uITouch.SetActive(selectedControl == ControlsEnum.Touch || selectedControl == ControlsEnum.ScreenButtons);
                if (keyListener != null) keyListener.SetActive(selectedControl == ControlsEnum.PC);

                AdjustCamera(selectedControl);
            };
        }
    }
    private void AdjustCamera(ControlsEnum control)
    {
        if (gameCamera == null)
            return;

        switch (control)
        {
            case ControlsEnum.PC:
                gameCamera.orthographicSize = 19f;
                break;
            case ControlsEnum.Touch:
            case ControlsEnum.ScreenButtons:
                gameCamera.orthographicSize = 15f;
                break;
        }
    }

}
