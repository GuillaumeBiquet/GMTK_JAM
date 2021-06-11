using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] float angle;
    [SerializeField] float force;

    Rigidbody2D rb;
    Vector2 velocity;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        velocity = Random.insideUnitCircle.normalized * force;
        rb.velocity = velocity;

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MotherShip"))
            return;

        Debug.Log("here1: " + collision.gameObject.name);
        velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        rb.velocity = velocity;
    }

}
