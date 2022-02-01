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

    private bool v;

    private GameManager gM;

    public bool playing;


    private void Awake() {
        sheepGenerator = FindObjectOfType<SheepGenerator>();
        sheepGenCoroutine = SheepGen();
        gM = FindObjectOfType<GameManager>();
    }

    private void OnEnable() {
        onFail.AddListener(OnFail);
    }
    
    private void OnDisable() {
        onFail.RemoveListener(OnFail);
    }
    
    

    public void InitMinigame(int dif) {
        difficulty = dif;
        sheepGenerator.parentS = new GameObject("parentS");
        sheepGenerator.parentS.transform.SetParent(gameObject.transform);
        playing = true;

        if (difficulty <= 3) {
            InitFences(1);
            v = false;
            StartCoroutine(sheepGenCoroutine);
            return;
        }
        
        if (difficulty <= 5 && difficulty > 3) {
            InitFences(2);
            v = false;
            StartCoroutine(sheepGenCoroutine);
            return;
        }
        
        if (difficulty >= 6) {
            InitFences(3);
            v = true;
            StartCoroutine(sheepGenCoroutine);
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
        float spIncrement = 300f;
        float frequencyIncrement = 5f;
        
        
        while (playing) {
            
            sheepGenerator.GenerateSheep(Mathf.Clamp(spIncrement, 0, 600));
            yield return new WaitForSeconds(Mathf.Clamp(frequencyIncrement, 1f, 100));
            if (!v) {
                spIncrement += 5f * (difficulty%3 + 1);
                frequencyIncrement -= 0.4f;
                print($"$Increased {frequencyIncrement}");
            }
            else {
                spIncrement += 2f * (difficulty%2 + 1);
                frequencyIncrement -= 0.8f;
            }
            
        }
    }

    public void DoReset()
    {
        Destroy(sheepGenerator.parentS);
        playing = false;
    }

    public void OnFail() {
        DoReset();
        gM.TransitionToNight();
    }
}
