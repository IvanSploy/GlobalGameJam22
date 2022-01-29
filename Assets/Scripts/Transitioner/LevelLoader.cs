using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    //Referencias
    public static LevelLoader instance;

    int level = 0;

    private void Awake()
    {
        if (instance)
            Destroy(this);
        instance = this;
    }

    public void ChangeScene(int level)
    {
        this.level = level;
        SceneManager.LoadScene(level);
    }
    public void NextLevel()
    {
        level++;
        if (level >= SceneManager.sceneCountInBuildSettings) level = 0;
        SceneManager.LoadScene(level);
    }
    public void PreviousLevel()
    {
        level--;
        if (level < 0) level = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(level);
    }
}
