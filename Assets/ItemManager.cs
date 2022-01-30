using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    //Referencias
    public static ItemManager instance;
    public GameObject trap;
    public GameObject bistec;
    public GameObject fence;

    public Transform spawn1;
    public Transform spawn2;

    private void Awake()
    {
        if (instance)
            Destroy(this);
        instance = this;
    }

    public void GenerateItems()
    {
        float random = Random.Range(0f,1f);
        if(random > 0.9f)
        {
            Instantiate(fence, spawn1.position, spawn2.rotation);
            Instantiate(fence, spawn2.position, spawn2.rotation);
        }
        else
        {
            Instantiate(trap, spawn1.position, spawn2.rotation);
            Instantiate(bistec, spawn2.position, spawn2.rotation);
        }
    }

}
