using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float weaponCooldown;
    public Player player;
    public GameObject playerGO;
    private Slider hpSlider;
    private Slider cooldownSlider;
    private LevelManager levelmanager;

    private void Start()
    {
        health = 100;
        player = gameObject.GetComponent<Player>();
        hpSlider = GameObject.FindGameObjectWithTag("HealthUI").GetComponent<Slider>();
        cooldownSlider = GameObject.FindGameObjectWithTag("CooldownUI").GetComponent<Slider>();
        hpSlider.maxValue = health;
        cooldownSlider.maxValue = weaponCooldown;
        levelmanager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();

        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        hpSlider.value = health;
        cooldownSlider.value = weaponCooldown;

        if (health <= 0)
        {
            //player.instanceForestSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            health = 100;
            levelmanager.FadeInCanvas();
            switch (levelmanager.playerCheckpoint)
            {
                case 0:
                    playerGO.transform.position = levelmanager.spawn.position;
                    break;
                case 1:
                    playerGO.transform.position = levelmanager.checkpoint0.position;
                    break;
                case 2:
                    playerGO.transform.position = levelmanager.checkpoint1.position;
                    break;
                case 3:
                    
                    playerGO.transform.position = levelmanager.checkpoint2.position;
                    break;
            }    
        }
    }
}
