using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField, Tooltip("Speed of this bullet.")]
    private float _speed = 4f;

    [SerializeField, Tooltip("Normalized direction of this bullet.")]
    private Vector3 _direction = Vector3.zero;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // move the bullet
        Vector3 newPos = transform.position;
        newPos += _direction * (_speed * Time.deltaTime);
        transform.position = newPos;
    }

    public void SetDirection(Vector3 direction) {
        _direction = direction;

        // rotate to face the direction of movement
        transform.LookAt(transform.position + _direction);
    }
}