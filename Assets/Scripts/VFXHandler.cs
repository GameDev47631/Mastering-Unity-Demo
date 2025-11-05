using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXHandler : MonoBehaviour {

    [SerializeField, Tooltip("Prefab to spawn when hit and destroyed.")]
    GameObject _mainExplosionChunk;

    [SerializeField, Tooltip("Less common prefab when hit and destroyed.")]
    GameObject _secondaryExplosionChunk;

    [SerializeField, Tooltip("Min explosion chunks to spawn.")]
    int _minChunks = 10;

    [SerializeField, Tooltip("Max amount to spawn.")]
    int _maxChunks = 20;

    [SerializeField, Tooltip("Force of explosion.")]
    float _explosionForce = 1500;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void SpawnExplosion() {
        // spawn a random number of the main chunks
        int rand = Random.Range(_minChunks, _maxChunks);
        if (_mainExplosionChunk) {
            for (int i = 0; i < rand; i++) {
                SpawnSubObject(_mainExplosionChunk);
            }
        }

        // now spawn the secondary object
        // (but only half the amount)
        rand /= 2;
        if (_secondaryExplosionChunk) {
            for (int i = 0; i < rand; i++) {
                SpawnSubObject(_secondaryExplosionChunk);
            }
        }
    }

    void SpawnSubObject(GameObject prefab) {
        // get a random point around our object
        // should prevent collision with parent
        Vector3 pos = transform.position;
        pos += Random.onUnitSphere * 0.8f;

        GameObject newObj = Instantiate(prefab, pos, Quaternion.identity);

        /* "Make sure the bullet prefab has the Rigibody component" */
        // give the chunk a random velocity
        Rigidbody rb = newObj.GetComponent<Rigidbody>();
        rb?.AddExplosionForce(_explosionForce, transform.position, 1f);
    }
}
