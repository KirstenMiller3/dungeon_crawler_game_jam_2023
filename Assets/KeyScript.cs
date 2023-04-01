using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour {
    private Inventory Inventory;
    // Start is called before the first frame update
    void Start() {
        Inventory = GameObject.FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && Vector3.Distance(transform.position, Inventory.transform.position) <= 2) {
            Debug.Log("Picked up key");
            Inventory.HasKey = true;
        }
    }
}
