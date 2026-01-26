using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsForceZone : MonoBehaviour {
    
    [SerializeField, Tooltip("Force applied to any hit RigidBody object.")]
    float _forceToApply = 1;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void Awake() {
        CapsuleCollider c = GetComponent<CapsuleCollider>();
        if (c) {
            c.isTrigger = true;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb) {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void OnTriggerStay(Collider collider) {
        GameObject hitObj = collider.gameObject;

        if (hitObj != null) {
            Rigidbody rb = hitObj.GetComponent<Rigidbody>();

            // get the direction of the Y axis
            Vector3 dir = transform.up;

            // apply directional force to this object
            rb.AddForce(dir * _forceToApply);
        }
    }
}
