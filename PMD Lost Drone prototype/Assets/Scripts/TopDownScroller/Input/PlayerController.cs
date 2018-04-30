using System.Collections;
using System.Collections.Generic;
using Core.Health;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}


public class PlayerController : DamageableBehaviour {

    public float speed;
    public Boundary boundary;

    private Rigidbody2D rb;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical);
        rb.velocity = movement * speed;

        rb.position = new Vector3
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
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
