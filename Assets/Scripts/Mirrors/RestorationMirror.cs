using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorationMirror : Mirror {
    [SerializeField] private GameObject[] _apples;
    [SerializeField] private HungerMonster _hungerMonster;
    [SerializeField] private Transform _playerTeleport;

    protected override void Start() {
        base.Start();

        _hungerMonster.Reset();
        _hungerMonster.OnComplete = OnComplete;

        for(int i = 0; i < _apples.Length; i++) {
            _apples[i].gameObject.SetActive(false);
        }
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        for(int i = 0; i < _apples.Length; i++) {
            _apples[i].gameObject.SetActive(true);
        }
    }

    public override void CancelPuzzle() {
        base.CancelPuzzle();

        for(int i = 0; i < _apples.Length; i++) {
            _apples[i].gameObject.SetActive(false);
        }

        _hungerMonster.Reset();
    }

    private void OnComplete() {
        UIManager.Instance.Transition(CompleteMirror);
    }

    public override void CompleteMirror() {
        PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>().Teleport(_playerTeleport.position, _playerTeleport.rotation);
        base.CompleteMirror();
    }

}
