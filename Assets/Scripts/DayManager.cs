using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DayManager : MonoBehaviour
{
    [SerializeField] private float dayTime;
    [SerializeField] private Slider progressbar;
    [SerializeField] private Image backgroundColor;

    private float progressTime;

    public bool isDay;
    // Start is called before the first frame update
    void Start()
    {
        isDay = true;
        progressbar.maxValue = dayTime;
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDay)
        {
            if (progressTime < dayTime)
            {
                progressTime += Time.deltaTime;
                progressbar.value = progressTime;
                backgroundColor.color = Color.yellow;
            }
            else
            {
                isDay = false;
                progressTime = 0;
            }
        }
    }
}
