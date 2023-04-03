using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConditionMeter : MonoBehaviour {
    [SerializeField] private Image[] _conditionImages;
    [SerializeField] private RectTransform[] _rects;
    [SerializeField] private Color _depletedColor;
    [SerializeField] private Color _fullColor;

    private Tween _tween;

    public void IsInteractingWithMirror(int level) {
        if(_tween != null) {
            _tween.Kill();
        }

        if(level == 0) {
            return;
        }

        _tween = _rects[level - 1].DOShakeAnchorPos(100f, 2f, 10);
    }

    public void StopInteraction() {
        if(_tween == null) {
            return;
        }
        _tween.Restart();
        _tween.Kill();
    }

    public void SetLevel(int level) {
        for(int i = 0; i < _conditionImages.Length; i++) {
            if(i < level) {
                _conditionImages[i].color = _fullColor;
            }
            else {
                _conditionImages[i].color = _depletedColor;
            }
        }
    }
}
