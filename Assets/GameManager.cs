using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    //Referencia
    public static GameManager instance;

    public int currentDay = 1;
    private SceneTransitioner transitioner;

    [SerializeField] private GameObject mobileCanvas;
    [SerializeField] private RawImage minigameImg;
    private MinigameManager minigameManager;

    [SerializeField] private Material daySkybox;
    [SerializeField] private Material nightSkybox;

    [SerializeField] private Light light;

    private DayManager day;
    private NightManager night;
    
    [SerializeField] Sprite sunLogo;
    [SerializeField] Sprite moonLogo;
    public int ovejitasvivas = 15;
    [SerializeField] TextMeshProUGUI numeroovejitasGUI;

    private void Awake() {
        if (instance)
            Destroy(this);
        instance = this;
    }

    private void Start()
    {
        mobileCanvas.SetActive(true);
        ItemManager.instance.GenerateItems();
        transitioner = FindObjectOfType<SceneTransitioner>();
        minigameManager = FindObjectOfType<MinigameManager>();

        day = FindObjectOfType<DayManager>();
        night = FindObjectOfType<NightManager>();

        night.gameObject.SetActive(false);
        StartCoroutine(WaitForGameOver());
    }

    private void Update()
    {
        numeroovejitasGUI.text = (ovejitasvivas.ToString());
    }

    public void TransitionToSleep() 
    {
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() =>
        {
            minigameImg.gameObject.SetActive(true);
            mobileCanvas.SetActive(false);
            night.gameObject.SetActive(true);
        });
        transitioner.OnEnd.AddListener(() => minigameManager.InitMinigame(currentDay));
        transitioner.SetImage(moonLogo);
        transitioner.SetBackgroundColor(Color.black);
        transitioner.SetTextColor(Color.white);
        transitioner.SetTitle("TIME TO SLEEP!");
        transitioner.SetSubtitle("Try not to wake up!");
        transitioner.StartTransition(2);
        MusicManager.instance.SetSong(0);
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
        transitioner.SetBackgroundColor(Color.black);
        transitioner.SetTextColor(Color.red);
        transitioner.SetTitle("WOLF IS AWAKE!");
        transitioner.SetSubtitle("May your sheeps survive?!");
        transitioner.StartTransition(2);
        RenderSettings.skybox = nightSkybox;
    }
    
    public void TransitionToDay()
    {
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() => 
        {
            light.color = Color.white;
            mobileCanvas.SetActive(true);
            FindObjectOfType<PlayerManager>().SwitchPlayer();
            RenderSettings.skybox = daySkybox;
            ItemManager.instance.GenerateItems();
        });
        transitioner.OnEnd.AddListener(() => {
            day.gameObject.SetActive(true);
            day.progressbar.gameObject.SetActive(true);
        });
        transitioner.SetImage(sunLogo);
        transitioner.SetBackgroundColor(new Color(0.75f, 1f, 1f));
        transitioner.SetTextColor(Color.black);
        currentDay++;
        transitioner.SetSubtitle($"{ovejitasvivas} sheeps alive");
        transitioner.SetTitle($"Day {currentDay} of 7");
        transitioner.StartTransition(2);
        MusicManager.instance.SetSong(1);
    }

    public void GameOver()
    {
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() =>
        {
            LevelLoader.instance.ChangeScene(0);
        });
        transitioner.SetImage(moonLogo);
        transitioner.SetBackgroundColor(Color.black);
        transitioner.SetTextColor(Color.white);
        currentDay++;
        transitioner.SetSubtitle("All sheeps are dead :(");
        transitioner.SetTitle($"Game Over");
        transitioner.StartTransition(2);
        MusicManager.instance.SetSong(0);
    }

    public void Win()
    {
        transitioner.ResetEvents();
        transitioner.OnTransition.AddListener(() =>
        {
            LevelLoader.instance.ChangeScene(0);
        });
        transitioner.SetImage(sunLogo);
        transitioner.SetBackgroundColor(Color.black);
        transitioner.SetTextColor(Color.white);
        currentDay++;
        transitioner.SetSubtitle("All sheeps are dead :(");
        transitioner.SetTitle($"Victoria");
        transitioner.StartTransition(2);
        MusicManager.instance.SetSong(0);
    }

    IEnumerator WaitForGameOver()
    {
        yield return new WaitUntil(() => ovejitasvivas==0);
        GameOver();
    }
}
