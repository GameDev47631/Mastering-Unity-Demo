using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    [SerializeField, Tooltip("The object to follow.")]
    private GameObject _camTarget;

    [SerializeField, Tooltip("Target offset.")]
    private Vector3 _targetOffset;

    [SerializeField, Tooltip("The height off the ground to follow from.")]
    private float _camHeight = 9;

    [SerializeField, Tooltip("The distance from the target to follow from.")]
    private float _camDistance = -16;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        // make sure we have a valid target
        if (!_camTarget) {
            return;
        }

        // use the targeet object and offsets to calculate our target position
        Vector3 targetPos = _camTarget.transform.position;
        targetPos += _targetOffset;
        targetPos.y += _camHeight;
        targetPos.z += _camDistance;

        // move camera towards target position
        Vector3 camPos = transform.position;
        transform.position = Vector3.Lerp(camPos, targetPos, Time.deltaTime * 5.0f);
    }
}
