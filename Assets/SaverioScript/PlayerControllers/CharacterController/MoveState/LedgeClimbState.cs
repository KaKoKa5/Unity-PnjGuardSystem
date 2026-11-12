using UnityEngine;

namespace CharacterController.Script.PlayerControllers
{
    public class LedgeClimbState : IMoveState
    {
        private const float Duration = 0.8f;
        private static readonly int LedgeHash = Animator.StringToHash("ledge");

        public float MoveSpeed { get; set; }

        private readonly Vector3 target;
        private Vector3 start;
        private Vector3 mid;
        private float time;

        public LedgeClimbState(Vector3 topPoint) => target = topPoint;

        public void EnterState(CharacterSetup s)
        {
            Debug.Log("Entering LedgeClimbState");
            start = s.MovementBase.controller.transform.position;
            mid = new Vector3(start.x, target.y, start.z);   
            s.MovementBase.ResetMotion();
            s.Animator.SetTrigger(LedgeHash);
        }

        public void UpdateState(CharacterSetup s)
        {
            time += Time.deltaTime / Duration;
            var t = Mathf.Clamp01(time);

            var desired = t < 0.5f
                ? Vector3.Lerp(start, mid, Mathf.SmoothStep(0f, 1f, t * 2f))
                : Vector3.Lerp(mid, target, Mathf.SmoothStep(0f, 1f, (t - 0.5f) * 2f));

            var cc = s.MovementBase.controller;
            cc.Move(desired - cc.transform.position);

            if (time >= 1f)
                s.GoTo(new NormalWalk());
        }

        public void ExitState(CharacterSetup s) { }
    }
}