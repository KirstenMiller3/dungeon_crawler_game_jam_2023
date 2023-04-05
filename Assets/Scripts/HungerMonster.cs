using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerMonster : MonoBehaviour {
    [SerializeField] private int _numberOfApplesNeeded = 3;
    [SerializeField] private Texture2D[] _sprites;
    [SerializeField] private Renderer _renderer;

    public System.Action OnComplete;

    private int _currentApples;

    public void Feed() {
        _currentApples++;

        if(_currentApples < _sprites.Length) {
            _renderer.material.mainTexture = _sprites[_currentApples];
        }

        if(_currentApples >= _numberOfApplesNeeded) {
            gameObject.SetActive(false);
            OnComplete?.Invoke();
        }
    }

    public void Reset() {
        _currentApples = 0;
        _renderer.material.mainTexture = _sprites[_currentApples];
    }
}
