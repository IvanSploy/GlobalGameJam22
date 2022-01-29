using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtEmoji : MonoBehaviour
{
    private Transform camera1;

    private Quaternion initialRot;
    
    
    // Start is called before the first frame update
    void Start()
    {
        camera1 = Camera.main.transform;
        initialRot = transform.localRotation;
    }

    // Update is called once per frame
    void LateUpdate() {
        transform.localRotation = initialRot;
        transform.LookAt(transform.position + camera1.rotation * Vector3.forward, camera1.rotation * Vector3.up);
    }
}

