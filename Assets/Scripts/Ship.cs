using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    const float MAX_SCALE = 2f;
    const float MIN_SCALE = 1f;

    const float MAX_SPEED = 5f;
    const float MIN_SPEED = 1f;

    [SerializeField] float speed;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject ropePrefab;

    Rigidbody2D rb;
    Vector2 velocity;
    bool isConnected = false;

    public bool IsConnected { get { return isConnected;  } }

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        int index = Random.Range(0, sprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = sprites[index];

        transform.localScale = Vector3.one * Random.Range(MIN_SCALE, MAX_SCALE);
        speed = Random.Range(MIN_SPEED, MAX_SPEED);

        velocity = Random.insideUnitCircle.normalized * speed;
        rb.velocity = velocity;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isConnected && ( collision.gameObject.CompareTag("MotherShip") || collision.gameObject.CompareTag("Ship") ) )
            return;

        velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship ship = collision.gameObject.GetComponent<Ship>();
            if (!ship.IsConnected && this.gameObject.transform.localScale.magnitude > collision.gameObject.transform.localScale.magnitude)
            {
                ConnectToShip(ship);
            }
        }
    }

    public void ConnectToShip(Ship ship)
    {
        Rope rope = Instantiate(ropePrefab, transform.position, Quaternion.identity).GetComponent<Rope>();
        rope.GenerateRope(this.gameObject, ship.gameObject);
        ship.Connect();
    }

    public void Connect()
    {
        isConnected = true;
        
    }

    public void OnDestroy()
    {
        GameManager.Instance.nbShips--;
    }

}
