using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShip : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Pet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger enter");
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

}
