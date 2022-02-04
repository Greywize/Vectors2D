using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class IdleClickDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IInitializePotentialDragHandler
{
    Rigidbody2D rigidBody2D;
    RectTransform rectTransform;

    InputAction mouseInput;
    Vector2 mousePosition;
    Vector2 globalGrabPoint;
    Vector2 grabOrientation; 
    float distance;

    [SerializeField] [Min(0)]
    [Tooltip("The minimum distance away from the pointer the object needs to be before elasticity kicks in")]
    float minDistance = 2.5f;
    [SerializeField] [Min(0)]
    [Tooltip("The max distance away from the pointer the object can be before it hits a hard limit")]
    float maxDistance = 5;
    [SerializeField] [Min(0)]
    float elasticity;

    [SerializeField]
    float torque;
    [SerializeField]
    Vector2 pullDirection;
    [SerializeField]
    Vector2 localGrabPoint;
    [SerializeField]
    float sin;

    private bool dragging;

    private void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rectTransform = GetComponent<RectTransform>();

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

            // Offset the grab position back to 0,0
            localGrabPoint = globalGrabPoint - (Vector2)transform.position;
            // Rotate offset by how much our object has rotated since the last frame
            localGrabPoint = (Quaternion.AngleAxis(angle, Vector3.forward) * localGrabPoint);
            // Calculate force from grab point to mouse position, accounting for our offset being at 0,0
            pullDirection = localGrabPoint + (mousePosition - (Vector2)transform.position);

            // Sin of the distance between axis point and force point
            sin = Vector2.SignedAngle(localGrabPoint, pullDirection);

            // Calculate torque
            // T = |force| * |radius| * sin(angle between f & r)
            torque = pullDirection.magnitude * localGrabPoint.magnitude * sin;
            rigidBody2D.AddTorque(torque * elasticity);

            // Calculate force
            Vector2 pullForce = mousePosition - (Vector2)transform.position;
            rigidBody2D.velocity += pullForce * elasticity;

            /*// Get the closest point to the perimeter of the maxDistance radius
            Vector3 distanceToPerimiter = dragDirection.normalized * (distance - maxDistance);
            if (distance > maxDistance)
            {
                // Correct our position by adding distanceToPerimiter to our position
                transform.position += distanceToPerimiter;
                // Maintain velocity of correction movement
                rigidBody.velocity += dragDirection * distanceToPerimiter.magnitude;

                float dot = Vector2.Dot(rigidBody.velocity, -dragDirection.normalized);
                Vector2 component = -dragDirection.normalized * dot;
                if (dot > 0)
                    rigidBody.velocity -= component;
            }*/
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        // Position of the mouse at time of pointer down
        globalGrabPoint = mousePosition;
        // Orientation of the object at time of pointer down
        grabOrientation = transform.up;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}