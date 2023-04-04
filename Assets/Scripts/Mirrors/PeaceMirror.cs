using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeaceMirror : PresentMirror {
    [SerializeField] private GameObject _beatinHeart;

    protected override void Start() {
        base.Start();
        _beatinHeart.gameObject.SetActive(false);
        AudioManager.instance.SetLowPassOn(false);
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        AudioManager.instance.SetLowPassOn(true);
        _beatinHeart.SetActive(true);
    }

    public override void CancelPuzzle() {
        base.CancelPuzzle();
        _beatinHeart.gameObject.SetActive(false);
        AudioManager.instance.SetLowPassOn(false);
    }

    public override void CompleteMirror() {
        base.CompleteMirror();

        AudioManager.instance.SetLowPassOn(false);
    }
}
