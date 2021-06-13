using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected const float LIFE_TIME = 1.5f;

    protected PlayerController playerController;
    protected float speed;
    protected float damage;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerController = GameManager.Instance.player;
        speed = playerController.bulletSpeed;
        damage = playerController.bulletDamage;
        Destroy(this.gameObject, LIFE_TIME);
    }

    public virtual void Launch(Vector2 playerVelocity, Vector2 direction) {}

    public void UpgradeDamage(float damageBoost)
    {
        damage += damageBoost;
    }

    public void UpgradeSpeed(float value)
    {
        speed += value;
    }

}
