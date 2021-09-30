using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Mode mode;
    public enum Mode
    {
        // Player following / spectating
        Follow,
        // Free flying
        Free,
        // No control allowed
        Frozen,

        // The three Fs. Can we get an F in chat?!
    }

    // --- Set this to private eventually - only public for now while we develop
    public Transform target;
    public float followSpeed = 2.5f;
    public bool instant = false;

    // Super simple camera follow - I actually did the first part of this drunk lol
    private void Update()
    {
        // Don't follow if we don't have a target or if we're frozen
        if (!target || mode == Mode.Frozen)
            return;

        // Instant follow for spectating mode
        if (instant)
            transform.position = target.position;
        else
        {
            // Lerp to postiion
            transform.position = Vector2.Lerp(transform.position, target.position, followSpeed * Time.deltaTime);

            // Snap to target position when we're only a tiny distance away
            if (Vector2.Distance(transform.position, target.position) < 0.01f)
                transform.position = target.position;
        }
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }
    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void SetMode(Mode mode)
    {
        this.mode = mode;
    }
}