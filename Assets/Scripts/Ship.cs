using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] float force;
    [SerializeField] Sprite[] sprites;

    Rigidbody2D rb;
    Vector2 velocity;
    bool isConnected = false;

    public bool IsConnected { get { return isConnected;  } }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        velocity = Random.insideUnitCircle.normalized * force;
        rb.velocity = velocity;

        int index = Random.Range(0, sprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = sprites[index];


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
        if (isConnected && ( collision.gameObject.CompareTag("MotherShip") || collision.gameObject.CompareTag("Ship") ) )
            return;

        velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        rb.velocity = velocity;
    }

    public void ConnectToMothership()
    {
        isConnected = true;
    }

}
