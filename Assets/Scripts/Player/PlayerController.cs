using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 mouseOnScreen;
    Vector2 positionOnScreen;
    Vector2 realPos;
    Rigidbody2D rb;
    [SerializeField] Camera mainCam;
    [Header("Stats")]
    Vector2 velocity;
    float veloctyMag;
    [SerializeField] float rotationSpeed = 1;
    [SerializeField] float thrustPower = 100;
    float lastFired;
    [SerializeField] float fireCd = 0.2f;
    [Header("Bullets")]
    [SerializeField] GameObject bullet1;
    [Header("Controls")]
    [SerializeField] bool isFiring;
    [SerializeField] bool isThrust;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        mouseOnScreen = mainCam.ScreenToWorldPoint(Input.mousePosition);
        positionOnScreen = mainCam.ScreenToWorldPoint(transform.position);
        realPos = mouseOnScreen - (Vector2)transform.position;
        velocity = rb.velocity;
        veloctyMag = velocity.magnitude;

        if (Input.GetKey(KeyCode.Space))
        {
            isThrust = true;
        } else
        {
            isThrust = false;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            isFiring = true;
        } else
        {
            isFiring = false;
        }

        if (isFiring && Time.time > lastFired + fireCd)
        {
            lastFired = Time.time;
            Fire();
        }

        LookAt(mouseOnScreen - (Vector2)transform.position);
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, 5);
    }

    private void FixedUpdate()
    {
        if (isThrust)
        {
            Vector2 force = realPos * Time.fixedDeltaTime * thrustPower;
            rb.AddForce(force);
        } else if (Mathf.Abs(rb.velocity.x) > 0 || Mathf.Abs(rb.velocity.y) > 0)
        {
            StartCoroutine(SlowDown());
        }
    }

    void Fire()
    {
        GameObject bullet = Instantiate(bullet1, transform.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet1>();
        bulletScript.Launch(realPos);
    }


    IEnumerator SlowDown()
    {
        float began = Time.time;
        float dragging = 0.05f;
        while (!isThrust && rb.drag < 2.5f)
        {
            rb.drag += dragging * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        while (!isThrust)
        {
            yield return new WaitForEndOfFrame();
        }
        rb.drag = 0;
        yield return new WaitForEndOfFrame();
    }

    void LookAt(Vector3 direction)
    {
        this.rb.angularVelocity = 0;
        Quaternion _lookRotation = Quaternion.LookRotation(Vector3.forward, direction);
        Quaternion adjustRotation = Quaternion.Euler(0, 0, 180);
        _lookRotation = _lookRotation * adjustRotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * rotationSpeed);
    }
}
