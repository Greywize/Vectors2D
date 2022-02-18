using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ClickDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    Rigidbody2D rigidBody2D;
    new Collider2D collider2D;

    InputAction mouseInput;
    Vector2 mousePosition;
    Vector2 grabOrientation;
    Vector2 grabOffset;
    float distance;
    float torque;
    float sin;

    private bool dragging;

    [SerializeField] [Min(0)]
    [Tooltip("The minimum distance away from the pointer the object needs to be before the pull kicks in")]
    float minDistance = 0f;
    [SerializeField] [Min(0)]
    [Tooltip("The max distance away from the pointer the object can be before the rope cannot stretch anymore")]
    float maxDistance = 15;
    [SerializeField] [Min(0)]
    float elasticity = 2.5f;
    [SerializeField] [Min(0)]
    [Tooltip("How much additional distance the object can be away from the max distance before the rope snaps")]
    float snapThreshhold = 2.5f;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();

        mouseInput = InputManager.interfaceActions.FindAction("Point");
        mouseInput.Enable();
    }

    private void FixedUpdate()
    {
        mousePosition = mouseInput.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        distance = Vector2.Distance(mousePosition, transform.position);

        if (dragging && distance > minDistance)
        {
            // The angle between our starting and current orientation
            float angle = Vector2.SignedAngle(grabOrientation, transform.up);

            // Vector from the center of the object to where we clicked on it, accounting for any additional rotation since the last frame
            Vector2 rotatedOffset = Quaternion.AngleAxis(angle, Vector3.forward) * grabOffset;
            // Vector from pull point (rotatedOffset) to mouse position
            Vector2 pullForce = (mousePosition - rotatedOffset) - (Vector2)transform.position;

            // Angle between direction from axis of rotation to force point and force vector
            sin = Vector2.SignedAngle(rotatedOffset, pullForce);
            // Calculate torque
            // T = |force| * |radius| * sin(angle between f & r)
            torque = pullForce.magnitude * rotatedOffset.magnitude * sin;
            rigidBody2D.AddTorque(torque * elasticity);

            // Calculate force - Accounts for where we clicked on the object as all velocity is applied from the object's center
            rigidBody2D.AddForce(pullForce * 100 * elasticity);

            // We're too far away, snap the rope - But only if we're not moving (i.e. stuck)
            if (distance > maxDistance + snapThreshhold && rigidBody2D.velocity.magnitude < 1)
            {
                dragging = false;
                return;
            }

            // Get the closest point to the perimeter of the maxDistance radius
            Vector3 distanceToPerimiter = pullForce.normalized * (distance - maxDistance);
            // If we've reached the max distance allowed
            if (distance > maxDistance)
            {
                // Maintain velocity of correction movement
                rigidBody2D.AddForce(pullForce * 100 * distanceToPerimiter.magnitude);

                // Find any velocity heading away from direction of pull
                float dot = Vector2.Dot(rigidBody2D.velocity, -pullForce.normalized);
                Vector2 component = -pullForce.normalized * dot;
                // Remove any velocity heading away from the direciton of pull
                if (dot > 0)
                    rigidBody2D.AddForce(-component * 100);
            }

            Debug.DrawLine(rotatedOffset + (Vector2)transform.position, mousePosition);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        // Orientation of the object at time of pointer down
        grabOrientation = transform.up;
        // Vector difference between where we grabbed the object and where it was at the time
        grabOffset = mousePosition - (Vector2)transform.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}