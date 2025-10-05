using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour {

    [SerializeField, Tooltip("The maximum health of this object.")]
    private float _healthMax = 10;

    [SerializeField, Tooltip("The current health of this object.")]
    private float _healthCur = 10;

    [SerializeField, Tooltip("Seconds of damage immunity after being hit.")]
    private float _invincibilityFramesMax = 1;

    [SerializeField, Tooltip("Remaining seconds of immunity after being hit.")]
    private float _invincibilityFramesCur = 0;

    [SerializeField, Tooltip("Is this object dead?")]
    private bool _isDead = false;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        // decrement the invincibility timer, if necessary
        if (_invincibilityFramesCur > 0) {
            _invincibilityFramesCur -= Time.deltaTime;

            if (_invincibilityFramesCur < 0) {
                _invincibilityFramesCur = 0;
            }
        }

        // handle death
        if (IsDead()) {
            if (GetComponent<PlayerController>()) {
                GameSessionManager.Instance.onPlayerDeath(gameObject);
            } else {
                GameObject.Destroy(gameObject);
            }
        }

        // handle visibility
        if (GetComponent<MeshRenderer>()) {
            if (_invincibilityFramesCur > 0) {
                // toggle rendering on/off
                if (GetComponent<MeshRenderer>().enabled == true) {
                    GetComponent<MeshRenderer>().enabled = false;
                } else {
                    GetComponent<MeshRenderer>().enabled = true;
                }
            } else {
                GetComponent<MeshRenderer>().enabled = true;
            }
        }

        // toggle camera shake for the player
        if (GetComponent<PlayerController>()) {
            CameraShake camshake = Camera.main.GetComponent<CameraShake>();

            if (camshake) {
                camshake.enabled = (bool)(_invincibilityFramesCur > 0);
            }
        }
    }

    public float AdjustCurHealth(float change) {
        // leave early if we've just been hit and we're trying to apply damage
        if (_invincibilityFramesCur > 0) {
            return _healthCur;
        }

        // adjust the health
        _healthCur += change;

        // check for health limits
        if (_healthCur <= 0) {
            // this object has died, so start the process to destroy it
            onDeath();
        } else if (_healthCur >= _healthMax) {
            // this object has more health than it should
            // so cap it to its max
            _healthCur = _healthMax;
        }

        // should we be invincible after a hit?
        if (change < 0 && _invincibilityFramesMax > 0) {
            _invincibilityFramesCur = _invincibilityFramesMax;
        }

        return _healthCur;
    }

    void onDeath() {
        if (_healthCur > 0) {
            Debug.Log(gameObject.name + " set as dead before health reached 0.");
        }

        _isDead = true;
    }

    public bool IsDead() {
        return _isDead;
    }

    public void Reset() {
        _isDead = false;
        _healthCur = _healthMax;
        _invincibilityFramesCur = 0;
    }

    public float GetHealthCur() {
        return _healthCur;
    }

    public float GetHealthMax() {
        return _healthMax;
    }
}