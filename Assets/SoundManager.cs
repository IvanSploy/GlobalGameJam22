using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private AudioSource audio;
    [SerializeField] private AudioClip[] sfxs; 


    private void Awake() {
        audio = GetComponent<AudioSource>();
    }

    public void Play(int id) {
        audio.PlayOneShot(sfxs[id]);
    }

    
}
