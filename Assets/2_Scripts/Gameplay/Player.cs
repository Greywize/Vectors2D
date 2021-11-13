using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // --- Objects
    public Color playerColor = new Color(1, 1, 1);
    private Rigidbody2D m_rigidBody;
    public GameObject weaponPrefab;
    private Weapon weapon;

    // --- Member Variables
    [Tooltip("How many units per second the player can move")]
    public float moveSpeed;
    [Tooltip("Turn speed is x full rotations per second")]
    public float turnSpeed;
    public float aimSpeed;
    //[ReadOnly]
    public float angle;
    //[ReadOnly]
    public float velocity;
    public bool instantAim = false;
    public bool canControl = true;

    // ---Vectors
    //[ReadOnly]
    public Vector2 m_inputDirection;
    public Vector2 m_movementDirection;
    //[ReadOnly]
    public Vector2 m_aimDirection;

    // List of projectiles we shoot to keep track of
    public Dictionary<GameObject, float> projectiles = new Dictionary<GameObject, float>();

    // --- Controls
    private InputActionMap m_controls;

    private InputAction m_movementInput;
    private InputAction m_mousePositionInput;
    private InputAction m_shootInput;
    
    // Subscribe to delegates here
    private void OnEnable()
    {
        
    }
    // Unsubscribe to delegates here
    private void OnDisable()
    {
        
    }

    // Unity functions
    private void Awake()
    {
        SetupControls();

        // --- Set up objects
        m_rigidBody = GetComponent<Rigidbody2D>();

        SetPlayerStartingRotation();
    }
    private void Start()
    {
        SetWeapon();
        SetColor(playerColor);
    }
    private void Update()
    {
        TimeOutProjectiles();
    }
    private void FixedUpdate()
    {
        if (!canControl)
            return;

        Move();
        Aim();
    }

    // Player control functions
    private void Move()
    {
        // Get movement vector from input
        m_inputDirection = m_movementInput.ReadValue<Vector2>();
        // Debug line to indicate input direction
        Debug.DrawLine(transform.position, new Vector2(transform.position.x + m_inputDirection.x, transform.position.y + m_inputDirection.y), new Color(0, 0, 1));

        // Get angle between input and current direction
        angle = Vector2.Angle(m_movementDirection, m_inputDirection);
        velocity = m_rigidBody.velocity.magnitude;

        // Rotate towards the direction we want our player to head in if we have input
        if (m_inputDirection.magnitude > 0)
        {
            // RotateTowards doesn't work with 180 angles
            if (angle == 180)
                m_movementDirection = Quaternion.Euler(0, 0, 1) * m_movementDirection;

            // Multiply turn speed by 360 so that 1 = one full rotation per second
            m_movementDirection = Rotate(m_movementDirection, m_inputDirection, turnSpeed);
            // Normalize m_direction
            m_movementDirection.Normalize();

            // Debug line to indicate current trajectory 
            Debug.DrawLine(transform.position, new Vector2(transform.position.x + m_movementDirection.x, transform.position.y + m_movementDirection.y), new Color(1, 0, 0));

            // Rotate player sprite
            transform.up = m_movementDirection;
            // Move player
            m_rigidBody.AddForce(m_movementDirection * moveSpeed, ForceMode2D.Force);
        }
    }
    private void Aim()
    {
        if (!weaponPrefab)
            return;

        // Get mouse position
        Vector2 mousePosition = m_mousePositionInput.ReadValue<Vector2>();
        // Get aiming direction vector
        m_aimDirection = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        m_aimDirection.Normalize();

        if (instantAim)
            weaponPrefab.transform.up = m_aimDirection;
        else
            weaponPrefab.transform.up = Rotate(weaponPrefab.transform.up, m_aimDirection, aimSpeed);

        if (m_shootInput.ReadValue<float>() > 0)
        {
            Shoot();
        }
    }
    private void Shoot()
    {
        // If we don't have a weapon, ain't gon' be no shooty tooty
        if (!weaponPrefab)
            return;

        if (!weaponPrefab.GetComponent<Weapon>().onCooldown)
        {
            weaponPrefab.GetComponent<Weapon>().Shoot(weaponPrefab.transform.up, transform.position);
            StartCoroutine(weapon.StartCooldown());
        }
    }
    private void SetWeapon()
    {
        if (!weaponPrefab)
            return;

        weaponPrefab = Instantiate(weaponPrefab, transform);
        weapon = weaponPrefab.GetComponent<Weapon>();
        weapon.color = playerColor;
    }
    private void SetColor(Color color)
    {
        GetComponent<SpriteRenderer>().color = color;

        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.color = color;
        }
        foreach (TrailRenderer renderer in GetComponentsInChildren<TrailRenderer>())
        {
            renderer.startColor= color;
            renderer.endColor = color;
        }
    }
    private void TimeOutProjectiles()
    {
        List<GameObject> temp = new List<GameObject>();

        foreach (GameObject projectile in projectiles.Keys)
        {
            temp.Add(projectile);
        }

        foreach (GameObject projectile in temp)
        {
            projectiles[projectile] -= Time.deltaTime;

            if (projectiles[projectile] <= 0)
            {
                projectiles.Remove(projectile);
                Destroy(projectile);
            }
        }
    }

    // Set up fucntions
    private void SetPlayerStartingRotation()
    {
        // Start facing upwards
        if (m_rigidBody)
            m_movementDirection = Vector2.up;
    }
    private void SetupControls()
    {
        // --- Set up action map
        m_controls = GetComponent<PlayerInput>().actions.FindActionMap("Player");
        m_controls.Enable();

        // --- Movement
        m_movementInput = m_controls.FindAction("Move");
        m_movementInput.Enable();
        // --- Aiming
        m_mousePositionInput = m_controls.FindAction("Aim");
        m_mousePositionInput.Enable();
        // --- Shooting
        m_shootInput = m_controls.FindAction("Shoot");
        m_shootInput.Enable();
    }

    // Math functions
    private Vector2 Rotate(Vector2 current, Vector2 target, float speed)
    {
        return Vector3.RotateTowards(current, target, (speed * 360) * Mathf.Deg2Rad * Time.deltaTime, 0.0f);
    }
}