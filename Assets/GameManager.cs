using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public int currentDay = 1;
    private SceneTransitioner transitioner;
    
    [SerializeField] private RawImage minigameImg;
    private MinigameManager minigameManager;

    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    [SerializeField] private Light light;

    private DayManager day;
    private NightManager night;
    
    [SerializeField] Sprite sunLogo;
    [SerializeField] Sprite moonLogo;

    private void Awake() {
        transitioner = FindObjectOfType<SceneTransitioner>();
        minigameManager = FindObjectOfType<MinigameManager>();

        day = FindObjectOfType<DayManager>();
        night = FindObjectOfType<NightManager>();
        
        night.gameObject.SetActive(false);
    }

    public void TransitionToSleep() 
    {
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() => minigameImg.gameObject.SetActive(true) );
        transitioner.OnEnd.AddListener(() => minigameManager.InitMinigame(currentDay));
        transitioner.SetImage(moonLogo);
        transitioner.SetBackgroundColor(Color.black);
        transitioner.SetTextColor(Color.white);
        transitioner.SetTitle("TIME TO SLEEP!");
        transitioner.SetSubtitle("Try not to wake up!");
        transitioner.StartTransition(2);
    }
    
    public void TransitionToNight()
    {
        light.color = new Color(0.094f, 0.057f, 0.28f);
        minigameManager.enabled = false;
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() => {
            minigameImg.gameObject.SetActive(false);
            FindObjectOfType<PlayerManager>().SwitchPlayer();
            });
        transitioner.OnEnd.AddListener(() => night.gameObject.SetActive(true));
        transitioner.SetSubtitle("Try not to kill your sheeps!");
        transitioner.StartTransition(2);
        RenderSettings.skybox = nightSkybox;
    }
    
    public void TransitionToDay()
    {
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() => 
        {
            light.color = Color.white;
            FindObjectOfType<PlayerManager>().SwitchPlayer();
        });
        transitioner.OnEnd.AddListener(() => {
            day.gameObject.SetActive(true);
            day.progressbar.gameObject.SetActive(true);
            RenderSettings.skybox = daySkybox;
        });
        transitioner.SetImage(sunLogo);
        transitioner.SetBackgroundColor(Color.cyan);
        transitioner.SetTextColor(Color.white);
        transitioner.SetSubtitle("Current sheeps: ??!");
        currentDay++;
        transitioner.SetTitle($"Day {currentDay}");
        transitioner.StartTransition(2);
    }
}
