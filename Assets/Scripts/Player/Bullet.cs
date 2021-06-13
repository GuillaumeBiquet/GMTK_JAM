using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected float speed = 400;
    protected float damage = 1;

    protected const float LIFE_TIME = 1.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, LIFE_TIME);
    }

    public virtual void Launch(Vector2 playerVelocity, Vector2 direction) {}

    public void UpgradeDamage(float damageBoost)
    {
        damage += damageBoost;
    }

}
