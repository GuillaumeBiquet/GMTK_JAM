using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    PlayerController playerController;

    float speedCost = 1000;
    float healthCost = 1000;
    // Start is called before the first frame update
    void Start()
    {
       playerController = GetComponent<PlayerController>();
    }

    public void UpgradeHealth()
    {
        if (GameManager.Instance.coins > healthCost)
        {
            GameManager.Instance.coins -= healthCost;
            playerController.UpgradeHealth(1);

        }
    }

    public void UpgradeSpeed()
    {
        if (GameManager.Instance.coins > speedCost)
        {
            GameManager.Instance.coins -= speedCost;
            playerController.UpgradeSpeed(25);

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
