using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void OpenScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}