using CharacterController.SaverioScript.PlayerControllers.CharacterController.MoveState;
using CharacterController.Script.PlayerControllers;
using Script.PlayerControllers;

namespace CharacterController.Script
{
	public class NormalWalk : IMoveState
	{
		public float MoveSpeed { get; set; }

		public void EnterState(CharacterSetup characterSetup)
		{
			characterSetup.Animator.SetTrigger("Normal");
			MoveSpeed = characterSetup.CharacterData.playerVelocity;
		}

		public void UpdateState(CharacterSetup characterSetup)
		{
			if (characterSetup.ReadInput.IsCrunching) characterSetup.GoTo(new CrounchState());
			if (characterSetup.ReadInput.IsRunning) characterSetup.GoTo(new RunState());
			if (characterSetup.ReadInput.IsFighting) characterSetup.GoTo(new FightState());
			if (characterSetup.ReadInput.isJumping && characterSetup.Climb) characterSetup.GoTo(new ClimbState());
		}

		public void ExitState(CharacterSetup characterSetup)
		{
		}
	}
}