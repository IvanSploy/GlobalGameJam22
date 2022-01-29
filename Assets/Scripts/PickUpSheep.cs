using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpSheep : MonoBehaviour
{
    [SerializeField] private SheepBehavior sheepToTake;
    [SerializeField] private Transform pickUpPosition;
    [SerializeField] private Animator anim;

    private int sheepsIn = 0;

    private InputController input;

    private bool isPicking;

    private int count = 0;

    [SerializeField] private Vector3 throwForce;
    private PlayerController playerController;
    private VirtualButton interact;

    private void Awake()
    {
        input = new InputController();
        interact = FindObjectOfType<VirtualButton>();
        interact.OnClick.AddListener(Interact);
        input.Player.Interact.started += (ctx) => Interact();
        playerController = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Sheep")) {
            count++;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Sheep") && !isPicking)
        {
            sheepToTake = other.gameObject.GetComponent<SheepBehavior>();
        }
    }
    
    private void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Sheep")) {
            count--;
        }
    }


    void Interact() {
        if (isPicking) {
            Throw();
        }
        else {
            PickUp();
        }
    }
    
    
    void PickUp() {
        if (!sheepToTake || isPicking || count <= 0) return;

        if (sheepToTake.recover != null) {
            StopCoroutine(sheepToTake.recover);
        }
        sheepToTake.Picked();
        sheepToTake.transform.SetParent(pickUpPosition);
        sheepToTake.GetComponent<Collider>().enabled = false;
        sheepToTake.transform.DOLocalMove(Vector3.zero, 0.3f);
        isPicking = true;
        count--;
        anim.SetBool("Picking", true);
    }

    void Throw() {
        sheepToTake.transform.SetParent(null);
        sheepToTake.Released();
        
        sheepToTake.GetComponent<Collider>().enabled = true;

        sheepToTake.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce.x +  transform.up * throwForce.y);
        isPicking = false;
        anim.SetBool("Picking", false);
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
