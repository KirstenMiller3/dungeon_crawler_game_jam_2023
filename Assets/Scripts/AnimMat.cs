using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimMat : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Texture2D[] _frames;
    [SerializeField] private float _frameRate = 12;

    private int _currentFrame = 0;
    private float _timer;

    void Update() {
        _timer += Time.deltaTime;

        if(_timer > 1 / _frameRate) {
            _renderer.material.mainTexture = _frames[_currentFrame];
            _currentFrame++;
            _timer = 0f;

            if(_currentFrame >= _frames.Length - 1) {
                _currentFrame = 0;
            }
        }
    }
}
