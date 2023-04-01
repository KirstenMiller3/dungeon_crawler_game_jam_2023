using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Milo.Tools {

    public delegate void ObservableChanged<T>(T prev, T cur);

    public class Observable<T> {

        private event ObservableChanged<T> OnChange;
        private T _value;

        public T Value {
            get { return _value; }
            set {
                var oldValue = _value;
                if(!EqualityComparer<T>.Default.Equals(_value, value)) {
                    _value = value;
                    RaiseChangedEvent(oldValue, _value);
                }
            }
        }

        public void Subscribe(ObservableChanged<T> listener, bool invokeImmediately = true) {
            OnChange += listener;
            if(invokeImmediately) {
                listener?.Invoke(Value, Value);
            }
        }

        public void Unsubscribe(ObservableChanged<T> listener) {
            OnChange -= listener;
        }

        private void RaiseChangedEvent(T prev, T cur) {
            if(OnChange != null) {
                OnChange.Invoke(prev, cur);
            }
        }
    }

}
