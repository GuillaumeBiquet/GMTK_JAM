using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 10);
    }

    public virtual void Launch(Vector2 direction)
    {

    }
}
