using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fade;

    private void Start() {
        _fade.alpha = 1f;
        _fade.DOFade(0f, 1f);
    }

    public void OnStart() {
        _fade.alpha = 1f;
        _fade.DOFade(1f, 1f).OnComplete(MainMenu);
    }

    public void MainMenu() {
        AudioManager.instance.Stop("rain");
        SceneManager.LoadScene("MainMenu");
    }
    

}
