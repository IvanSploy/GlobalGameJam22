using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {
    [SerializeField] private RectTransform menu;
    [SerializeField] private RectTransform back;
    
    [SerializeField] private GameObject cam;
    public Sprite imageTransition;
    private Vector3 initialRot;
    private Vector3 rot;
    [SerializeField] private MeshRenderer mesh;
    private Material mat;
    
    
    // Start is called before the first frame update
    void Start() {
        initialRot = cam.transform.rotation.eulerAngles;
        rot = new Vector3(-50, 344.59f, 0);
        mat = mesh.material;
        mat.DOFade(0, 0);
    }
    

    public void CreditsIn() {
        cam.transform.DORotate(rot, 1);
        back.DOMoveX(back.rect.width/2, 1);
        menu.DOAnchorPosY(-menu.rect.height, 1);
        mat.DOFade(1, 1);

    }
    
    public void CreditsOut() {
        back.DOMoveX(back.rect.width, 1);
        
        cam.transform.DORotate(initialRot, 1);
        menu.DOAnchorPosY(0, 1);
        
        mat.DOFade(0, 1);
    }

    public void StartLevel()
    {
        SceneTransitioner transitioner = FindObjectOfType<SceneTransitioner>();
        transitioner.SetTitle("Day 1 of 7");
        MusicManager.instance.SetSong(1);
        transitioner.SetImage(imageTransition);
        transitioner.SetBackgroundColor(new Color(0.75f, 1f, 1f));
        transitioner.SetTextColor(Color.black);
        transitioner.SetSubtitle("15 sheeps alive");
        transitioner.OnTransition.AddListener(() => transitioner.GetComponent<LevelLoader>().NextLevel());
        transitioner.StartTransition(2);
    }
}
