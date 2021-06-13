using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, ISerializationCallbackReceiver
{
    const float MAX_SCALE = 2f;
    const float MIN_SCALE = 1f;

    const float MAX_SPEED = 5f;
    const float MIN_SPEED = 1f;

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
        rb = this.GetComponent<Rigidbody2D>();

        int index = Random.Range(0, sprites.Length);
        this.GetComponent<SpriteRenderer>().sprite = sprites[index];

        initialScale = Vector3.one * Random.Range(MIN_SCALE, MAX_SCALE);
        transform.localScale = initialScale;
        speed = Random.Range(MIN_SPEED, MAX_SPEED);

        velocity = Random.insideUnitCircle.normalized * speed;
        rb.velocity = velocity;

        gameObject.name = "" + UID;
        hp = maxHp;

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
