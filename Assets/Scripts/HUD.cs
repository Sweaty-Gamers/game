using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public GameObject healthUi;
    public GameObject healthBarUi;
    public TextMeshProUGUI healthText;
    public Slider healthBar;
    public PlayerScript playerStats;
    public GameObject player;

    public GameObject energyBarUI;
    public GameObject energyUI;
    public TextMeshProUGUI energyText;
    public Slider energyBar;

    public GameObject weapon; 
    public WeaponScript weaponStats;
    public GameObject weaponUI;
    public TextMeshProUGUI ammo;

    void Start()
    {
        healthUi = GameObject.Find("Health");
        healthText = healthUi.GetComponent<TextMeshProUGUI>();
        healthBarUi = GameObject.Find("HealthBar");
        healthBar = healthBarUi.GetComponent<Slider>();

        energyUI = GameObject.Find("Energy");
        energyText = energyUI.GetComponent<TextMeshProUGUI>();
        energyBarUI = GameObject.Find("EnergyBar");
        energyBar = energyBarUI.GetComponent<Slider>();


        weaponUI = GameObject.Find("Ammo");
        ammo = weaponUI.GetComponent<TextMeshProUGUI>();


        playerStats = FindObjectOfType<PlayerScript>();
        healthBar.maxValue = playerStats.maxHealth;
        healthBar.value = playerStats.health;
        energyBar.maxValue = 100f;
        energyBar.value = playerStats.sprintMeter;
        updateEnergy();
        updateHealth();
        updateAmmo();
    }

    public void updateAmmo()
    {
        weapon = playerStats.weapons.transform.GetChild(playerStats.weaponIndex).gameObject;
        weaponStats = weapon.GetComponent<WeaponScript>();
        ammo.text = weaponStats.bulletsLeftInMag + "/" + weaponStats.reserveBullets;
    }

    public void updateEnergy()
    {
        //energyText.text = playerStats.sprintMeter.ToString() + " / " + "100";
        energyBar.value = playerStats.sprintMeter;
    }

    public void updateHealth()
    {
        //healthText.text = playerStats.health.ToString() + " / " + playerStats.maxHealth.ToString();
        healthBar.value = playerStats.health;
    }
}
