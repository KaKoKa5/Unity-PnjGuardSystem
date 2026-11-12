using System;
using System.Collections.Generic;
using CharacterController.SaverioScript.PlayerControllers.CharacterController;
using CharacterController.SaverioScript.PlayerControllers.CharacterController.MoveState;
using CharacterController.Script.PlayerControllers;
using CharacterController.Script.PlayerControllers.Utilities;
using UnityEngine;

namespace CharacterController.Script
{
    public class CharacterSetup : MonoBehaviour
    {
        [field: SerializeField] public CharacterData CharacterData { get; private set; }
        [field: SerializeField] public ReadInput ReadInput { get; private set; }
        [field: SerializeField] public MovementBase MovementBase { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [field: SerializeField] public bool Grounded { get; private set; }
        [field: SerializeField] public bool Climb { get; private set; }
        [SerializeField] private Camera cam;

        public IMoveState MoveState { get; private set; }
        public event Action<IMoveState> OnStateChanged;

        private readonly List<IAptitude> aptitudes = new();
        public IReadOnlyList<IAptitude> Aptitudes => aptitudes;

        private void Reset()
        {
            Animator = GetComponentInChildren<Animator>();
            cam = Camera.main;
        }

        private void Awake()
        {
            if (cam == null) cam = Camera.main;
            GetComponentsInChildren(true, aptitudes);
            foreach (var aptitude in aptitudes)
                aptitude.Initialize(this);
        }

        private void Start()
        {
            GoTo(new NormalWalk());
        }

        private void OnEnable()
        {
            ReadInput.OnAttackPressed += HandleAttackPressed;
        }

        private void OnDisable()
        {
            ReadInput.OnAttackPressed -= HandleAttackPressed;
        }

        private void Update()
        {
            var cc = MovementBase.controller;
            var origin = cc.transform.position + Vector3.up * 0.1f; 
            var rayHit = RaycastChecker.Check(origin, Vector3.down, 0.4f, GameLayers.Ground,
                out _, new Color(0, 1, 0));
            Grounded = cc.isGrounded || rayHit;
            Climb = RaycastChecker.Check(origin, transform.forward, 1f, GameLayers.Climb,
                out var hit, new Color(1, 0, 0));
            
            MoveState.UpdateState(this);
            
            if (MoveState is VaultState or ClimbState or LedgeClimbState) return;
            
            MovementBase.HandleRotation(CharacterData.rotationSpeed, cam);
            MovementBase.Jump(this);
            MovementBase.HandleMovement(this, cam);

            if (MovementBase.CurrentMoveDirection.magnitude > CharacterData.vaultAutoSpeedThreshold)
                Vaulting.TryVault(this, MovementBase.CurrentMoveDirection.magnitude);
        }

        public T GetAptitude<T>() where T : class, IAptitude
        {
            foreach (var aptitude in aptitudes)
                if (aptitude is T found)
                    return found;
            return null;
        }

        private void HandleAttackPressed()
        {
            if (MovementBase.controller.isGrounded)
                GoTo(new AttackState(CharacterData.firstAttack));
        }

        public void GoTo(IMoveState state)
        {
            MoveState?.ExitState(this);
            MoveState = state;
            MoveState?.EnterState(this);
            OnStateChanged?.Invoke(state);
        }
    }
}