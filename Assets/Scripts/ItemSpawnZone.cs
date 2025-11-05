using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnZone : MonoBehaviour {

    [SerializeField, Tooltip("Prefab to spawn in this zone.")]
    private GameObject _itemToSpawn;

    [SerializeField, Tooltip("Number of items to spawn.")]
    private float _itemCount = 30;

    [SerializeField, Tooltip("The area to spawn these items.")]
    private BoxCollider _spawnZone;

    [SerializeField, Tooltip("How should these objects be organized when spawned?")]
    private SpawnShape _spawnShape;
    private enum SpawnShape {
        Random, Circle, Grid, Count
    }

    [SerializeField, Tooltip("Speed that this group of objects will rotate.")]
    private Vector3 _rotationSpeed;

    void Start() {
        // instantiate the objects according to spawn shape
        if (_spawnShape == SpawnShape.Circle) {
            SpawnObjectsInCircle();
        } else {
            /* "The for loop and its coment were originally at the beginning of the 'Start()' function." */
            // spawn the items within this area
            for (int i = 0; i < _itemCount; i++) {
                SpawnItemAtRandomPosition();
            }
        }    
    }

    void Update() {
        // calculate the new rotation
        // by taking the old rotation
        // and applying the speed parameter
        Vector3 newRot = transform.localEulerAngles;
        newRot += _rotationSpeed * Time.deltaTime;
        transform.localEulerAngles = newRot;
    }

    void SpawnItemAtRandomPosition() {
        Vector3 randomPos;

        // randomize location based on the size of the associated BoxCollider
        randomPos.x = Random.Range(_spawnZone.bounds.min.x, _spawnZone.bounds.max.x);
        randomPos.y = Random.Range(_spawnZone.bounds.min.y, _spawnZone.bounds.max.y);
        randomPos.z = Random.Range(_spawnZone.bounds.min.z, _spawnZone.bounds.max.z);

        // spawn the item prefab at this position
        Instantiate(_itemToSpawn, randomPos, Quaternion.identity);
    }

    /// <summary>
    /// Go through all the objects and spawn them in a circle.
    /// Radius is determined by the size of the spawn zone collider.
    /// </summary>
    void SpawnObjectsInCircle() {
        /* "This boolean list wasn't found either." */
        List<bool> seatTaken = new List<bool>();

        float radius = _spawnZone.bounds.size.x / 2;
        Transform parent = this.gameObject.transform;

        for (int i = 0; i < _itemCount; i++) {
            // get the position on the circle to spawn this object
            float angle = i * Mathf.PI * 2 / _itemCount;
            Vector3 pos = Vector3.zero;
            pos.x = Mathf.Cos(angle);
            pos.z = Mathf.Sin(angle);
            pos *= radius;
            /* "Despite this line of code, on page 145, the prefabs will
             *  orbit around the spawn zone's origin, often from a distance,
             *  rather than rotate within its current coordinates."
             *
             * pos += _spawnZone.bounds.center; */

            // spawn as a child of the parent object
            GameObject newObj = Instantiate(_itemToSpawn, parent);
            newObj.transform.localPosition = pos;
        }
    }
}