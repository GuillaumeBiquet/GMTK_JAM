using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : Bullet
{

    float speed = 200;
    float damage = 1;

    public override void Launch(Vector2 direction)
    {
        rb.AddForce(direction.normalized * speed);
    }
}
