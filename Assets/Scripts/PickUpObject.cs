using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{
    [SerializeField] private SheepBehavior sheepToTake;
    [SerializeField] private GameObject objectToTake;
    [SerializeField] private GameObject objectTaken;
    [SerializeField] private Transform pickUpPosition;
    [SerializeField] private Animator anim;

    private int sheepsIn = 0;

    private InputController input;

    private bool isPicking;
    private bool isSheep;

    private int count = 0;

    [SerializeField] private Vector3 throwForce;
    [SerializeField] private Vector3 leaveForce;

    private void Awake()
    {
        input = new InputController();
    }

    private void Start()
    {
        input.Player.Interact.started += (ctx) => Interact();
        isSheep = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Sheep")) {
            count++;
        }
        if (other.gameObject.CompareTag("Trap") || other.gameObject.CompareTag("Bistec") || other.gameObject.CompareTag("Fence"))
        {
            objectToTake = other.gameObject;
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

        if (other.gameObject.CompareTag("Trap") || other.gameObject.CompareTag("Bistec") || other.gameObject.CompareTag("Fence"))
        {
            if(objectToTake.Equals(other.gameObject)) objectToTake = null;
        }
    }


    void Interact() {
        if (isPicking) {
            if (isSheep) Throw();
            else Leave();
        }
        else {
            PickUp();
        }
    }
    
    
    void PickUp() {
        if (objectToTake)
        {
            isSheep = false;
            isPicking = true;
            objectToTake.transform.SetParent(pickUpPosition);
            objectToTake.GetComponent<Collider>().enabled = false;
            objectToTake.transform.DOLocalMove(Vector3.zero, 0.3f);
            objectToTake.GetComponent<Rigidbody>().isKinematic = true;
            objectTaken = objectToTake;
            anim.SetBool("Picking", true);
        }
        else
        {
            if (!sheepToTake || isPicking || count <= 0) return;

            isSheep = true;
            if (sheepToTake.recover != null) {
                sheepToTake.StopRecover();
            }
            sheepToTake.Picked();
            sheepToTake.transform.SetParent(pickUpPosition);
            sheepToTake.GetComponent<Collider>().enabled = false;
            sheepToTake.transform.DOLocalMove(Vector3.zero, 0.3f);
            isPicking = true;
            count--;
            anim.SetBool("Picking", true);
        }
    }

    void Throw() {
        isSheep = false;
        sheepToTake.transform.SetParent(null);
        sheepToTake.Released();
        
        sheepToTake.GetComponent<Collider>().enabled = true;

        sheepToTake.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce.x +  transform.up * throwForce.y);
        isPicking = false;
        anim.SetBool("Picking", false);
    }

    void Leave()
    {
        objectTaken.transform.SetParent(null);
        objectTaken.GetComponent<Collider>().enabled = true;
        objectTaken.GetComponent<Rigidbody>().isKinematic = false;
        objectTaken.GetComponent<Rigidbody>().AddForce(transform.forward * leaveForce.x + transform.up * leaveForce.y);
        isPicking = false;
        if (objectToTake.Equals(objectTaken)) objectToTake = null;
        objectTaken = null;
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
