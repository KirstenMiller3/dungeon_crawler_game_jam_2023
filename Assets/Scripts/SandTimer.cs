using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandTimer : MonoBehaviour {
    [SerializeField] private Renderer[] _renderers;
    [SerializeField] private float _fillTime = 1f;

    public bool IsRunning => _running;

    private float _timer = 0;
    private bool _running = false;

    private int _index = 0;

    private Stack<Renderer> _renderStack = new Stack<Renderer>();


    public void Update() {
        if(!_running) {
            return;
        }

        _timer += Time.deltaTime;

        if(_timer >= _fillTime) {
            _timer = 0f;

            _index++;
        }
    }

    [ContextMenu("Run")]
    public void RunTimer() {
        _running = true;
        _timer = 0f;

        _renderStack = new Stack<Renderer>(_renderers);
    }
}
