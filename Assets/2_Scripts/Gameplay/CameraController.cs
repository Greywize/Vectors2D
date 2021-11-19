using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MatchMade;

public class CameraController : MonoBehaviour
{
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

    MatchMade.Player locaPlayer;

    Camera cam;
    Transform target;

    Vector3 targetPosition;
    Vector3 offset = new Vector3(0,0,10);

    Mode mode;

    public float followSpeed = 2.5f;
    public bool instant = false;

    private void Start()
    {
        locaPlayer = GetComponent<MatchMade.Player>();
        if (!locaPlayer.isLocalPlayer)
            return;

        GetMainCamera();
        SetTarget(locaPlayer.transform);

        SetMode(Mode.Follow);
    }
    // Super simple camera follow - I actually did the first part of this drunk lol
    private void FixedUpdate()
    {
        if (!locaPlayer.isLocalPlayer || !NetworkManager.Instance.isNetworkActive)
            return;

        if (!cam)
        {
            Debug.Log($"{gameObject} could not find camera.");
            return;
        }

        // Don't follow if we don't have a target or if we're frozen
        if (!target || mode == Mode.Frozen)
            return;

        // Smooth follow
        if (!instant)
        {
            // Lerp to postiion
            targetPosition = Vector2.Lerp(cam.transform.position, target.position, followSpeed * Time.deltaTime);

            // Snap to target position when we're only a tiny distance away
            if (Vector2.Distance(targetPosition, target.position) < 0.01f)
                targetPosition = target.position;
        }
        // Instant follow
        else
        {
            targetPosition = target.position;
        }

        cam.transform.position = targetPosition - offset;
    }
    public void GetMainCamera()
    {
        cam = Camera.main;
    }
    public void SetPosition(Vector2 position)
    {
        cam.transform.position = position;
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