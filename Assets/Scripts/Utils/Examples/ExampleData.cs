using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milo.Tools;
public class ExampleData : Singleton<ExampleData> {

    public Observable<int> _test = new Observable<int>();

    private float _timer = 0;

    public void Update() {
        _timer += Time.deltaTime;
        if(_timer >= 1) {
            _timer = 0;
            _test.Value++;
        }
    }

}
