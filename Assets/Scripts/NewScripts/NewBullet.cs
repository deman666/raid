using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBullet : MonoBehaviour
{
    private bool canMove;
    private Vector3 destinationVector;

    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, destinationVector, 0.01f);
        }
    }

    public void SetDistinationVector(Vector3 vector)
    {
        destinationVector = vector;
        canMove = true;
    }
}
