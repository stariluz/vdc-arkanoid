namespace Stariluz.GameControl
{
    public abstract class MultiplatformBehaviour<T>
    {
        public delegate T ControlBehaviour();
        public ControlBehaviour currentBehaviour;
        public abstract T PCBehaviour();
        public abstract T TouchMobileBehaviour();
        public abstract T ScreenButtonsBehaviour();

        public virtual T SetCurrentBehaviour(ControlBehaviour behaviour){
            currentBehaviour=behaviour;
        }
    }
    public enum ControlsEnum
    {
        PC,
        Touch,
        ScreenButtons
    }
}