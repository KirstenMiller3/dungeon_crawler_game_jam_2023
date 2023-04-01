using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuController : MonoBehaviour
{
    [SerializeField] private string _firstLevel = "SampleScene";
    [SerializeField] private TransitionController _transition;
    [SerializeField] private AudioClip _music;

    private bool _isTransitioning = false;

    private void Start() {
        SettingsManager.Instance.SetMenuCursor();
        AudioController.Instance.SetAmbienceSource(false);
        AudioController.Instance.FadeIn();
        AudioController.Instance.SetMusicClip(_music);
    }

    public void OnClickStart() {
        _transition.FadeOut();
        _isTransitioning = true;
        AudioController.Instance.FadeOut();
    }

    public void Update()
    {
        if (!_transition.IsTransitioningOut())
        {
            _isTransitioning = true;
        }
        else
        {
            if (_isTransitioning == true)
            {
                _isTransitioning = false;
                SceneManager.LoadScene(_firstLevel);
            }
        }
    }
}
