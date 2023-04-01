
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBase : MonoBehaviour, IInteractable {
    [SerializeField] private string _id;
    [SerializeField] private string _objectName;
    [SerializeField] private KeyCode _keyCode;
    [SerializeField] private string _animationBool;
    [SerializeField] private float _interactionTime;
    [SerializeField] private AudioClip _audioClip;

    //[Header("Unlocked by")]
    //[SerializeField] protected CollectibleBase.CollectibleItemType _unlockItem;
    //[SerializeField] protected int _itemUnlockCount = 0;

    public string ObjectName => _objectName;
    public KeyCode KeyCode => _keyCode;

    public string AnimationBool => _animationBool;

    public float InteractionTime => _interactionTime;

    public Transform Transform => transform;

    //public CollectibleBase.CollectibleItemType UnlockItem => _unlockItem;
    //public int ItemUnlockCount => _itemUnlockCount;

    public AudioClip AudioClip => _audioClip;

    protected GameObject _playerObject;

    protected virtual void Start() {
        //if(GameStateManager.Instance != null && GameStateManager.Instance.AmIUsed(_id)) {
        //    OnUsedAndRestart();
        //}
    }

    public virtual bool CanInteract() {
        return true;
    }

    public virtual void OnInteract() {
    }

    public virtual void OnUsed()  {
        _playerObject.GetComponent<PlayerInteract>().OutOfVicinity();
        //if(GameStateManager.Instance != null) {
        //    GameStateManager.Instance.AddUsedInteractable(_id);
        //}
    }

    public virtual void OnCancel() {

    }

    public virtual void OnTriggerEnter(Collider collider) {
        if(collider.tag != "Player") {
            return;
        }

        collider.GetComponent<PlayerInteract>().InVicinityOfInteractable(this);
        _playerObject = collider.gameObject;
    }

    public virtual void OnTriggerExit(Collider collider) {
        if(collider.tag != "Player") {
            return;
        }

        collider.GetComponent<PlayerInteract>().OutOfVicinity();
        _playerObject = null;
    }

    public virtual void OnUsedAndRestart() {

    }

    [ContextMenu("Generate ID")]
    public void OnGenerateID() {
        _id = Guid.NewGuid().ToString();
    }

    protected virtual void OnDrawGizmos() {
        if(string.IsNullOrEmpty(_id)) {
            Gizmos.color = Color.red;
        }
        else {
            Gizmos.color = Color.green;
        }

        Gizmos.DrawSphere(new Vector3(transform.position.x, 2f, transform.position.z), 0.5f);
    }
}
