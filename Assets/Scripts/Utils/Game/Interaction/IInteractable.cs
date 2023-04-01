using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IInteractable {
    string ObjectName { get; }
    KeyCode KeyCode { get; }
    string AnimationBool { get; }
    float InteractionTime { get; }
    Transform Transform { get; }
    //CollectibleBase.CollectibleItemType UnlockItem { get; }
    //int ItemUnlockCount { get; }
    AudioClip AudioClip { get; }

    bool CanInteract();
    void OnUsed();
    void OnInteract();
    void OnCancel();
    void OnUsedAndRestart();
    void OnTriggerEnter(Collider collider);
    void OnTriggerExit(Collider collider);
}
