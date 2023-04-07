using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidMirror : Mirror {
    [SerializeField] private AcceptanceMessage[] _acceptanceObjs;

    private int _index;
    private AdvancedGridMovement _player;

    protected override void Start() {
        base.Start();
        for(int i = 0; i < _acceptanceObjs.Length; i++) {
            _acceptanceObjs[i].gameObject.SetActive(false);
        }
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        _player = PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>();

        _acceptanceObjs[_index].OnPickUp = () => OnClaim(_acceptanceObjs[_index].Type);
        _acceptanceObjs[_index].gameObject.SetActive(true);
    }

    public override void CancelPuzzle() {
        base.CancelPuzzle();

        for(int i = 0; i < _acceptanceObjs.Length; i++) {
            _acceptanceObjs[i].gameObject.SetActive(false);
        }
    }

    public override void CompleteMirror() {
        for(int i = 0; i < _acceptanceObjs.Length; i++) {
            _acceptanceObjs[i].OnPickUp = null;
        }

        _player.Teleport(_player.transform.position, Quaternion.Euler(Vector3.up * -90f));
        base.CompleteMirror();
    }

    private void OnClaim(AcceptanceMessage.AcceptanceType type) {
        _index++;

        switch(type) {
            case AcceptanceMessage.AcceptanceType.Normal:
                break;
            case AcceptanceMessage.AcceptanceType.Weak:
                PlayerManager.Instance.ReduceConditionToOne();
                break;
            case AcceptanceMessage.AcceptanceType.Slow:
                _player.SetSpeedMultiplier(0.5f);
                break;
            case AcceptanceMessage.AcceptanceType.Clumsy:
                _player.SetInvertedMovement(true);
                break;
            case AcceptanceMessage.AcceptanceType.Reset:
                _player.ResetSpeedMultiplier();
                _player.SetInvertedMovement(false);
                PlayerManager.Instance.FullHeal();
                break;
            default:
                break;
        }

        if(_index > _acceptanceObjs.Length - 1) {
            UIManager.Instance.Transition(CompleteMirror);
            return;
        }

        _acceptanceObjs[_index].OnPickUp = () => OnClaim(_acceptanceObjs[_index].Type);
        _acceptanceObjs[_index].gameObject.SetActive(true);
    }
}
