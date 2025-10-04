using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionManager : MonoBehaviour {

    [Tooltip("Remaining player lives.")]
    private int _playerLives = 3;

    [SerializeField, Tooltip("Where the player will respawn.")]
    private Transform _respawnLocation;

    static public GameSessionManager Instance;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    void Awake() {
        // the GameSessionManager is a Singleton
        // store this as the instacne of this object
        Instance = this;
    }

    public void onPlayerDeath(GameObject player) {
        if (_playerLives <= 0) {
            // player is out of lives
            GameObject.Destroy(player.gameObject);
            Debug.Log("Game Over!");
        } else {
            // use a life to respawn the player
            _playerLives--;

            // reset health
            HealthManager playerHealth = player.GetComponent<HealthManager>();

            if (playerHealth) {
                playerHealth.Reset();
            }

            if (_respawnLocation) {
                player.transform.position = _respawnLocation.position;
            }

            Debug.Log("Player Lives Remaining: " + _playerLives);
        }
    }
}
