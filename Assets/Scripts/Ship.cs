using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour, ISerializationCallbackReceiver
{
    const float MAX_SCALE = 2f;
    const float MIN_SCALE = 1f;

    const float MAX_SPEED = 5f;
    const float MIN_SPEED = 1f;

    [SerializeField] Sprite[] sprites;
    [SerializeField] GameObject connectedGFX;
    [SerializeField] GameObject ropePrefab;

    Rigidbody2D rb;
    Vector2 velocity;
    bool isConnected = false;
    float speed;

    Dictionary<int, Ship> directConnectedShips = new Dictionary<int, Ship>();
    Dictionary<int, Ship> allConnectedShips = new Dictionary<int, Ship>();

    public List<int> _keys = new List<int> ();
    public List<Ship> _values = new List<Ship> ();

    public bool IsConnected { get { return isConnected; } }
    public bool IsInvincible { get { return connectedGFX.activeSelf; } }
    public int UID { get { return gameObject.GetInstanceID(); } }

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

        gameObject.name = "" + UID;
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
            bool scaleIsSmallerThanMine = transform.localScale.magnitude > ship.transform.localScale.magnitude;
            if (scaleIsSmallerThanMine && !IsAlreadyConnectedToMe(ship.UID))
            {
                GenerateRopeToShip(ship);
                this.ConnectToShip(ship);
                ship.ConnectToShip(this);
            }
        }
    }

    bool IsAlreadyConnectedToMe(int _UID)
    {
        Debug.Log(UID +  " contains " + _UID + " : " + allConnectedShips.ContainsKey(_UID));
        return allConnectedShips.ContainsKey(_UID);
    }

    public void GenerateRopeToShip(Ship ship)
    {
        Rope rope = Instantiate(ropePrefab, transform.position, Quaternion.identity).GetComponent<Rope>();
        rope.GenerateRope(this.gameObject, ship.gameObject);
    }

    public void ConnectToShip(Ship ship)
    {
        isConnected = true;
        directConnectedShips.Add(ship.UID, ship);
        allConnectedShips.Add(ship.UID, ship);
        foreach (KeyValuePair<int, Ship> entry in ship.allConnectedShips)
        {
            if(entry.Value != this && !allConnectedShips.ContainsKey(entry.Key))
            {
                allConnectedShips.Add(entry.Key, entry.Value);
                if (!entry.Value.allConnectedShips.ContainsKey(UID) )
                {
                    entry.Value.allConnectedShips.Add(UID, this);
                }
            }
        }

        if (!isConnected)
        {
            isConnected = true;
            connectedGFX.SetActive(true);
        }
    }

    public void DisconnectFromShip(Ship ship)
    {
        directConnectedShips.Remove(ship.UID);
        allConnectedShips.Remove(ship.UID);
        DisconnectFromAllDirectConnectedShipOf(ship, this);
        Debug.Log("eerzeffds");
        if (directConnectedShips.Count == 0)
        {
            connectedGFX.SetActive(false);
            isConnected = false;
        }
    }
    
    void DisconnectFromAllDirectConnectedShipOf(Ship ship, Ship origin)
    {
        foreach (KeyValuePair<int, Ship> entry in ship.directConnectedShips)
        {
            if (allConnectedShips.ContainsKey(entry.Key))
            {
                allConnectedShips.Remove(entry.Key);
                if (entry.Value.allConnectedShips.ContainsKey(UID))
                {
                    entry.Value.allConnectedShips.Remove(UID);
                }
            }

            if (entry.Value != origin)
            {
                DisconnectFromAllDirectConnectedShipOf(entry.Value, this);
            }
        }
    }

    public void OnDestroy()
    {
        GameManager.Instance.nbShips--;
    }


    public void OnBeforeSerialize()
    {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in allConnectedShips)
        {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }


    public void OnAfterDeserialize()
    {
    }





}
