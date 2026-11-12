using CharacterController.Script;
using CharacterController.Script.PlayerControllers;
using CharacterController.Script.PlayerControllers.Utilities;
using UnityEngine;

namespace CharacterController.SaverioScript.PlayerControllers.CharacterController.MoveState
{
    public class ClimbState : IMoveState
    {
        private static readonly int ClimbHash = Animator.StringToHash("isClimbing");

        public float MoveSpeed { get; set; }

        public void EnterState(CharacterSetup characterSetup)
        {
            Debug.Log("Entering ClimbState");
            MoveSpeed = characterSetup.CharacterData.climbSpeed;   
            characterSetup.MovementBase.ResetMotion();   
            characterSetup.Animator.SetBool(ClimbHash, true);
        }

        public void UpdateState(CharacterSetup characterSetup)
        {
            var mb = characterSetup.MovementBase;
            
            if (characterSetup.Grounded && characterSetup.ReadInput.Direction.y < -0.1f)
            {
                characterSetup.GoTo(new NormalWalk());
                return;
            }
            var t = characterSetup.MovementBase.controller.transform;
            var origin = t.position + Vector3.up * (characterSetup.MovementBase.controller.height * 0.6f);
            
            if (!RaycastChecker.Check(origin, t.forward, 0.5f, GameLayers.Climb, out RaycastHit hit, new Color(1, 0, 0)))
            {
                characterSetup.GoTo(new NormalWalk());
                return;
            }
            
            if (characterSetup.ReadInput.Direction.y > 0.7f && TryGetLedge(characterSetup,out var top))
            {
                characterSetup.GoTo(new LedgeClimbState(top));
                return;
            }

            mb.Climb(characterSetup, MoveSpeed, hit);
        }

        public void ExitState(CharacterSetup characterSetup)
        {
            characterSetup.Animator.SetBool(ClimbHash, false);
            characterSetup.MovementBase.ResetMotion();
        }
        
        private bool TryGetLedge(CharacterSetup characterSetup, out Vector3 topPoint)
        {
            topPoint = default;
            var t =  characterSetup.MovementBase.controller.transform;
            var height =  characterSetup.MovementBase.controller.height;
            var headOrigin = t.position + Vector3.up * (height + 0.2f);
            var radius =  characterSetup.MovementBase.controller.radius;

            if (RaycastChecker.Check(headOrigin, t.forward, 0.5f, GameLayers.Climb, out _, new Color(1, 1, 0)))
                return false;

            var downOrigin = headOrigin + t.forward * (radius + 0.4f);
            if (RaycastChecker.Check(downOrigin, Vector3.down, 1f, GameLayers.Ground, out var hit, new Color(0, 1, 1)))
            {
                topPoint = hit.point;
                return true;
            }
            return false;
        }
    }
}