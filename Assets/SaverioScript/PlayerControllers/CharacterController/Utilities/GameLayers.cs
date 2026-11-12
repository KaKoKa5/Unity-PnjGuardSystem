using UnityEngine;

namespace CharacterController.Script.PlayerControllers.Utilities
{
	public static class GameLayers
	{
		public static readonly LayerMask Wall = LayerMask.GetMask("Wall");
		public static readonly LayerMask Vaultable = LayerMask.GetMask("Vault");
		public static readonly LayerMask Player = LayerMask.GetMask("Player");
		public static LayerMask Ground = LayerMask.GetMask("Default");
		public static LayerMask Climb = LayerMask.GetMask("Climb");
	}
}