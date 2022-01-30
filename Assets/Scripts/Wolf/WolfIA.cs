using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfIA : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private SheepBehavior[] sheepBehaviors;
    private bool locked;
    private bool killed = false;
    public int state;

    // Start is called before the first frame update

    public float timeEating = 3;
    public float timeInTrap = 10;

    private void Awake()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        state = 0;
    }
    void Start()
    {
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
    }

    private void Update()
    {
        UpdateState(state);
    }

    public void ReloadSheeps()
    {
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
        killed = false;
    }

    private void SearchSheeps()
    {
        if(killed) ReloadSheeps();
        if (sheepBehaviors.Length == 0)
        {
            //SE ACABA EL JUEGO NO HAY MAS BICHOS

            return;
        }
        float distance = Vector3.Distance(sheepBehaviors[0].transform.position, transform.position);
        int numberSheep = 0;
        for (int i = 1; i < sheepBehaviors.Length; i++)
        {
            if (Vector3.Distance(sheepBehaviors[i].transform.position, transform.position) < distance)
            {
                distance = Vector3.Distance(sheepBehaviors[i].transform.position, transform.position);
                numberSheep = i;
            }
        }
        navMeshAgent.destination = sheepBehaviors[numberSheep].transform.position;
    }

    public void GoToMeat()
    {
        Debug.Log(navMeshAgent.SetDestination(transform.parent.GetComponentInChildren<BistecBehaviour>().bistecTarget.transform.position));
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Sheep") && locked == false)
        {
            //ESTO ES PARA QUE NO SE COMA A DOS A LA VEZ
            locked = true;
            
            //METER ANIMACION LOBO
            //CREO CORRUTINA PARA QUE LA OVEJA DESAPAREZCA EN CUANTO TERMINE LA ANIMACION
            
            StartCoroutine(KillSheep(collider.gameObject));
        }
        if (collider.CompareTag("Bistec"))
        {
            //ESTO ES PARA QUE NO SE COMA A DOS A LA VEZ
            locked = true;

            //METER ANIMACION LOBO
            //CREO CORRUTINA PARA QUE LA OVEJA DESAPAREZCA EN CUANTO TERMINE LA ANIMACION

            StartCoroutine(EatBistec(collider.gameObject));
        }
    }

    IEnumerator KillSheep(GameObject sheep)
    {
        sheep.GetComponent<NavMeshAgent>().enabled = false;
        yield return new WaitForSeconds(timeEating);
        Destroy(sheep);
        killed = true;
        locked = false;
    }
    IEnumerator EatBistec(GameObject bistec)
    {
        yield return new WaitForSeconds(timeEating);
        Destroy(bistec);
        locked = false;
        ChangeState(0);
    }

    public void GoToTrap()
    {
        TrapBehaviour trap = transform.parent.GetComponentInChildren<TrapBehaviour>();
        transform.position = trap.transform.position;
        locked = true;
        StartCoroutine(WaitForRelease(trap));
    }

    IEnumerator WaitForRelease(TrapBehaviour trap)
    {
        yield return new WaitForSeconds(timeInTrap);
        Destroy(trap.gameObject);
        locked = false;
        ChangeState(0);
    }

    #region Estados
    void UpdateState(int state)
    {
        switch (state)
        {
            //Cazando
            case 0:
                if (!locked) SearchSheeps();
                break;

            default:
                break;
        }
    }

    public void ChangeState(int newState)
    {
        locked = false;
        switch (state)
        {
            //Comiendo carne
            case 1:
                GoToMeat();
                break;

            //En trampa
            case 2:
                GoToTrap();
                break;

            default:
                break;
        }
        state = newState;
    }
    #endregion
}
