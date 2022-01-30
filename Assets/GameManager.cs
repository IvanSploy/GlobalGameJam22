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

    private Image TransitionLogo;
    [SerializeField] private Image bgTrans;

    private void Awake() {
        transitioner = FindObjectOfType<SceneTransitioner>();
        minigameManager = FindObjectOfType<MinigameManager>();

        day = FindObjectOfType<DayManager>();
        night = FindObjectOfType<NightManager>();
        
        night.gameObject.SetActive(false);

        TransitionLogo = GameObject.Find("Image").GetComponent<Image>();
        bgTrans = GameObject.Find("Fondo").GetComponent<Image>();
    }

    public void TransitionToSleep() {
        
        StartCoroutine(ToSleepCoroutine());

    }

    IEnumerator ToSleepCoroutine() {
        TransitionLogo.sprite = moonLogo;
        bgTrans.color = Color.black;
        transitioner.SetText("TIME TO SLEEP!");
        transitioner.StartTransition(2);
        yield return new WaitForSeconds(1);
        minigameImg.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        minigameManager.InitMinigame(currentDay);
    }
    
    public void TransitionToNight() {
        StartCoroutine(ToNightCoroutine());
        RenderSettings.skybox = nightSkybox;
    }
    
    IEnumerator ToNightCoroutine() {
        light.color = new Color(0.094f, 0.057f, 0.28f);
        transitioner.SetText($"Night {currentDay}");
        minigameManager.enabled = false;
        transitioner.StartTransition(2);
        yield return new WaitForSeconds(1);
        minigameImg.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        night.gameObject.SetActive(true);
        
    }
    
    public void TransitionToDay() {
        StartCoroutine(ToDayCoroutine());
        RenderSettings.skybox = daySkybox;
    }
    
    IEnumerator ToDayCoroutine() {
        TransitionLogo.sprite = sunLogo;
        bgTrans.color = Color.cyan;
        currentDay++;
        transitioner.SetText($"Day {currentDay}");
        transitioner.StartTransition(2);
        yield return new WaitForSeconds(1);
        light.color = Color.white;
        yield return new WaitForSeconds(2);
        day.gameObject.SetActive(true);
        day.progressbar.gameObject.SetActive(true);
    }
}
