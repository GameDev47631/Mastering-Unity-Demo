using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // the Rigid Body physics component of this project
    // since we'll be accessing it a lot, we'll store it as a member
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

    [SerializeField, Tooltip("Are we on the ground?")]
    private bool _isGrounded = false;

    [SerializeField, Tooltip("The player's main collision shape.")]
    Collider _myCollider = null;

    [Tooltip("The direction that the Player is facing.")]
    Vector3 _curFacing = /* Vector3.zero */ new Vector3(1, 0, 0);

    // store whether or not input was received this frame
    bool _moveInput = false;

    // the animator controller for this object
    Animator _myAnimator;

    void Start() {
        _rigidbody = GetComponent<Rigidbody>();
        _myCollider = GetComponent<Collider>();
        _myAnimator = GetComponent<Animator>();
    }

    void Update() {
        // get the current speed from the RigidBody physics component
        // grabbing this ensues we retain the gravity speed
        Vector3 curSpeed = _rigidbody.velocity;

        // reset move input
        _moveInput = false;

        // check to see if any of the keyboard arrows are being pressed
        // if so, adjust the speed of the player
        // also store the facing based on the keys being pressed
        if (Input.GetKey(KeyCode.RightArrow)) {
            _moveInput = true;
            curSpeed.x += _movementAcceleration * Time.deltaTime;
            _curFacing.x = 1;
            _curFacing.z = 0;
        }

        if (Input.GetKey(KeyCode.LeftArrow)) {
            _moveInput = true;
            curSpeed.x -= _movementAcceleration * Time.deltaTime;
            _curFacing.x = -1;
            _curFacing.z = 0;
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            _moveInput = true;
            curSpeed.z += _movementAcceleration * Time.deltaTime;
            _curFacing.x = 0;
            _curFacing.z = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow)) {
            _moveInput = true;
            curSpeed.z -= _movementAcceleration * Time.deltaTime;
            _curFacing.x = 0;
            _curFacing.z = -1;
        }

        /* "Although the book stated to delete this code, on page 152, I decided to archive it instead."
         *  // store the current facing
         *  // do this after speed is adjusted by arrow keys
         *  // be before friction is applied
         *  if (curSpeed.x != 0 && curSpeed.z != 0) {
         *      _curFacing = curSpeed.normalized;
         *  } */

        // does the player want to jump?
        if (Input.GetKeyDown(KeyCode.Space) && /* Mathf.Abs(curSpeed.y) < 1 */ CalcIsGrounded()) {
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

        /* "For some reason, the 'x' & 'z' values had to be negated.
            Otherwise, the hero would move around backwards; page 213." */
        // set rotation based on facing
        transform.LookAt(transform.position + new Vector3(-_curFacing.x, 0f, -_curFacing.z));

        UpdateAnimation();
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

    /// <summary>
    /// Check below the player object.
    /// If they're standing on a solid object, they can Jump
    /// and perform other actions not available in mid-air.
    /// </summary>
    bool CalcIsGrounded() {
        float offset = 0.1f;

        Vector3 pos = _myCollider.bounds.center;
        pos.y = _myCollider.bounds.min.y - offset;

        _isGrounded = Physics.CheckSphere(pos, offset);

        return _isGrounded;
    }

    void UpdateAnimation() {
        if (_myAnimator == null) {
            return;
        }

        if (_moveInput) {
            _myAnimator.Play("Run");
        } else {
            _myAnimator.Play("Idle");
        }
    }
}
