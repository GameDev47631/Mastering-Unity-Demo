using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // the Rigid Body physics component of this project
    // since we'll be accessing it a lot, wwe'll store it as a member
    private Rigidbody _rigidbody;

    [SerializeField, Tooltip("How much acceleration is applied to this object when directional input is received.")]
    private float _movementAcceleration = 2;

    [SerializeField, Tooltip("The maximum velocity of this object - keeps the player from moving too fast.")]
    private float _movementVelocityMax = 2;

    [SerializeField, Tooltip("Deceleration when no dorection input is received.")]
    private float _movementFriction = 0.1f;

    [SerializeField, Tooltip("Upwards force applied when Jump key is pressed.")]
    private float _jumpVelocity = 20;

    [SerializeField, Tooltip("Additional gravitational pull.")]
    private float _extraGravity = 40;

    [SerializeField, Tooltip("The bullet projectile prefab to fire.")]
    private GameObject _bulletToSpawn;

    [Tooltip("The direction that the Player is facing.")]
    Vector3 _curFacing = Vector3.zero;

    // Start is called before the first frame update
    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        // get the current speed from the RigidBody physics component
        // grabbing this ensues we retain the gravity speed
        Vector3 curSpeed = _rigidbody.velocity;

        // check to see if any of the keyboard arrows are being pressed
        if (Input.GetKey(KeyCode.RightArrow)) {
            curSpeed.x += _movementAcceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            curSpeed.x -= _movementAcceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            curSpeed.z += _movementAcceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            curSpeed.z -= _movementAcceleration * Time.deltaTime;
        }

        // store the current facing
        // do this after speed is adjusted by arrow keys
        // be before friction is applied
        if (curSpeed.x != 0 && curSpeed.z != 0) {
            _curFacing = curSpeed.normalized;
        }

        // does the player want to jump?
        if (Input.GetKey(KeyCode.Space) && Mathf.Abs(curSpeed.y) < 1) {
            curSpeed.y += _jumpVelocity;
        } else {
            curSpeed.y -= _extraGravity * Time.deltaTime;
        }

        // fire the weapon?
        if (Input.GetKeyDown(KeyCode.Return)) {
            GameObject newBullet = Instantiate(_bulletToSpawn, transform.position, Quaternion.identity);

            Bullet bullet = newBullet.GetComponent<Bullet>();

            if (bullet) {
                bullet.SetDirection(new Vector3(_curFacing.x, 0f, _curFacing.z));
            }
        }

        // if both left and right keys are simultaneously pressed (or not pressed), apply friction
        if (Input.GetKey(KeyCode.LeftArrow) == Input.GetKey(KeyCode.RightArrow)) {
            curSpeed.x -= _movementFriction * curSpeed.x;
        }

        // apply similar friction logic to up and down keys
        if (Input.GetKey(KeyCode.UpArrow) == Input.GetKey(KeyCode.DownArrow)) {
            curSpeed.z -= _movementFriction * curSpeed.z;
        }

        // apply the max speed
        curSpeed.x = Mathf.Clamp(curSpeed.x, _movementVelocityMax * -1, _movementVelocityMax);
        curSpeed.z = Mathf.Clamp(curSpeed.z, _movementVelocityMax * -1, _movementVelocityMax);

        // apply the changed velocity to this object's physics component
        _rigidbody.velocity = curSpeed;
    }

    void OnTriggerEnter(Collider collider) {
        // did we collide with a PickUpItem?
        if (collider.gameObject.GetComponent<PickUpItem>()) {
            // we collided with a valid PickUpItem
            // so let that item know it's been 'Picked Up' by this gameObject
            PickUpItem item = collider.gameObject.GetComponent<PickUpItem>();
            item.onPickedUp(this.gameObject);
        }
    }
}