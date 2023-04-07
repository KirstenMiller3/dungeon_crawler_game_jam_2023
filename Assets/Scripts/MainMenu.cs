using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField] private CanvasGroup _fade;

    private void Start() {
        AudioManager.instance.Play("main");

        _fade.alpha = 1f;
        _fade.DOFade(0f, 1f);
    }

    public void OnStart() {
        _fade.alpha = 1f;
        _fade.DOFade(1f, 1f).OnComplete(LoadScenes);
    }

    private void LoadScenes() {
        SceneManager.LoadScene("Environment_Interactable");
        SceneManager.LoadScene("Kirsten", LoadSceneMode.Additive);
    }
    
    public void OnQuit() {
        Application.Quit();
    }
}
