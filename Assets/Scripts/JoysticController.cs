using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Stariluz.GameControl
{
    public class JoysticController : MonoBehaviour
    {
        public RectTransform joystick;
        public RectTransform joystickLimits;
        public float sensibility;

        public delegate void JoysticBehaviour();
        public JoysticBehaviour ExecuteBehaviour;
        private Vector2 _movement;
        public Vector2 movement
        {
            get => _movement;
            set => _movement = value;
        }
        private bool isMoving = false;
        private float widthLimit = 0;
        private float heightLimit = 0;
        void Start()
        {
            ExecuteBehaviour = UpdatePlaying;
            widthLimit = joystickLimits.rect.width / 2;
            heightLimit = joystickLimits.rect.height / 2;
        }
        void Update()
        {
            ExecuteBehaviour();
        }
        public void Pause()
        {
            isMoving = false;
            joystickLimits.gameObject.SetActive(false);
            movement = Vector2.zero;
            joystick.transform.localPosition = Vector2.zero;
            ExecuteBehaviour = UpdatePaused;
        }
        public void Resume()
        {
            ExecuteBehaviour = UpdatePlaying;
        }
        private Vector2 originPosition;
        private Vector2 lastPosition;

        private float movementLimit;
        public void UpdatePlaying()
        {
            movementLimit = widthLimit / sensibility;
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    // Debug.Log(touch.position);
                    isMoving = true;
                    lastPosition = Vector2.zero;
                    originPosition = touch.position;
                    joystickLimits.transform.position = originPosition;
                }
                else if (touch.phase == TouchPhase.Moved && isMoving)
                {
                    joystickLimits.gameObject.SetActive(true);
                    Vector2 newPosition = Vector2.ClampMagnitude(touch.position - originPosition, movementLimit);
                    // Vector2 offsetPosition = newPosition - lastPosition;
                    // if ((offsetPosition).magnitude > sensibility)
                    // {
                    //     newPosition = lastPosition = Vector2.ClampMagnitude(offsetPosition, sensibility);
                    // }
                    movement = newPosition / movementLimit;
                    joystick.transform.localPosition = newPosition * sensibility;
                    lastPosition = newPosition;
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    isMoving = false;
                    joystickLimits.gameObject.SetActive(false);
                    lastPosition = Vector2.zero;
                    movement = Vector2.zero;
                    joystick.transform.localPosition = Vector2.zero;
                }
            }
        }
        public void UpdatePaused() { }

    }
}