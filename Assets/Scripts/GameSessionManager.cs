using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviour {

    [Tooltip("Remaining player lives.")]
    private int _playerLives = 3;

    [SerializeField, Tooltip("Where the player will respawn.")]
    private Transform _respawnLocation;

    [SerializeField, Tooltip("Object to display when the game is over.")]
    private GameObject _gameOverObj;

    [SerializeField, Tooltip("Title Menu countdown after the game is over.")]
    private float _returnToMenuCountdown = 0;

    static public GameSessionManager Instance;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (_returnToMenuCountdown > 0) {
            _returnToMenuCountdown -= Time.deltaTime;

            if (_returnToMenuCountdown < 0) {
                SceneManager.LoadScene("TitleMenu");
            }
        }
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

            _gameOverObj.SetActive(true);
            _returnToMenuCountdown = 4;

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

    public int GetCoins() {
        return PickUpItem.s_objectsCollected;
    }

    public int GetLives() {
        return _playerLives;
    }
}
