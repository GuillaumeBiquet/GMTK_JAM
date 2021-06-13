using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const float MAX_VELOCITY = 5f;
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
    [SerializeField] GameObject upgradePanel;
    ParticleSystem.EmissionModule emission;

    [Header("Stats")]
    [SerializeField] float health = 10;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float thrustPower = 100;
    float lastFired;


    [Header("Fire")]
    [SerializeField] Transform bulletPos;
    [SerializeField] GameObject bullet1;
    [SerializeField] float fireCd = 0.2f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        emission = thrustParticles.emission;
        emission.enabled = false;
        //upgradePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        mouseOnScreen = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mouseDirection = (mouseOnScreen - (Vector2)transform.position).normalized;

        isThrust = Input.GetKey(KeyCode.Space);
        isFiring = Input.GetKey(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.C))
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
        }

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
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, MAX_VELOCITY);
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

    }

    public void UpgradeSpeed(float value)
    {
        Debug.Log("Upgraded Speed");
        thrustPower += value;
    }
}
