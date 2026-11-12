using CharacterController.SaverioScript.PlayerControllers.CharacterController;
using Script.PlayerControllers;
using UnityEngine;

namespace CharacterController.Script.PlayerControllers
{
    public class AnimationHandler : MonoBehaviour, IAptitude
    {
        [Header("Setup")]
        [SerializeField] private float smoothing = 0.2f;
        [SerializeField] private float runningSmoothing = 0.25f;
        [SerializeField] private float runBlendValue = 1.5f;

        private static readonly int XHash = Animator.StringToHash("X");
        private static readonly int YHash = Animator.StringToHash("Y");

        private CharacterSetup setup;
        private UnityEngine.CharacterController controller;

        public void Initialize(CharacterSetup characterSetup)
        {
            setup = characterSetup;
            controller = setup.MovementBase.controller;
        }

        private void Update()
        {
            if (setup == null || setup.MoveState is VaultState)
                return;

            UpdateLocomotion();
        }

        private void UpdateLocomotion()
        {
            var maxSpeed = setup.CharacterData.playerRunVelocity;
            if (maxSpeed < 0.01f) return;

            var localVelocity = controller.transform.InverseTransformDirection(
                setup.MovementBase.CurrentMoveDirection);
            var normalized = Vector3.ClampMagnitude(localVelocity / maxSpeed, 1f) * runBlendValue;

            var damp = setup.ReadInput.IsRunning ? runningSmoothing : smoothing;
            setup.Animator.SetFloat(XHash, normalized.x, damp, Time.deltaTime);
            setup.Animator.SetFloat(YHash, normalized.z, damp, Time.deltaTime);
        }
    }
}