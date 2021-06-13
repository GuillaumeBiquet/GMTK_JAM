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
        StartCoroutine(Grow());
    }

    private void FixedUpdate()
    {
        
    }

    IEnumerator Grow()
    {
        while (gameObject.scene.IsValid() && !GameManager.Instance.TooManyShips && spawnRate > 0.5)
        {
            spawnRate -= 0.1f;
            yield return new WaitForSecondsRealtime(5);
        }
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
                if (Random.Range(0,1) < 0.5)
                {
                    Instantiate(shipPrefab, transform.position, Quaternion.identity);
                    GameManager.Instance.nbShips++;
                    yield return new WaitForSeconds(spawnRate);
                }
            }
        }
    }

}
