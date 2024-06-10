using System;
using UnityEngine;
namespace Stariluz.GameControl
{
    [Serializable]
    public abstract class MultiplatformBehaviour<T>
    {
        public delegate T ControlBehaviour();
        public ControlBehaviour ExecuteBehaviour;
        public abstract T PCBehaviour();
        public abstract T TouchMobileBehaviour();
        public abstract T ScreenButtonsBehaviour();
        public MultiplatformBehaviour()
        {
            SetBehaviourToExecute(ControlsEnum.PC);
        }
        public MultiplatformBehaviour(ControlsEnum controlsEnum)
        {
            SetBehaviourToExecute(controlsEnum);
        }
        public virtual void SetBehaviourToExecute(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.PC:
                    {
                        ExecuteBehaviour = PCBehaviour;
                        break;
                    }
                case ControlsEnum.Touch:
                    {
                        ExecuteBehaviour = TouchMobileBehaviour;
                        break;
                    }
                case ControlsEnum.ScreenButtons:
                    {
                        ExecuteBehaviour = ScreenButtonsBehaviour;
                        break;
                    }
            }
        }
    }
    [Serializable]
    public abstract class MultiplatformBehaviour
    {
        public delegate void ControlBehaviour();
        public ControlBehaviour ExecuteBehaviour;
        public abstract void PCBehaviour();
        public abstract void TouchMobileBehaviour();
        public abstract void ScreenButtonsBehaviour();
        public MultiplatformBehaviour()
        {
            SetBehaviourToExecute(ControlsEnum.PC);
        }
        public MultiplatformBehaviour(ControlsEnum controlsEnum)
        {
            SetBehaviourToExecute(controlsEnum);
        }
        public virtual void SetBehaviourToExecute(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.PC:
                    {
                        ExecuteBehaviour = PCBehaviour;
                        break;
                    }
                case ControlsEnum.Touch:
                    {
                        ExecuteBehaviour = TouchMobileBehaviour;
                        break;
                    }
                case ControlsEnum.ScreenButtons:
                    {
                        ExecuteBehaviour = ScreenButtonsBehaviour;
                        break;
                    }
            }
        }
    }
    public enum ControlsEnum
    {
        PC,
        Touch,
        ScreenButtons
    }
}