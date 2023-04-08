using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AngrySoul : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    [SerializeField] private Animator _animator;

    public System.Action OnDestroyed;
    public System.Action OnHitTarget;

    private Transform _target;
    private Vector3 _scale;

    private Tween _activeMoveTween;
    private Tween _activeScaleTween;

    private bool _isClicked = false;


    private float _speed;

    private void Start() {
        _scale = transform.localScale;
    }

    public void SetUp(Transform target, float speed) {
        _target = target;
        _speed = speed;
        _activeMoveTween = transform.DOMove(target.position, _speed, true).SetEase(Ease.Linear).OnComplete(OnHitCenter);
    }
    
    private void OnHitCenter() {
        OnHitTarget?.Invoke();
        Explode();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        _activeScaleTween = transform.DOScale(_scale * 1.3f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData) {
        transform.DOScale(_scale, 0.5f);
    }

    public void OnPointerDown(PointerEventData eventData) {
        if(_isClicked) {
            return;
        }

        _isClicked = true;

        _activeMoveTween.Kill();
        _activeScaleTween.Kill();
        OnDestroyed?.Invoke();
        Explode();
    }

    public void Explode() {
        _animator.SetTrigger("Kill");
        Destroy(gameObject, 0.3f);
    }
}
