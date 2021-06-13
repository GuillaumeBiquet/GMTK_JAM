using MoreMountains.Feedbacks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet1 : Bullet
{
    [SerializeField] ParticleSystem explo;

    public override void Launch(Vector2 playerVelocity ,Vector2 direction)
    {
        rb.velocity = playerVelocity;
        rb.AddForce(direction.normalized * speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RopeSegment"))
        {
            collision.gameObject.GetComponent<RopeSegment>().TakeDamage(damage);
            Impact();
        }
        else if (collision.CompareTag("Ship") && !collision.isTrigger)
        {
            collision.gameObject.GetComponent<Ship>().TakeDamage(damage);
            Impact();
        }
        else if (collision.CompareTag("Wall"))
        {
            Impact();
        }

    }

    void Impact()
    {
        Instantiate(explo, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}
