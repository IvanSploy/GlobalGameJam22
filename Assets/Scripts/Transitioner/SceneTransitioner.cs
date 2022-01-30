using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneTransitioner : MonoBehaviour
{
    //Referencias
    public TMP_Text info;
    public static SceneTransitioner instance;
    private Animator anim;

    //Eventos
    public AnimationClip[] delayClips;
    public UnityEvent OnTransition;

    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        instance = this;
        DontDestroyOnLoad(gameObject);

        anim = GetComponent<Animator>();
    }

    /*private void Start()
    {
        StartTransition("Hola chami", 1);
    }*/

    public void StartTransition(string text, float delay)
    {
        SetText(text);
        StartTransition(delay);
    }

    public void StartTransition(string text)
    {
        anim.SetTrigger("ChangeState");
        SetText(text);
        StartCoroutine(WaitForTransition(0));
    }
    public void StartTransition(float delay)
    {
        anim.SetTrigger("ChangeState");
        StartCoroutine(WaitForTransition(delay));
    }

    public void SetText(string text)
    {
        info.SetText(text);
    }

    private IEnumerator WaitForTransition(float delay)
    {
        foreach(AnimationClip ac in delayClips)
             delay += ac.length;
        yield return new WaitForSeconds(delay);
        OnTransition.Invoke();
        EndTransition();
    }

    public void EndTransition()
    {
        anim.SetTrigger("ChangeState");
    }
}
