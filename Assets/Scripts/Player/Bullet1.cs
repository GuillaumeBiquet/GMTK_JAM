using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : Bullet
{

    public override void Launch(Vector2 playerVelocity ,Vector2 direction)
    {
        rb.velocity = playerVelocity;
        rb.AddForce(direction.normalized * speed);
    }
}
