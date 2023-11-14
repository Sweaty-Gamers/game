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

    public GameObject bossHealthUi;
    public GameObject bossHealthBarUi;
    public TextMeshProUGUI bossHealthText;
    public static Slider bossHealthBar;

    public GameObject bossHealthUiOutline;
    public GameObject bossHealthBarUiOutline;
    public TextMeshProUGUI bossHealthTextOutline;
    public static Slider bossHealthBarOutline;

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
        GameObject ui = GameObject.Find("UI");
        weaponUI = ui.transform.Find("Ammo").gameObject;
        ammo = weaponUI.GetComponent<TextMeshProUGUI>();

        healthUi = GameObject.Find("Health");
        healthText = healthUi.GetComponent<TextMeshProUGUI>();
        healthBarUi = GameObject.Find("HealthBar");
        healthBar = healthBarUi.GetComponent<Slider>();

        bossHealthUi = GameObject.Find("Health");
        bossHealthText = bossHealthUi.GetComponent<TextMeshProUGUI>();
        bossHealthBarUi = GameObject.Find("BossHealthBar");
        bossHealthBar = bossHealthBarUi.GetComponent<Slider>();

        bossHealthUiOutline = GameObject.Find("Health");
        bossHealthTextOutline = bossHealthUi.GetComponent<TextMeshProUGUI>();
        bossHealthBarUiOutline = GameObject.Find("BossHealthBar Outline");
        bossHealthBarOutline = bossHealthBarUi.GetComponent<Slider>();

        energyUI = GameObject.Find("Energy");
        energyText = energyUI.GetComponent<TextMeshProUGUI>();
        energyBarUI = GameObject.Find("EnergyBar");
        energyBar = energyBarUI.GetComponent<Slider>();

        playerStats = FindObjectOfType<PlayerScript>();
        healthBar.maxValue = playerStats.maxHealth;
        healthBar.value = playerStats.health;
        energyBar.maxValue = 100f;
        energyBar.value = playerStats.sprintMeter;
        updateEnergy();
        updateHealth();
        updateAmmo();
    }

    void Update()
    {
        GameObject golem = GameObject.Find("Golem");
        GameObject golem_parent = GameObject.Find("GolemPrefab Variant");
        GameObject golem_parent_clone = GameObject.Find("GolemPrefab Variant Variant(Clone)");
        Debug.Log("TESTTTT");
        if (golem_parent != null)
        {
            Debug.Log("GOOD");
        }
        if (golem_parent_clone == null)
        {
            // If the "golem" object doesn't exist or is not active, make the boss UI elements inactive
            bossHealthUi.SetActive(false);
            bossHealthBarUi.SetActive(false);
            bossHealthUiOutline.SetActive(false);
            bossHealthBarUiOutline.SetActive(false);
        }
        else
        {
            bossHealthUi.SetActive(true);
            bossHealthBarUi.SetActive(true);
            bossHealthUiOutline.SetActive(true);
            bossHealthBarUiOutline.SetActive(true);
        }
    }


    public void updateAmmo()
    {
        weapon = playerStats.weapons.transform.GetChild(playerStats.weaponIndex).gameObject;
        weaponStats = weapon.GetComponent<WeaponScript>();
        ammo.text = weaponStats.bulletsLeftInMag.ToString() + "/" + weaponStats.reserveBullets.ToString();
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
    
    public static void updateBossHealth(float newHealth)
    {
        bossHealthBar.value = newHealth;
    }
}
