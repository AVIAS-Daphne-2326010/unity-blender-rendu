using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace BUT
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        Movement m_Movement;

        float m_CurrentSpeed;
        public float CurrentSpeed
        {
            set
            {
                if (m_CurrentSpeed == value) return;
                m_CurrentSpeed = value;
                OnSpeedChange?.Invoke(m_CurrentSpeed);
            }
            get => m_CurrentSpeed;
        }

        bool m_IsSprinting;
        public bool IsSprinting { set => m_IsSprinting = value; get => m_IsSprinting; }

        private bool m_IsMoving;
        public bool IsMoving
        {
            set
            {
                if (m_IsMoving == value) return;
                m_IsMoving = value;
                OnMovingChange?.Invoke(m_IsMoving);
            }
            get => m_IsMoving;
        }

        private Vector3 m_Direction;
        public Vector3 Direction { set => m_Direction = value; get => m_Direction; }
        public Vector3 FullDirection
{
    get
    {
        // direction horizontale
        Vector3 horizontalMove = Direction * CurrentSpeed;
        // ajouter la gravité séparément
        Vector3 verticalMove = Vector3.up * GravityVelocity;
        return horizontalMove + verticalMove;
    }
}

        private Quaternion m_GroundRotationOffset;
        public Quaternion GroundRotationOffset { set => m_GroundRotationOffset = value; get => m_GroundRotationOffset; }

        public const float GRAVITY = -9.31f;

        private float m_GravityVelocity;
        public float GravityVelocity { set => m_GravityVelocity = value; get => m_GravityVelocity; }

        private int m_JumpNumber;
        public int JumpNumber { set => m_JumpNumber = value; get => m_JumpNumber; }

        [SerializeField]
        float m_RayLenght;
        [SerializeField]
        LayerMask m_RayMask;

        RaycastHit m_Hit;

        private bool m_IsGrounded;
        public bool IsGrounded
        {
            set
            {
                if (IsGrounded == value) return;
                m_IsGrounded = value;
                OnGroundedChange?.Invoke(m_IsGrounded);
            }
            get => m_IsGrounded;
        }

        private CharacterController m_CharacterController;
        private Vector2 m_MovementInput;
        private Vector3 m_MovementDirection;

        public UnityEvent<float> OnSpeedChange;
        public UnityEvent<bool> OnMovingChange;
        public UnityEvent<bool> OnGroundedChange;

        private void Awake()
        {
            m_CharacterController = GetComponent<CharacterController>();
        }

        public void MovingChanged(bool _moving)
        {
            OnMovingChange?.Invoke(_moving);
        }

        public void SpeedChanged(float _speed)
        {
            OnSpeedChange?.Invoke(_speed);
        }

        public void GroundedChanged(bool _grounded)
        {
            OnGroundedChange?.Invoke(_grounded);
        }

        private void OnDisable()
        {
            IsMoving = false;
            m_MovementInput = Vector2.zero;
        }

        private void OnEnable()
        {
            if (!Application.isPlaying) return;
            m_MovementInput = Vector2.zero;
        }

        private void Update()
        {
            // Gestion du mouvement
            if (m_MovementInput.magnitude > 0.1f)
            {
                if (!IsMoving)
                {
                    IsMoving = true;
                }
            }
            else if (IsMoving)
            {
                IsMoving = false;
            }

            ManageDirection();
            ManageGravity();

            if (IsMoving)
            {
                ApplyRotation();
                ApplyMovement();
            }
            else
            {
                // Même sans mouvement, applique la gravité
                ApplyMovement();
            }
        }

        public void SetInputMove(InputAction.CallbackContext _context)
        {
            m_MovementInput = _context.ReadValue<Vector2>();
            Debug.Log("Input: " + m_MovementInput);
        }

        public void SetInputJump(InputAction.CallbackContext _context)
        {
            if (!_context.started || (!m_CharacterController.isGrounded && JumpNumber >= m_Movement.MaxJumpNumber)) return;
            if (JumpNumber == 0) StartCoroutine(WaitForLanding());
            JumpNumber++;

            if (m_Movement.MinimazeJumpPower) GravityVelocity += m_Movement.JumpPower / JumpNumber;
            else GravityVelocity += m_Movement.JumpPower;
        }

        IEnumerator WaitForLanding()
        {
            yield return new WaitUntil(() => !m_CharacterController.isGrounded);
            yield return new WaitUntil(() => m_CharacterController.isGrounded);
            JumpNumber = 0;
        }

        public void SetInputSprint(InputAction.CallbackContext _context)
        {
            IsSprinting = _context.started || _context.performed;
        }

        [SerializeField] float m_SpeedMultiplier = 4f;
        private void ManageDirection()
        {
            // set direction
            m_MovementDirection = new Vector3(m_MovementInput.x, 0, m_MovementInput.y);

            // modify direction according to camera view
            m_MovementDirection = Camera.main.transform.TransformDirection(m_MovementDirection);
            m_MovementDirection.y = 0;
            Debug.DrawRay(transform.position, -transform.up * m_RayLenght, Color.red);

            m_MovementDirection.Normalize();

            Direction = m_MovementDirection;
            Debug.DrawRay(transform.position, Direction, Color.red);

            // calculate speed according to input force
            float inputMagnitude = (m_MovementInput.magnitude > 0.1f) ? 1f : 0f;
            CurrentSpeed = ((IsSprinting) ? m_Movement.SprintFactor : 1f)
                           * m_Movement.MaxSpeed
                           * m_Movement.SpeedFactor.Evaluate(inputMagnitude)
                           * m_SpeedMultiplier;

            Debug.Log("CurrentSpeed = " + CurrentSpeed);

        }

        public void ApplyRotation()
        {
            if (!IsMoving) return;

            // calculate target rotation
            Quaternion targetRotation = Quaternion.LookRotation(Direction, transform.up);
            // lerp toward the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation,
                m_Movement.MaxAngularSpeed * Mathf.Deg2Rad * m_Movement.AngularSpeedFactor.Evaluate(Direction.magnitude) * Time.deltaTime);
        }

        public void ApplyMovement()
        {
            Vector3 move = FullDirection * Time.deltaTime;
            m_CharacterController.Move(move);

            // debug
            Debug.DrawRay(transform.position, move, Color.yellow);
        }

        private void ManageGravity()
        {
            if (m_CharacterController.isGrounded && GravityVelocity < 0.0f)
            {
                // if grounded set back gravity velocity to a normal number
                GravityVelocity = -1;
            }
            else
            {
                // if not grounded add gravity
                GravityVelocity += GRAVITY * m_Movement.GravityMultiplier * Time.deltaTime;
            }
        }
    }
}