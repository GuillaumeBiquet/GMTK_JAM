using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject shipPrefab;
    [SerializeField] float spawnRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        while (gameObject.scene.IsValid())
        {
            if (GameManager.Instance.TooManyShips)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                GameObject go = Instantiate(shipPrefab, transform.position, Quaternion.identity);
                GameManager.Instance.nbShips++;
                yield return new WaitForSeconds(spawnRate);
            }
        }
    }

}
