using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Stariluz.GameControl
{
    public class JoysticController : MonoBehaviour
    {
        public RectTransform joystick;
        public RectTransform joystickLimits;

        public delegate void JoysticBehaviour();
        public JoysticBehaviour ExecuteBehaviour;
        private Vector2 _movement;
        public Vector2 movement
        {
            get => _movement;
            set => _movement = value;
        }
        private bool isMoving = false;
        private float joyconLimitRadius = 0;
        void Start()
        {
            ExecuteBehaviour = UpdatePlaying;
            joyconLimitRadius = joystickLimits.rect.width / 2;
            if (PlayerPrefs.HasKey("Sensibility"))
            {
                LoadSensibility();
            }
            else
            {
                SetSensibilityFromSlider();
            }
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


        public float sensibility;
        public Slider sensibilitySlider;
        public void SetSensibilityFromSlider()
        {
            sensibility =sensibilitySlider.value;
        }
        public void LoadSensibility()
        {
            sensibilitySlider.value = PlayerPrefs.GetFloat("Sensibility");
            SetSensibilityFromSlider();
        }
        private Vector2 originPosition;
        private Vector2 lastPosition;

        private float movementLimit;
        public void UpdatePlaying()
        {
            movementLimit = joyconLimitRadius / sensibility;
            if (Input.touchCount > 0)
            {

                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began&&!EventSystem.current.IsPointerOverGameObject(touch.fingerId))
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