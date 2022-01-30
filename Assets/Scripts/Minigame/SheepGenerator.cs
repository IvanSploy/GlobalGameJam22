using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepGenerator : MonoBehaviour {
    [SerializeField] private GameObject sheep;
    [SerializeField] public GameObject parentS;
    
    public void GenerateSheep(float speed) {
        var sheepInstance = Instantiate(sheep, transform);
        sheepInstance.GetComponent<MinigameSheep>().InitSheep(speed);
        sheepInstance.transform.SetParent(parentS.transform);
    }
}
