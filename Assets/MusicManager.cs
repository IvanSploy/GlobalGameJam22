using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    [SerializeField] private AudioClip[] songs;
    private float initialVolume;
    private AudioSource audio;
    
    public static MusicManager instance;

    private void Awake() {
        if (instance)
            Destroy(gameObject);
        
        instance = this;
        audio = GetComponent<AudioSource>();
        initialVolume = audio.volume;
        DontDestroyOnLoad(gameObject);
    }

    public void SetSong(int id) {
        audio.DOFade(0, 1).OnComplete((() => {
            audio.clip = songs[id];
            audio.Play();
            audio.DOFade(initialVolume, 1);
        }));
    }
    
    
}
