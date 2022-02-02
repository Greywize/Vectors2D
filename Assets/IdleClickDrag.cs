using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class IdleClickDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IInitializePotentialDragHandler
{
    Rigidbody2D rigidBody;

    InputAction mouseInput;
    Vector2 mousePosition;
    float distance;

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

        mouseInput = InputManager.interfaceActions.FindAction("Point");
        mouseInput.Enable();
    }

    private void FixedUpdate()
    {
        if (!dragging)
            return;

        mousePosition = mouseInput.ReadValue<Vector2>();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        distance = Vector2.Distance(mousePosition, transform.position);
        if (distance > minDistance)
        {
            Vector3 dragDirection = new Vector2(mousePosition.x - transform.position.x,
                                                mousePosition.y - transform.position.y);

            rigidBody.velocity += (Vector2)dragDirection * elasticity;

            if (distance > maxDistance)
            {
                // Get the closest point to the perimeter of the maxDistance radius
                Vector3 distanceToPerimiter = dragDirection.normalized * (distance - maxDistance);
                // Correct our position by adding distanceToPerimiter to our position
                transform.position += distanceToPerimiter;
                // Maintain velocity of correction movement
                rigidBody.velocity += (Vector2)dragDirection * distanceToPerimiter.magnitude;

                float dot = Vector2.Dot(rigidBody.velocity, -dragDirection.normalized);
                Vector2 component = -dragDirection.normalized * dot;
                if (dot > 0)
                    rigidBody.velocity -= component;
            }
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}