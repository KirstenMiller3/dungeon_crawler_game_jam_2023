using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStart() {
        SceneManager.LoadScene("Environment_Interactable");
        SceneManager.LoadScene("Kirsten", LoadSceneMode.Additive);
    }
    
    public void OnQuit() {
        Application.Quit();
    }
}
