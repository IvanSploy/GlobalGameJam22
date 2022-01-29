using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpSheep : MonoBehaviour
{
    private SheepBehavior sheepToTake;
    [SerializeField] private Transform pickUpPosition;

    private int sheepsIn = 0;

    private InputController input;

    private bool isPicking;
    
    private void Awake()
    {
        input = new InputController();
        input.Player.Interact.performed += (ctx) => PickUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        print("Ay");
        if (other.gameObject.CompareTag("Sheep") && !isPicking)
        {
            sheepToTake = other.GetComponent<SheepBehavior>();
            PickUp();
            isPicking = true;
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Sheep"))
    //    {
    //        sheepToTake = gameObject.GetComponent<SheepBehavior>();
    //    }
    //}

    void PickUp()
    {
        sheepToTake.Picked();
        sheepToTake.transform.SetParent(pickUpPosition);
        sheepToTake.GetComponent<Collider>().enabled = false;
        sheepToTake.transform.DOLocalMove(Vector3.zero, 0.3f);
        
    }

}
