using PlayerSystem;
using PlayerSystem.Manager;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace PlayerSystem.Controller
{
    public class PlayerController : MonoBehaviour
    {
        // Singleton
        public static PlayerController Instance { get; private set; }

        [Header("References")]
        private Rigidbody body;
        [SerializeField] private Transform spawn;
        [SerializeField] private Transform cameraPosition;
        [SerializeField] private Transform barrel;
        [SerializeField] private GameObject bullet;
        [SerializeField] private LayerMask groundLayer;

        [Header("Inputs")]
        [SerializeField] private InputAction look;
        [SerializeField] private InputAction move;
        [SerializeField] private InputAction jump;
        [SerializeField] private InputAction pressClick;
        [SerializeField] private InputAction releaseClick;

        [Header("Stats")]
        [SerializeField] private float sensitivity = 1;
        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float jumpForce = 10f;
        [SerializeField] private float groundTolerance = 1.5f;
        private float xRotation = 0;
        private float yRotation = 0;
        private float distanceToGround;
        private float colliderRadius = 0.5f;
        private bool isGrounded;
        private Vector2 moveDirection;
        private Vector3 barrelPos, movement;
        private Quaternion barrelRot;

        [HideInInspector] public bool activate = false;

        private void Awake()
        {
            // Keep only one instance of this script
            if (Instance != null && Instance != this)
            {
                GameObject.Destroy(this.gameObject);
            }
            Instance = this;
        }

        private void Start()
        {
            // Retrieve References
            body = GetComponent<Rigidbody>();

            // Enable Input Actions
            look.Enable();
            move.Enable();
            jump.Enable();
            pressClick.Enable();
            releaseClick.Enable();

            LockCursor();
        }

        private void Update()
        {
            Movement();
            Shooting();
            FallRestart();
        }
        private void FixedUpdate()
        {
            // Physics Movement
            body.AddForce(movement * Time.deltaTime, ForceMode.VelocityChange);
        }

        private void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Movement()
        {
            GroundCheck();

            // Read move input and place it in a variable for vector force
            moveDirection = move.ReadValue<Vector2>();
            movement = transform.forward * speed * moveDirection.y +
                transform.right * speed * moveDirection.x;

            Camera();
            Jump();

            DebugLines();
        }

        private void Camera()
        {
            // Read mouse movement
            Vector2 mouseDelta = look.ReadValue<Vector2>();

            // Rotate off of mouse delta value
            xRotation -= mouseDelta.y * sensitivity;
            yRotation += mouseDelta.x * sensitivity;
            // Clamp vertical rotation
            xRotation = Mathf.Clamp(xRotation, -90, 90);

            // Move and rotate camera
            cameraPosition.position = transform.position;
            cameraPosition.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            transform.rotation = Quaternion.Euler(0, yRotation, 0);
        }

        private void Jump()
        {
            // If space pressed and within ground tolerance, jump
            if (jump.triggered && isGrounded)
            {
                body.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            }
        }

        private void GroundCheck()
        {
            // Create sphere cast
            bool isGroundBelow = Physics.SphereCast(
                transform.position,
                colliderRadius,
                Vector3.down,
                out RaycastHit hitinfo,
                groundLayer);

            // If ground is within the distance threshold, can jump
            if (isGroundBelow)
            {
                distanceToGround = transform.position.y - hitinfo.point.y;
            }
            else
            {
                distanceToGround = 0;
            }

            isGrounded = isGroundBelow && distanceToGround <= groundTolerance;
        }

        private void Shooting()
        {
            // Store Barrel position and rotation
            barrelPos = barrel.position;
            barrelRot = barrel.rotation;

            // On mouse click instantiate bullet prefab
            if (pressClick.triggered)
            {
                activate = false;
                Instantiate(bullet, barrelPos, barrelRot);
            }
            // On mouse release activate bullet platform
            else if (releaseClick.triggered)
            {
                activate = true;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(new Vector3(
                transform.position.x, 
                transform.position.y - 1, 
                transform.position.z),
                colliderRadius);
        }

        private void DebugLines()
        {
            Debug.Log(moveDirection.x + " " + moveDirection.y);
        }

        private void FallRestart()
        {
            if (transform.position.y < -9.5)
            {
                transform.position = spawn.position;
            }
        }
    }
}


