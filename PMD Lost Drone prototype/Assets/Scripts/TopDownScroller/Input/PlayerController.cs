using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}


public class PlayerController : MonoBehaviour {

    public float speed;
    public float tilt;
    public Boundary boundary;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0.0f);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax),
            0.0f
        );
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 topLeft = new Vector3(boundary.xMin, boundary.yMax);
        Vector3 topRight = new Vector3(boundary.xMax, boundary.yMax);
        Vector3 bottomLeft = new Vector3(boundary.xMin, boundary.yMin);
        Vector3 bottomRight = new Vector3(boundary.xMax, boundary.yMin);

        Gizmos.DrawLine(bottomLeft, topLeft);//left
        Gizmos.DrawLine(topRight, topLeft);//top
        Gizmos.DrawLine(bottomRight, topRight);//right
        Gizmos.DrawLine(bottomRight, bottomLeft);//bottom
    }
}
