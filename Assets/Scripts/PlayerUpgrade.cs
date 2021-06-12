using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrade : MonoBehaviour
{
    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
       playerController = GetComponent<PlayerController>();
    }

    public void UpgradeHealth()
    {
        playerController.UpgradeHealth(1);
    }

    public void UpgradeSpeed()
    {
        playerController.UpgradeSpeed(25);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
