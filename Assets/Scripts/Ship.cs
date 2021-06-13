using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, ISerializationCallbackReceiver
{
    const float MAX_SCALE = 2f;
    const float MIN_SCALE = 1f;

    const float MAX_SPEED = 15f;
    const float MIN_SPEED = 5f;

    [SerializeField] LayerMask mask;
    [SerializeField] float maxHp = 3f;
    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject connectedGFX;
    [SerializeField] GameObject ropePrefab;

    Rigidbody2D rb;
    Vector2 velocity;
    Vector3 initialScale;
    bool isConnected = false;
    float speed;
    float hp = 0f;
    Dictionary<int, Ship> connectedShips = new Dictionary<int, Ship>();

    // for inspector
    public List<Ship> listConnectedShips = new List<Ship> ();

    public bool IsConnected { get { return isConnected; } }
    public bool IsInvincible { get { return connectedGFX.activeSelf; } }

    public int UID { get { return gameObject.GetInstanceID(); } }

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.reuseCollisionCallbacks = true;

        rb = this.GetComponent<Rigidbody2D>();

        int index = Random.Range(0, sprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = sprites[index];

        initialScale = Vector3.one * Random.Range(MIN_SCALE, MAX_SCALE);
        transform.localScale = initialScale;
        

        gameObject.name = "" + UID;
        hp = maxHp;

        StartCoroutine(DetectAround());

    }
    public void Launch(float speed)
    {
        //speed = Random.Range(MIN_SPEED, MAX_SPEED);

        velocity = Random.insideUnitCircle.normalized * speed;
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.velocity = velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isCollidingWithShipWhenConnected = isConnected && collision.gameObject.CompareTag("Ship");
        if (isCollidingWithShipWhenConnected || collision.gameObject.CompareTag("Player"))
            return;
        velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        rb.velocity = velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isConnected)
        {
            return;
        }
        if (collision.gameObject.CompareTag("Ship") && collision.isTrigger)
        {
            Ship ship = collision.gameObject.GetComponent<Ship>();

            bool scaleIsSmallerThanMine = transform.localScale.magnitude > ship.transform.localScale.magnitude;

            if( scaleIsSmallerThanMine || (ship.IsConnected && this.IsConnected))
            {
                return;
            }

            if (!IsAlreadyConnectedToMe(ship.UID, this))
            {
                GenerateRopeToShip(ship);
                this.ConnectToShip(ship);
                ship.ConnectToShip(this);
            }

        }
    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 2)
        {
            rb.velocity *= 2;
        }
    }

    IEnumerator DetectAround()
    {
        while (true)
        {
            RaycastHit2D[] hit = Physics2D.CircleCastAll(transform.position, 2, Vector2.zero, 0, mask);

            foreach (RaycastHit2D h in hit)
            {
                if (h.collider.gameObject.name.Equals(this.UID.ToString()) || h.collider.isTrigger)
                {
                    continue;
                }
                Vector2 direction = -(h.collider.transform.position - transform.position);
                Vector2 force = direction.normalized * 200;
                rb.AddForce(force);
            }
            yield return new WaitForSeconds(2);
        }
    }

    bool IsAlreadyConnectedToMe(int _UID, Ship origin)
    {
        return connectedShips.ContainsKey(_UID);
    }

    public void GenerateRopeToShip(Ship ship)
    {
        Rope rope = Instantiate(ropePrefab, transform.position, Quaternion.identity).GetComponent<Rope>();
        rope.GenerateRope(this.gameObject, ship.gameObject);
    }

    public void ConnectToShip(Ship ship)
    {
        connectedShips.Add(ship.UID, ship);
        if (!isConnected)
        {
            isConnected = true;
            connectedGFX.SetActive(true);
            StopCoroutine(Regen());
            StartCoroutine(Regen());
        }
    }

    public void DisconnectFromShip(Ship ship)
    {
        connectedShips.Remove(ship.UID);
        if (connectedShips.Count == 0)
        {
            connectedGFX.SetActive(false);
            isConnected = false;
        }
    }


    public void TakeDamage(float damage)
    {
        if (!isConnected)
        {
            hp -= damage;

            if (hp <= 0)
            {
                GameManager.Instance.crystals += 25;
                Destroy(gameObject);
            }
            else
            {
                transform.localScale = initialScale * (hp / maxHp);
            }
        }
    }


    IEnumerator Regen()
    {
        yield return new WaitForSeconds(4f);
        while (isConnected)
        {
            if (hp < maxHp)
            {
                hp = Mathf.Clamp(hp+1, 0, maxHp);
                transform.localScale = initialScale * (hp / maxHp);
                yield return new WaitForSeconds(4f);
            }
            yield return new WaitForEndOfFrame();
        }
    }


    public void OnDestroy()
    {
        GameManager.Instance.nbShips--;
    }


    public void OnBeforeSerialize()
    {
        listConnectedShips.Clear();

        foreach (var kvp in connectedShips)
        {
            listConnectedShips.Add(kvp.Value);
        }
    }


    public void OnAfterDeserialize()
    {
    }





}
