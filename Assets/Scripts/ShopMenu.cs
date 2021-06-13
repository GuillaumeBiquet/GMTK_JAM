using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public float healthCost = 150;
    [SerializeField] TextMeshProUGUI TMHealthCost;
    [SerializeField] Button healthButton;
    public float speedCost = 150;
    [SerializeField] TextMeshProUGUI TMSpeedCost;
    [SerializeField] Button speedButton;
    public float damageCost = 150;
    [SerializeField] TextMeshProUGUI TMDamageCost;
    [SerializeField] Button damageButton;
    public float shieldCost = 150;
    [SerializeField] TextMeshProUGUI TMShieldCost;
    [SerializeField] Button shieldButton;


    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        healthButton.interactable = GameManager.Instance. crystals >= healthCost;
        speedButton.interactable = GameManager.Instance.crystals >= speedCost;
        damageButton.interactable = GameManager.Instance.crystals >= damageCost;
        shieldButton.interactable = GameManager.Instance.crystals >= shieldCost;

        TMHealthCost.text = healthCost.ToString();
        TMSpeedCost.text = speedCost.ToString();
        TMDamageCost.text = damageCost.ToString();
        TMShieldCost.text = shieldCost.ToString();

    }

    public void UpgradeHealth()
    {
        if (GameManager.Instance.crystals > healthCost)
        {
            GameManager.Instance.crystals -= healthCost;
            playerController.UpgradeHealth(1);
            if (healthCost < 665)
            {
                healthCost *= 1.5f;
                healthCost = Mathf.Round(healthCost);
            }
            
            if (healthCost > 665)
            {
                healthCost = 666;
            }
        }
    }

    public void UpgradeSpeed()
    {
        if (GameManager.Instance.crystals > speedCost)
        {
            GameManager.Instance.crystals -= speedCost;
            playerController.UpgradeSpeed();
            if (speedCost < 665)
            {
                speedCost *= 1.5f;
                speedCost = Mathf.Round(speedCost);
            }

            if (speedCost > 665)
            {
                speedCost = 666;
            }
            
        }
    }

    public void UpgradeDamage()
    {
        if (GameManager.Instance.crystals > damageCost)
        {
            GameManager.Instance.crystals -= damageCost;
            playerController.UpgradeDamage();

            if (damageCost < 665)
            {
                damageCost *= 1.5f;
                damageCost = Mathf.Round(damageCost);
            }

            if (damageCost > 665)
            {
                damageCost = 666;
            }
            
        }
    }

    public void BuyShield()
    {
        if (GameManager.Instance.crystals > shieldCost)
        {
            GameManager.Instance.crystals -= shieldCost;
            //toDo
            playerController.BuyShield();

            if (shieldCost < 665)
            {
                shieldCost *= 1.5f;
                shieldCost = Mathf.Round(shieldCost);
            }

            if (shieldCost > 665)
            {
                shieldCost = 666;
            }

        }
    }


}
