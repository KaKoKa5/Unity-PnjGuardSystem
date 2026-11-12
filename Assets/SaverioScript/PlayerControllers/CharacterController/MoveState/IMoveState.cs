using CharacterController.Script;
using Script.PlayerControllers;

public interface IMoveState
{
	public float MoveSpeed { get; set; }
	public void EnterState(CharacterSetup characterSetup);
	public void UpdateState(CharacterSetup characterSetup);
	public void ExitState(CharacterSetup characterSetup);
}