using UnityEngine;
namespace Stariluz.GameControl
{
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
    public enum ControlsEnum
    {
        PC,
        Touch,
        ScreenButtons
    }
}