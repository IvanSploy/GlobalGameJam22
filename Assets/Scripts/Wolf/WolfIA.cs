using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfIA : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator anim;
    private SheepBehavior[] sheepBehaviors;
    private bool locked;
    private bool killed = false;
    public int state;
    public int previousState;

    // Start is called before the first frame update

    public float timeEating = 3;
    public float timeInTrap = 10;

    private SoundManager sM;
    private void Awake()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        anim = transform.parent.GetComponentInChildren<Animator>();
        state = 0;
        ChangeState(0);
        sM = FindObjectOfType<SoundManager>();
    }

    void Start()
    {
        sheepBehaviors = FindObjectsOfType<SheepBehavior>();
        anim.SetBool("Moving", false);
        anim.SetBool("Eating", false);
        sM.Play(0);
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
            EndGame();
            return;
        }
        if (!sheepBehaviors[0])
        {
            EndGame();
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
        anim.SetBool("Moving", true);
    }

    public void GoToMeat()
    {
        Debug.Log(navMeshAgent.SetDestination(transform.parent.GetComponentInChildren<BistecBehaviour>().bistecTarget.transform.position));
        anim.SetBool("Moving", true);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Sheep") && locked == false && state == 0)
        {
            //ESTO ES PARA QUE NO SE COMA A DOS A LA VEZ
            locked = true;
            navMeshAgent.enabled = false;
            Vector3 vel = GetComponentInParent<Rigidbody>().velocity;
            vel.x = 0;
            vel.y = 0;
            GetComponentInParent<Rigidbody>().velocity = vel;
            //METER ANIMACION LOBO
            //CREO CORRUTINA PARA QUE LA OVEJA DESAPAREZCA EN CUANTO TERMINE LA ANIMACION

            StartCoroutine(KillSheep(collider.gameObject));
        }
        if (collider.CompareTag("Bistec") && state == 1)
        {
            //ESTO ES PARA QUE NO SE COMA A DOS A LA VEZ
            locked = true;
            navMeshAgent.enabled = false;
            Vector3 vel = GetComponentInParent<Rigidbody>().velocity;
            vel.x = 0;
            vel.y = 0;
            GetComponentInParent<Rigidbody>().velocity = vel;

            //METER ANIMACION LOBO
            //CREO CORRUTINA PARA QUE LA OVEJA DESAPAREZCA EN CUANTO TERMINE LA ANIMACION

            StartCoroutine(EatBistec(collider.gameObject));
        }
    }

    IEnumerator KillSheep(GameObject sheep)
    {
        anim.SetBool("Moving", false);
        anim.SetBool("Eating", true);
        sheep.GetComponent<NavMeshAgent>().enabled = false;
        sM.Play(1);
        yield return new WaitForSeconds(timeEating);
        anim.SetBool("Eating", false);
        navMeshAgent.enabled = true;
        Destroy(sheep);
        killed = true;
        locked = false;
        GameManager.instance.ovejitasvivas--;
    }
    IEnumerator EatBistec(GameObject bistec)
    {
        anim.SetBool("Moving", false);
        anim.SetBool("Eating", true);
        sM.Play(1);
        yield return new WaitForSeconds(timeEating);
        anim.SetBool("Eating", false);
        navMeshAgent.enabled = true;
        Destroy(bistec);
        locked = false;
        ChangeState(0);
    }

    public void GoToTrap()
    {
        GameObject trap = transform.parent.GetComponentInChildren<TrapBehaviour>().trapTarget;
        navMeshAgent.enabled = false;
        Vector3 pos = trap.transform.position;
        pos.y += 0.5f;
        transform.parent.position = pos;
        Vector3 vel = GetComponentInParent<Rigidbody>().velocity;
        vel.x = 0;
        vel.y = 0; 
        GetComponentInParent<Rigidbody>().velocity = vel;
        anim.SetBool("Moving", false);
        trap.GetComponent<Animator>().SetTrigger("Close");
        locked = true;
        StartCoroutine(WaitForRelease(trap));
    }

    IEnumerator WaitForRelease(GameObject trap)
    {
        yield return new WaitForSeconds(timeInTrap);
        navMeshAgent.enabled = true;
        Destroy(trap);
        locked = false;
        ChangeState(previousState);
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
        StopAllCoroutines();
        locked = false;
        previousState = state;
        state = newState;
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
    }
    #endregion

    public void EndGame()
    {
        anim.SetBool("Moving", false);
        anim.SetBool("Eating", false);
    }
}
