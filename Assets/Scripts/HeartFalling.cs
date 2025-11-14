using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartFalling : MonoBehaviour
{
    public float fallSpeed = 200f;
    private bool shouldFall = false;

    void Update()
    {
        if (shouldFall)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // delete when fully off-screen
            if (transform.localPosition.y < -300f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void DropHeart()
    {
        shouldFall = true;

        // detach from layout group so it can fall freely
        transform.SetParent(transform.parent.parent);
    }
}
