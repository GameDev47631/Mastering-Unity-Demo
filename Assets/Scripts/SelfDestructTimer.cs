using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructTimer : MonoBehaviour {

    [SerializeField, Tooltip("Seconds until this object self-destructs.")]
    float _countdownTimer = 1.5f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        _countdownTimer -= Time.deltaTime;
        if (_countdownTimer <= 0) {
            Destroy(gameObject);
        }
    }
}
