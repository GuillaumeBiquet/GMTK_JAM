using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float maxVelocity = 5f;
    const float DRAG_INCREMENT = 5f;
    const float MAX_DRAG = 2.5f;


    Vector2 mouseOnScreen;
    Vector2 mouseDirection;
    Quaternion lookRotation;
    Quaternion adjustRotation;


    Rigidbody2D rb;
    bool isFiring;
    bool isThrust;


    [SerializeField] Camera mainCam;
    [SerializeField] ParticleSystem thrustParticles;
    ParticleSystem.EmissionModule emission;

    [Header("Stats")]
    [SerializeField] float maxHealth = 10;
    [SerializeField] HealthBar healthBar;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float thrustPower = 100;

    public float bulletDamage = 1;
    public float bulletSpeed = 200;
    float lastFired;
    float health;

    [Header("Fire")]
    [SerializeField] Transform bulletPos;
    [SerializeField] GameObject bullet1;
    [SerializeField] float fireCd = 0.2f;


    [Header("Invincibility")]
    [SerializeField] float invincibilityTime = 1.5f;
    [SerializeField] float invincibilityDeltaTime = 0.15f;
    bool isInvincible = false;

    public bool IsDead { get { return health <= 0; } }


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        emission = thrustParticles.emission;
        emission.enabled = false;

        health = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        healthBar.SetValue(health);
    }

    // Update is called once per frame
    void Update()
    {
        mouseOnScreen = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = (mouseOnScreen - (Vector2)transform.position).normalized;

        isThrust = Input.GetKey(KeyCode.Space);
        isFiring = Input.GetKey(KeyCode.Mouse0);

        if (isFiring && Time.time > lastFired + fireCd)
        {
            lastFired = Time.time;
            Fire();
        }

        LookAt(mouseOnScreen - (Vector2)transform.position, 180);
    }

    private void FixedUpdate()
    {
        if (isThrust)
        {
            rb.AddForce(mouseDirection * Time.fixedDeltaTime * thrustPower);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
            emission.enabled = true;
        }
        else if (rb.velocity != Vector2.zero)
        {
            emission.enabled = false;
            StartCoroutine(SlowDown());
        }
    }

    void Fire()
    {
        mouseOnScreen = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = (mouseOnScreen - (Vector2)transform.position).normalized;
        Bullet bullet = Instantiate(bullet1, bulletPos.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.Launch(rb.velocity, mouseDirection);
    }


    IEnumerator SlowDown()
    {
        while (!isThrust)
        {
            if(rb.drag < MAX_DRAG)
            {
                rb.drag += DRAG_INCREMENT * Time.deltaTime;
            }
            yield return new WaitForEndOfFrame();
        }
        rb.drag = 0;
        yield return new WaitForEndOfFrame();
    }

    void LookAt(Vector3 direction, float offSet=0)
    {
        this.rb.angularVelocity = 0;
        adjustRotation = Quaternion.Euler(0, 0, offSet);
        lookRotation = Quaternion.LookRotation(Vector3.forward, direction) * adjustRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    public void UpgradeHealth(float value)
    {
        Debug.Log("Upgraded Health");
        health += value;
    }

    public void UpgradeDamage()
    {
        Debug.Log("Upgraded Damage");
<<<<<<< Updated upstream
        bullet1.GetComponent<Bullet>().UpgradeDamage(value);
=======
        bulletSpeed += 50;
        fireCd /= 1.2f;
>>>>>>> Stashed changes
    }

    public void UpgradeSpeed()
    {
        Debug.Log("Upgraded Speed");
        thrustPower += 25;
        maxVelocity += 1;
    }

    //TODO
    public void BuyShield()
    {
        Debug.Log("Shield bought");
    }


    public void TakeDamage(float damage)
    {
        if (isInvincible)
        {
            return;
        }


        health -= damage;
        if (IsDead)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            healthBar.SetValue(health);
            StartCoroutine(BecomeTemporarilyInvincible());
        }
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;

        for (float i = 0; i < invincibilityTime; i += invincibilityDeltaTime)
        {
            // Alternate between 0 and 1 scale to simulate flashing
            if (transform.localScale == Vector3.one)
            {
                transform.localScale = Vector3.zero;
            }
            else
            {
                transform.localScale = Vector3.one;
            }
            yield return new WaitForSeconds(invincibilityDeltaTime);
        }

        transform.localScale = Vector3.one;
        isInvincible = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("Ship") &&  !collision.isTrigger) || collision.gameObject.CompareTag("RopeSegment"))
        {
            TakeDamage(1f);
        }
    }

}
