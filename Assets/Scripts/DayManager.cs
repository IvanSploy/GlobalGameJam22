using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DayManager : MonoBehaviour
{
    [SerializeField] private float dayTime;
    [SerializeField] public Slider progressbar;
    [SerializeField] private Image backgroundColor;

    private float progressTime;

    public bool isDay;

    // Start is called before the first frame update
    void OnEnable()
    {
        StartDay();
    }

    public void StartDay()
    {
        isDay = true;
        progressbar.maxValue = dayTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDay)
        {
            progressbar.gameObject.SetActive(true);
            if (progressTime < dayTime)
            {
                progressTime += Time.deltaTime;
                progressbar.value = progressTime;
                Color color = new Color(1 - Normalizar(progressTime), 0.5f - Normalizar(progressTime), Normalizar(progressTime), 0.65f);
                backgroundColor.color = color;
            }
            else
            {
                isDay = false;
                progressTime = 0;
                progressbar.gameObject.SetActive(false);
                GameManager.instance.TransitionToSleep();
            }
        }
    }

    private float Normalizar(float num)
    {
        return (num / dayTime);
    }
}
