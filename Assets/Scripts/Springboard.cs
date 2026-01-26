using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : MonoBehaviour {

    [SerializeField, Tooltip("Velocity change on the Y axis.")]
    float _upwardsForce = 2000f;

    /* "Although this didn't work within the 'HealthModifier' script,
     *  there does not seem to be any problems here;
     *  this code is located between pages 178-179." */
    private void OnCollisionEnter(Collision collision) {
        GameObject hitObj = collision.gameObject;

        if (hitObj != null) {
            Rigidbody rb = hitObj.GetComponent<Rigidbody>();
            rb?.AddForce(0, _upwardsForce, 0);
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }
}
