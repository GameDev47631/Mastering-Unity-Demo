using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour {
    [SerializeField, Tooltip("The speed that this object rotates at.")]
    private float _rotationSpeed = 5;

    public static int s_objectsCollected = 0;

    void Start() {
        /* "Though not mentioned within Chapter 4, this line of text
         *  ensures that the number of items collected don't stack up
         *  after playing a new game." */
        s_objectsCollected = 0;
    }

    void Update() {
        // grab the current rotation, increment it, and re-apply it
        Vector3 newRotation = transform.eulerAngles;
        newRotation.y += (_rotationSpeed * Time.deltaTime);
        transform.eulerAngles = newRotation;
    }

    public void onPickedUp(GameObject whoPickedUp) {
        // show the collection count in the console window
        s_objectsCollected++;
        Debug.Log(s_objectsCollected + " item(s) picked up.");

        // destroy the item
        Destroy(gameObject);
    }
}