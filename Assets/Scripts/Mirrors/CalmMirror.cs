using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalmMirror : PresentMirror {
    [SerializeField] private GameObject _solutionObj;

    protected override void Start() {
        base.Start();
        _solutionObj.SetActive(false);
    }

    public override void OnPressStartPuzzle() {
        base.OnPressStartPuzzle();
        _solutionObj.SetActive(true);
    }

    public override void CancelPuzzle() {
        base.CancelPuzzle();
        _solutionObj.SetActive(false);
    }
}
