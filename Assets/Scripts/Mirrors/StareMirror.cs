using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StareMirror : Mirror {
    [SerializeField] private float _stareTime = 5f;

    private float _timer;
    private AdvancedGridMovement _player;

    protected override void Start() {
        base.Start();

        _player = PlayerManager.Instance.PlayerTransform.GetComponent<AdvancedGridMovement>();
    }

    private void Update() {
        if(!_hasStarted) {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer >= _stareTime) {
            CompleteMirror();
        }
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();

        UIManager.Instance.FadeToBlack(StartExperience);
        _player.DisableMovement(true);

    }

    private void StartExperience() {
        AudioManager.instance.SetPresence(true);
        AudioManager.instance.Play("presence_1");
        AudioManager.instance.Play("presence_2");
    }

    public override void CompleteMirror() {
        base.CompleteMirror();

        UIManager.Instance.ReturnFade();
        _player.DisableMovement(false);
        AudioManager.instance.SetPresence(false);
        AudioManager.instance.Stop("presence_1");
        AudioManager.instance.Stop("presence_2");
    }

}
