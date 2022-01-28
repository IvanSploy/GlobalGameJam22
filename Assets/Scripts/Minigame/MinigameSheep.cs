using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MinigameSheep : MonoBehaviour {
    [SerializeField] private bool grounded = true;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private MinigameManager minigameManager;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        minigameManager = FindObjectOfType<MinigameManager>();
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0) && grounded) {
            rb.AddForce(Vector2.up * jumpForce);
            grounded = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate() {
        rb.velocity = new Vector2(0, rb.velocity.y);
        rb.AddForce(Vector2.right * speed);
  
    }
    

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("MinigameGround")) {
            if (!grounded) {
                grounded = true;
            }

            return;
        }
        
        if (other.gameObject.CompareTag("MinigameFence")) {
            minigameManager.OnFail();
            return;
        }
        
        if (other.gameObject.CompareTag("Destroyer")) {
            Destroy(gameObject);
            return;
        }
    }

    public void InitSheep(float sp) {
        speed = sp;
    }
    
}
