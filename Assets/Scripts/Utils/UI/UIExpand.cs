using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIExpand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private float _scaleUpSize = 1.3f;

    private float _scaleTarget = 1f;


    private void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(_scaleTarget, _scaleTarget, 1f), Time.deltaTime * 6f);
    }


    public void OnPointerEnter(PointerEventData eventData) {
        _scaleTarget = _scaleUpSize;
    }

    public void OnPointerExit(PointerEventData eventData) {
        _scaleTarget = 1f;
    }
}

