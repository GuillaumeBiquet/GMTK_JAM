using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float speed = 200;
    protected float damage = 1;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 5);
    }

    public virtual void Launch(Vector2 playerVelocity, Vector2 direction) {}

    public void UpgradeDamage(float damageBoost)
    {
        damage += damageBoost;
    }

}
