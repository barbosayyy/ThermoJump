using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float weaponCooldown;
    private Slider hpSlider;
    private Slider cooldownSlider;

    private void Start()
    {
        health = 100;
        hpSlider = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Slider>();
        cooldownSlider = GameObject.FindGameObjectWithTag("CooldownUI").GetComponent<Slider>();
        hpSlider.maxValue = health;
        cooldownSlider.maxValue = weaponCooldown;
    }

    private void Update()
    {
        hpSlider.value = health;
        cooldownSlider.value = weaponCooldown;
    }
}
