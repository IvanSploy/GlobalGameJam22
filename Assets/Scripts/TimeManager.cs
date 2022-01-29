using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float DayTime;
    [SerializeField] private float NightTime;
    private bool day;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetDay()
    {
        
    }
    private void SetNight()
    {
        
    }
    private void ChangeDayNight()
    {
        day = !day;
        if(day)
        {
            SetDay();
        }
        else
        {
            SetNight();
        }
    }
}
