using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

namespace MatchMade
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(NetworkIdentity))]
    [RequireComponent(typeof(PlayerInput))]
    public class Player : NetworkBehaviour
    {
        public static Player LocalPlayer;
        [SerializeField] bool isLocal;
        [SerializeField] uint networkId;

        Rigidbody2D rigidBody;

        public float moveSpeed;
        public float turnSpeed;
        public float angle;
        public float velocity;

        // --- Controls
        private PlayerInput playerInput;
        private InputActionMap controls;
        private InputAction movementInput;

        public Vector2 inputDirection;
        public Vector2 movementDirection;

        public override void OnStartAuthority()
        {
            // Cache a reference and enable the PlayerInput component before anything else
            // Clients don't register input using the new input system if we don't do this for some reason
            playerInput = GetComponent<PlayerInput>();
            playerInput.enabled = true;
        }
        private void Awake()
        {
            // Set up components
            rigidBody = GetComponent<Rigidbody2D>();
        }
        private void Start()
        {
            // Return if this is not the local player
            if (!isLocalPlayer)
                return;

            LocalPlayer = this;

            GetComponent<SpriteRenderer>().color = Color.white;

            SetupControls();
        }
        private void FixedUpdate()
        {
            // Return if we've been disconnected or this is not our local player
            if (!NetworkClient.isConnected || !isLocalPlayer)
                return;

            // Get movement vector from input
            inputDirection = movementInput.ReadValue<Vector2>();

            // If we have input, request movement from the server
            if (inputDirection.magnitude > 0)
            {
                CmdMove();
            }
        }
        [Command]
        public void CmdMove()
        {
            // Run movement Target Rpc for the client requesting movement
            TargetMove(connectionToClient);
        }
        [TargetRpc]
        public void TargetMove(NetworkConnection conn)
       {
            if (movementDirection.magnitude == 0)
                movementDirection = transform.up;
            // Debug line to indicate input direction
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + inputDirection.x, transform.position.y + inputDirection.y), new Color(0, 0, 1));

            // Get angle between input and current direction
            angle = Vector2.Angle(movementDirection, inputDirection);
            velocity = rigidBody.velocity.magnitude;

            // RotateTowards doesn't work with 180 angles
            if (angle == 180)
                movementDirection = Quaternion.Euler(0, 0, 1) * movementDirection;

            // Rotate movement vector towards target direction, which is our input vector
            movementDirection = Rotate(movementDirection, inputDirection, turnSpeed);
            // Normalize direction
            movementDirection.Normalize();

            // Debug line to indicate current trajectory 
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + movementDirection.x, transform.position.y + movementDirection.y), new Color(1, 0, 0));

            // Rotate player sprite
            transform.up = movementDirection;
            // Move player
            rigidBody.AddForce(movementDirection * moveSpeed, ForceMode2D.Force);
        }
        // Rotate function
        private Vector2 Rotate(Vector2 current, Vector2 target, float speed)
        {
            // Rotate towards target from current using speed multiplied by 360 so that 1 = one full rotation per second
            // Convert the result to radians and multiply by deltaTime;
            return Vector3.RotateTowards(current, target, (speed * 360) * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
        }
        private void SetupControls()
        {
            // --- Set up action map
            controls = GetComponent<PlayerInput>().actions.FindActionMap("Player");
            controls.Enable();
            // --- Movement
            movementInput = controls.FindAction("Move");
            movementInput.Enable();
        }
    }
}