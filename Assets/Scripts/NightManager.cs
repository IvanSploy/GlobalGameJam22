using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class NightManager : MonoBehaviour
{
    [SerializeField] private float nightTime;
    [SerializeField] public Slider progressbar;
    [SerializeField] private Image backgroundColor;

    private float progressTime;
    public bool isNight;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartNight();
    }

    public void StartNight()
    {
        progressbar.maxValue = nightTime;
        isNight = true;
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        if (isNight)
        {
            progressbar.gameObject.SetActive(true);
            if (progressTime < nightTime)
            {
                progressTime += Time.deltaTime;
                progressbar.value = progressTime;
                Color color = new Color(Normalizar(progressTime), 0.5f - Normalizar(progressTime), 1 - Normalizar(progressTime), 0.65f);
                backgroundColor.color = color;
            }
            else
            {
                isNight = false;
                progressTime = 0;
                progressbar.gameObject.SetActive(false);
                if (GameManager.instance.currentDay < 7) GameManager.instance.TransitionToDay();
                else GameManager.instance.Win();
            }
        }
    }

    private float Normalizar(float num)
    {
        return (num / nightTime);
    }
}
