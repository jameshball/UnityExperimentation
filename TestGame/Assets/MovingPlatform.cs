using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Vector3 destination;
    public float moveTime;
    public float waitTime;

    private Vector3 velocity = Vector3.zero;
    private Vector3 finalDestination;
    private float timeOfLastMovement;
    private bool movingForward;

    void Start()
    {
        finalDestination = transform.position + destination;
        movingForward = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, finalDestination) > 0.05)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalDestination, ref velocity, moveTime);
            timeOfLastMovement = Time.time;
        }
        else if (Time.time - timeOfLastMovement > waitTime)
        {
            finalDestination = movingForward ? transform.position - destination : transform.position + destination;
            movingForward = !movingForward;
        }
    }
}
