using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fade;

    private void Start() {
        _fade.alpha = 1f;
        _fade.DOFade(0f, 1f);
    }

    public void OnStart() {
        _fade.alpha = 1f;
        _fade.DOFade(1f, 1f).OnComplete(LoadGame);
    }

    public void LoadGame() {
        SceneManager.LoadScene("Environment_Interactable");
        SceneManager.LoadScene("Kirsten", LoadSceneMode.Additive);    
    }
}
