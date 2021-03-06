﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIControl : MonoBehaviour
{
    public static UIControl instance;

    private Transform playerStats;
    private Transform shipStats;
    private Transform bossStats;
    private Transform debugStats;

    private Slider levelSlider;
    private Slider healthSlider;
    private Text level;

    private PlayerShip pShip;
    bool isFpsPanelOn = false;

    private void Awake()
    {
        if (!instance)
            instance = this;

        playerStats = transform.Find("PlayerStats");
        if (!playerStats)
            Debug.LogError("Player stats ui not found");

        levelSlider = playerStats.Find("XPSlider").GetComponent<Slider>();
        if(!levelSlider)
            Debug.LogError("Level slider not found");

        healthSlider = playerStats.Find("HealthSlider").GetComponent<Slider>();
        if (!healthSlider)
            Debug.LogError("Health slider not found");
        shipStats = transform.Find("ShipStats");
        if (!shipStats)
            Debug.LogError("Ship stats ui not found");

        bossStats = transform.Find("BossStats");
        if (!bossStats)
            Debug.LogError("Boss stats ui not found");

        debugStats = transform.Find("DebugStats");
        if (!debugStats)
            Debug.LogError("Debug stats ui not found");

        pShip = GameObject.Find("Player").GetComponent<PlayerShip>();
        if (!pShip)
            Debug.LogError("No player ship in UIControls found");

        ShowUpgrades(false);
        ShowBossStats(false);
        ShowFPS(false);
    }

    private void Update()
    {
        UpdateFPS((1f / Time.unscaledDeltaTime));
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.F))
            ShowFPS(!isFpsPanelOn);
    }

    public void UpdateFPS(float fps)
    {
        debugStats.Find("FPS").GetComponent<Text>().text = $"Fps: {fps}";
    }

    public void ShowBossStats(bool value)
    {
        bossStats.gameObject.SetActive(value);
    }

    public void UpdateBossStats(float bossHealth, string bossName)
    {
        bossStats.Find("HealthSlider").GetComponent<Slider>().value = bossHealth;
        bossStats.Find("Name").GetComponent<Text>().text = bossName;
    }

    public void UpdateXP(float newXp)
    {
        levelSlider.value = newXp;
    }

    public void UpdateHealth(float newHealth)
    {
        healthSlider.value = newHealth;
    }

    public void UpdateLevel(int newLevel)
    {
        playerStats.Find("Level").GetComponent<Text>().text = newLevel.ToString();
    }

    public void ShowUpgrades(bool value)
    {
        if (value)
            UpdateUpgradePanel();
        shipStats.gameObject.SetActive(value);
    }

    public void ShowFPS(bool value)
    {
        Debug.Log(value);
        isFpsPanelOn = value;
        debugStats.gameObject.SetActive(value);
    }

    public void UpdateUpgradePanel()
    {
        Stats pStats = pShip.stats;
        // Update points
        shipStats.Find("UpgradePoints").GetComponent<Text>().text = $"+{PlayerProgression.instance.upgradePoints}";
        // Update sliders
        float maxUp = pStats.MaxUpgrades;
        UpdateSlider("FireRateUpgrade", pStats.FireRateUpgrades / maxUp);
        UpdateSlider("BulletSpeedUpgrade", pStats.BulletSpeedUpgrades / maxUp);
        UpdateSlider("BulletDamageUpgrade", pStats.BulletDmgUpgrades / maxUp);
        UpdateSlider("RangeUpgrade", pStats.RangeUpgrades / maxUp);
        UpdateSlider("MovementSpeedUpgrade", pStats.MovementSpeedUpgrades / maxUp);
        //UpdateSlider("MaxHealthUpgrade", pStats.HealthUpgrades / maxUp);
        shipStats.Find("MaxHealthUpgrade").Find("Slider").GetComponent<Slider>().value = pStats.HealthUpgrades / maxUp;
    }

    private void UpdateSlider(string sliderName, float value)
    {
        shipStats.Find(sliderName).Find("Slider").GetComponent<Slider>().value = value;
    }
}
