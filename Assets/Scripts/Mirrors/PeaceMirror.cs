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

    protected override void Update() {
        base.Update();

        if(_hasStarted && IsComplete.Value == false) {
            //Vector3 dir = (PlayerManager.Instance.PlayerTransform.position - _beatinHeart.transform.position);
            UIManager.Instance.UpdateHeartCompass(_beatinHeart.transform.position);
        }
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        AudioManager.instance.SetLowPassOn(true);
        _beatinHeart.SetActive(true);
        UIManager.Instance.ShowHeartCompass(true);
    }

    public override void CancelPuzzle() {
        base.CancelPuzzle();
        _beatinHeart.gameObject.SetActive(false);
        AudioManager.instance.SetLowPassOn(false);
        UIManager.Instance.ShowHeartCompass(false);
    }

    public override void CompleteMirror() {
        base.CompleteMirror();

        UIManager.Instance.ShowHeartCompass(false);
        AudioManager.instance.SetLowPassOn(false);
    }
}
