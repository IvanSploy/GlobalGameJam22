using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MinigameSheep : MonoBehaviour {
    [SerializeField] private bool grounded = true;
    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private MinigameManager minigameManager;

    private InputController input;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        minigameManager = FindObjectOfType<MinigameManager>();

        input = new InputController();
        
        input.Player.Minigame.performed += (ctx) => OnJump();
        
    }

    private void OnJump() {
        if (grounded) {
            rb.AddForce(Vector2.up * jumpForce);
            grounded = false;
            transform.DORotate(new Vector3(0, 0, 20), 0.3f);
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
                transform.DORotate(Vector3.zero, 0.2f);
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
    
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
    
}
