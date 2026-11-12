using CharacterController.Script;

namespace CharacterController.SaverioScript.PlayerControllers.CharacterController.MoveState
{
    public class RunState : IMoveState
    {
        public float MoveSpeed { get; set; }
        public void EnterState(CharacterSetup characterSetup)
        {
            MoveSpeed = characterSetup.CharacterData.playerRunVelocity;
        }

        public void UpdateState(CharacterSetup characterSetup)
        {
            if(!characterSetup.ReadInput.IsRunning)
                characterSetup.GoTo(new NormalWalk());
        }

        public void ExitState(CharacterSetup characterSetup)
        {
        }
    }
}