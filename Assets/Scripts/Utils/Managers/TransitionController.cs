using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Milo.Tools;

public class TransitionController : Singleton<TransitionController> {

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _fadeSpeed = 3f;

    private float _targetAlpha = 0f;

    protected override void Awake() {
        base.Awake();
        _canvasGroup.alpha = 1f;
    }

    private void Update() {
        _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, _targetAlpha, Time.deltaTime * _fadeSpeed);
    }

    public void FadeIn() {
        _targetAlpha = 0f;
    }

    public void FadeOut() {
        _targetAlpha = 1f;
    }

    public bool IsTransitioningOut() {
        if(_canvasGroup.alpha < 0.99f) {
            return false;
        }

        return true;
    }
}
