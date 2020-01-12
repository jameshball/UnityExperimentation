using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;

        if (collider.name == "Player")
        {
            collider.transform.SetParent(transform.parent.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        GameObject collider = collision.collider.gameObject;

        if (collider.name == "Player")
        {
            collider.transform.SetParent(null);
        }
    }
}
