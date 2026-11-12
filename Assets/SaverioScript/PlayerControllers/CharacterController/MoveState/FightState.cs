using CharacterController.Script;

namespace CharacterController.SaverioScript.PlayerControllers.CharacterController.MoveState
{
	public class FightState : IMoveState
	{
		public float MoveSpeed { get; set; }

		public void EnterState(CharacterSetup characterSetup)
		{
			characterSetup.Animator.SetBool("fight", true);
			MoveSpeed = characterSetup.CharacterData.playerVelocity-0.75f;
		}


		public void UpdateState(CharacterSetup characterSetup)
		{
			if (!characterSetup.ReadInput.IsFighting) characterSetup.GoTo(new NormalWalk());
		}

		public void ExitState(CharacterSetup characterSetup)
		{
			characterSetup.Animator.SetBool("fight", false);
		}
	}
}