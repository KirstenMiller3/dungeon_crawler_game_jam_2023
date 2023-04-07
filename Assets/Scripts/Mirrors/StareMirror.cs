using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareMirror : Mirror {
    [SerializeField] private float _stareTime = 5f;

    private float _timer;
    private AdvancedGridMovement _player;
    private bool _complete;

    protected override void Start() {
        base.Start();

        _player = PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>();
    }

    protected override void Update() {
        base.Update();

        if(!_hasStarted) {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer >= _stareTime) {
            Finish();
        }
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        UIManager.Instance.Transition(StartExperience);

        _player.DisableMovement(true);

    }

    private void StartExperience() {
        UIManager.Instance.SetPresence();
        AudioManager.instance.SetPresence(true);
        AudioManager.instance.Play("presence_1");
        AudioManager.instance.Play("presence_2");
    }

    public override void CompleteMirror() {
        UIManager.Instance.EndPresence();
        _player.DisableMovement(false);
        AudioManager.instance.SetPresence(false);
        AudioManager.instance.Stop("presence_1");
        AudioManager.instance.Stop("presence_2");

        base.CompleteMirror();

    }

    private void Finish() {
        if(_complete) {
            return;
        }
        _complete = true;
        UIManager.Instance.Transition(CompleteMirror);
    }

}
