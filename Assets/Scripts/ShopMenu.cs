using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    public float healthCost = 150;
    [SerializeField] TextMeshProUGUI TMHealthCost;
    [SerializeField] TextMeshProUGUI TMHealthStat;
    [SerializeField] Button healthButton;
    public float speedCost = 150;
    [SerializeField] TextMeshProUGUI TMSpeedCost;
    [SerializeField] TextMeshProUGUI TMSpeedStat;
    [SerializeField] Button speedButton;
    public float damageCost = 150;
    [SerializeField] TextMeshProUGUI TMDamageCost;
    [SerializeField] Button damageButton;
    public float shieldCost = 300;
    [SerializeField] TextMeshProUGUI TMShieldCost;
    [SerializeField] Button shieldButton;

    [SerializeField] TextMeshProUGUI TMBank;


    PlayerController playerController;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameManager.Instance.player;
    }

    // Update is called once per frame
    void Update()
    {
        healthButton.interactable = GameManager.Instance. crystals >= healthCost && playerController.health < playerController.maxHealth;
        speedButton.interactable = GameManager.Instance.crystals >= speedCost;
        damageButton.interactable = GameManager.Instance.crystals >= damageCost;
        shieldButton.interactable = GameManager.Instance.crystals >= shieldCost && !playerController.HasShield;

        TMHealthCost.text = healthCost.ToString();
        TMSpeedCost.text = speedCost.ToString();
        TMDamageCost.text = damageCost.ToString();
        TMShieldCost.text = shieldCost.ToString();
        TMBank.text = GameManager.Instance.crystals.ToString();

    }

    public void UpgradeHealth()
    {
        if (GameManager.Instance.crystals > healthCost)
        {
            GameManager.Instance.crystals -= healthCost;
            playerController.Heal();
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
        if (GameManager.Instance.crystals > shieldCost && !playerController.HasShield)
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
