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

        public string PlayerName { get; set; }
        public int Score { get; set; }
        public bool CanControl { get; set; }

        [SerializeField] float moveSpeed;
        [SerializeField] float turnSpeed;
        float angle;
        float velocity;

        // --- Controls
        PlayerInput playerInput;
        InputActionMap controls;
        InputAction movementInput;

        // Components
        Rigidbody2D rigidBody;
        SpriteRenderer spriteRenderer;

        Vector2 inputDirection;
        Vector2 movementDirection;

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

            spriteRenderer = GetComponent<SpriteRenderer>(); 
        }
        private void Start()
        {
            // Return if this is not the local player
            if (!isLocalPlayer)
                return;

            LocalPlayer = this;

            // Local player needs to send the server its own name
            // ToDo - This ^
            PlayerName = NetworkManager.Instance.localPlayerName;

            if (spriteRenderer)
                spriteRenderer.color = Color.white;

            // In case it wasn't already
            CanControl = true;

            SetupControls();
            Chat.Instance.UnfocusChat();
        }
        private void FixedUpdate()
        {
            // Return if we've been disconnected, if this is not our local player, or if we're not allowed control
            if (!NetworkClient.isConnected || !isLocalPlayer || !CanControl)
                return;

            // Get movement vector from input
            inputDirection = movementInput.ReadValue<Vector2>();

            // If we have input, request movement from the server
            if (inputDirection.magnitude > 0)
            {
                CmdMove();
            }
        }

        #region Commands
        [Command]
        public void CmdMove()
        {
            // Run movement Target Rpc for the client requesting movement
            TargetMove(connectionToClient);
        }
        [Command]
        public void CmdSendChatMessage(string senderName, string message)
        {
            Chat.Instance.RpcRecieveMessage(senderName, message);
        }
        #endregion

        #region RPCs
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
        #endregion

        // Rotate function
        private Vector2 Rotate(Vector2 current, Vector2 target, float speed)
        {
            // Convert the result to radians and multiply by deltaTime;
            return Vector3.RotateTowards(current, target, speed * Mathf.Deg2Rad, 0.0f);
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