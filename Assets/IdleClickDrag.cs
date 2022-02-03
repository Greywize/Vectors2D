using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class IdleClickDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IInitializePotentialDragHandler
{
    Rigidbody2D rigidBody;
    RectTransform rectTransform;

    InputAction mouseInput;
    Vector2 mousePosition;
    Vector2 pivotOffset;
    Vector2 grabPoint;
    float distance;
    Vector2 upDirectionOnGrab;

    [SerializeField] [Min(0)]
    [Tooltip("The minimum distance away from the pointer the object needs to be before elasticity kicks in")]
    float minDistance = 2.5f;
    [SerializeField] [Min(0)]
    [Tooltip("The max distance away from the pointer the object can be before it hits a hard limit")]
    float maxDistance = 5;
    [SerializeField] [Min(0)]
    float elasticity;

    private bool dragging;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
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
            grabPoint = (Vector2)transform.position + pivotOffset;
            float angle = Vector2.Angle(upDirectionOnGrab, transform.up) * Mathf.Deg2Rad;

            Vector2 adjustedGrabPoint = grabPoint - (Vector2)transform.position;
            adjustedGrabPoint = (Vector2)transform.position + new Vector2(adjustedGrabPoint.x * Mathf.Cos(angle) - adjustedGrabPoint.y * Mathf.Sin(angle),
                                            adjustedGrabPoint.x * Mathf.Sin(angle) + adjustedGrabPoint.y * Mathf.Cos(angle));

            Vector2 dragDirection = mousePosition - adjustedGrabPoint;
            rigidBody.velocity += dragDirection * elasticity;

            // Get the closest point to the perimeter of the maxDistance radius
            Vector3 distanceToPerimiter = dragDirection.normalized * (distance - maxDistance);
            if (distance > maxDistance)
            {
                // Correct our position by adding distanceToPerimiter to our position
                transform.position += distanceToPerimiter;
                // Maintain velocity of correction movement
                rigidBody.velocity += (Vector2)dragDirection * distanceToPerimiter.magnitude;

                float dot = Vector2.Dot(rigidBody.velocity, -dragDirection.normalized);
                Vector2 component = -dragDirection.normalized * dot;
                if (dot > 0)
                    rigidBody.velocity -= component;
            }

            Vector2 r = adjustedGrabPoint - (Vector2)transform.position;

            float torque = r.magnitude * dragDirection.magnitude * Mathf.Sin(r.magnitude);

            Debug.DrawLine(adjustedGrabPoint, mousePosition, Color.white);

            rigidBody.AddTorque(torque);
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;

        pivotOffset = mousePosition - (Vector2)transform.position;
        upDirectionOnGrab = transform.up;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}