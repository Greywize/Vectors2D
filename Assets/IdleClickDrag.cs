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
    [SerializeField]
    Vector2 mousePosition;
    Vector2 grabPosition;
    [SerializeField]
    Vector2 pivotOffset;
    [SerializeField]
    Vector2 adjustedPoint;
    float distance;
    Vector2 grabOrientation;

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
    Vector2 f;
    [SerializeField]
    Vector2 r;
    [SerializeField]
    float a;
    [SerializeField]
    float angle;

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
            angle = Vector2.SignedAngle(grabOrientation, transform.up);

            Vector2 offset = (grabPosition - (Vector2)transform.position);
            Debug.DrawLine(transform.position, (Vector2)transform.position + offset);
            offset = Quaternion.AngleAxis(angle, Vector3.forward) * offset;
            Debug.DrawLine(transform.position, (Vector2)transform.position + offset, Color.cyan);

            /*Vector2 dragDirection = adjustedPoint + mousePosition;
            rigidBody.velocity += dragDirection * elasticity;*/

            // T = f * r * sin(a)
            // f - dragDirection
            // r - Distance vector between adjustedGrabPoint & position
            // a - Angle between r & f

            /*f = dragDirection;
            r = adjustedPoint - (Vector2)transform.position;
            a = Mathf.Sin(Vector2.Angle(r, f));
            torque = r.magnitude * f.magnitude * a;

            rigidBody.AddTorque(torque);*/

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
        grabPosition = mousePosition;
        // Orientation of the object at time of pointer down
        grabOrientation = transform.up;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}