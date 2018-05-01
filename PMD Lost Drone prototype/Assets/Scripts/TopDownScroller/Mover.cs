using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveDirection { Down, Top, Right, Left, TopLeft, TopRight, DownLeft, DownRight };

public class Mover : MonoBehaviour {

    public float speed;
    public MoveDirection moveDirection;

    public Vector3 velocity;

    public bool isStopped { get; set; }

	// Use this for initialization
	private void Start () {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = MoveDirectionToVector(moveDirection) * speed;

    }
	
	Vector3 MoveDirectionToVector (MoveDirection dir)
    {
        Vector3 pos = Vector3.zero;
        switch (dir)
        {
            case MoveDirection.Down:
                pos = Vector3.down;
                break;
            case MoveDirection.Top:
                pos = Vector3.up;
                break;
            case MoveDirection.Right:
                pos = Vector3.right;
                break;
            case MoveDirection.Left:
                pos = Vector3.left;
                break;
            case MoveDirection.TopLeft:
                pos = new Vector3(-1, 1);
                break;
            case MoveDirection.TopRight:
                pos = new Vector3(1, 1);
                break;
            case MoveDirection.DownLeft:
                pos = new Vector3(-1, -1);
                break;
            case MoveDirection.DownRight:
                pos = new Vector3(1, -1);
                break;
            default:
                break;
        }
        return pos;
    }
}
