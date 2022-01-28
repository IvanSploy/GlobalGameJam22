using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : MonoBehaviour {
    public UnityEvent onFail;
    [SerializeField] private GameObject[] fences;

    [SerializeField] private int difficulty;
    [SerializeField] private float frequency = 6;
    
    private IEnumerator sheepGenCoroutine;
    private SheepGenerator sheepGenerator;


    private void Awake() {
        sheepGenerator = FindObjectOfType<SheepGenerator>();
        sheepGenCoroutine = SheepGen();
    }

    private void OnEnable() {
        onFail.AddListener(OnFail);
    }
    
    private void OnDisable() {
        onFail.RemoveListener(OnFail);
    }

    private void Start() {
        InitMinigame(6);
    }
    

    public void InitMinigame(int dif) {
        difficulty = dif;
        StartCoroutine(sheepGenCoroutine);

        if (difficulty <= 3) {
            InitFences(1);
            return;
        }
        
        if (difficulty <= 6) {
            InitFences(2);
            return;
        }
        
        if (difficulty <= 6) {
            InitFences(3);
            return;
        }
    }

    void InitFences(int n) {
        foreach (var f in fences) {
            f.SetActive(false);
        }
        
        if (n == 1) {
            fences[1].SetActive(true);
            return;
        }
        
        if (n == 2) {
            fences[0].SetActive(true);
            fences[2].SetActive(true);
            return;
        }
        
        if (n == 3) {
            foreach (var f in fences) {
                f.SetActive(true);
            }
        }
    }

    IEnumerator SheepGen() {
        var spIncrement = 0f;
        var frequencyIncrement = 0f;
        var currentF = frequency;
        var currentS = 300f;
        
        
        while (true) {
            currentF -= frequencyIncrement;
            currentS += spIncrement;

            sheepGenerator.GenerateSheep(Mathf.Clamp(currentS, 0, 600));
            yield return new WaitForSeconds(Mathf.Clamp(currentF, 2, 100));
            
            spIncrement += 15f * (difficulty%3 + 1);
            frequencyIncrement += 0.1f * (difficulty%3 + 1);
        }
    }

    public void OnFail() {
        onFail.Invoke();
    }
}
