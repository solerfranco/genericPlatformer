using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement
{
    public Rigidbody2D rb;

    public void Move(float direction, float speed)
    {
        Debug.Log(rb.velocity + "  ");
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }
}
