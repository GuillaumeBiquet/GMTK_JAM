using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{
    [SerializeField] GameObject ropePrefab;

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Pet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ship"))
        {
            Ship ship = collision.gameObject.GetComponent<Ship>();
            if (!ship.IsConnected) {
                ConnectToShip(ship);
            }
        }
    }

    IEnumerator Pet()
    {
        while (true)
        {
            Debug.Log("Pet");

            yield return new WaitForSeconds(5);
            GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * 5000);
        }
    }

    public void ConnectToShip(Ship ship)
    {
        Rope rope = Instantiate(ropePrefab, transform.position, Quaternion.identity).GetComponent<Rope>();
        rope.GenerateRope(this.gameObject, ship.gameObject);
        //ship.Connect();
    }


}
