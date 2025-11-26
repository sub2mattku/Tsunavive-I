// ...existing code...
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TsunamiBehaivour : MonoBehaviour
{
    public float tsunamiSpeed = 5f;
    public Vector3 moveDirection = Vector3.forward;

    public float resetPositionZ;
    public float startPositionZ;

    public GameObject tsunamiObject;
    public GameObject tsunamiPrefab;
    public float spawnInterval = 60f; // seconds

    private float spawnTimer;
    public static float spawnstaticTimer;

    private List<GameObject> activeTsunamis = new List<GameObject>();

    public string GameoverSceneName = "GameOverScene";

    

    void Start()
    {
        spawnTimer = spawnInterval;
        if (tsunamiObject != null)
            activeTsunamis.Add(tsunamiObject);
    }

    void Update()
    {
        if (activeTsunamis.Count == 0)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnTsunami();
                spawnTimer = spawnInterval;
            }
        }
        else
        {
            spawnTimer = spawnInterval;
        }

        for (int i = activeTsunamis.Count - 1; i >= 0; i--)
        {
            var t = activeTsunamis[i];
            if (t == null)
            {
                activeTsunamis.RemoveAt(i);
                continue;
            }

            t.transform.Translate(moveDirection.normalized * tsunamiSpeed * Time.deltaTime, Space.World);

            float dirZ = moveDirection.normalized.z;
            float tz = t.transform.position.z;
            bool passedReset = (dirZ >= 0f) ? (tz >= resetPositionZ) : (tz <= resetPositionZ);

            if (passedReset)
            {
                Destroy(t);
                activeTsunamis.RemoveAt(i);
                PlayerMovement.Score += 10;
            }
        }

        spawnstaticTimer = spawnTimer;
    }

    private void SpawnTsunami()
    {
        if (tsunamiPrefab == null) return;

        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, startPositionZ);
        var instance = Instantiate(tsunamiPrefab, spawnPos, Quaternion.identity);
        activeTsunamis.Add(instance);

        // make sure the instance can detect the player: add a kinematic Rigidbody + trigger collider
        EnsureCollisionSetup(instance);

        // attach the handler and pass scene name
        var hit = instance.AddComponent<TsunamiHit>();
        hit.GameoverSceneName = GameoverSceneName;
    }

    private void EnsureCollisionSetup(GameObject go)
    {
        if (go == null) return;

        // ensure there's a Collider (use existing, otherwise add BoxCollider)
        Collider col = go.GetComponent<Collider>();
        if (col == null)
        {
            col = go.AddComponent<BoxCollider>();
        }
        col.isTrigger = true; // use trigger for reliable detection while we move via transform

        // ensure there's a Rigidbody (kinematic so physics won't move it)
        Rigidbody rb = go.GetComponent<Rigidbody>();
        if (rb == null)
            rb = go.AddComponent<Rigidbody>();

        rb.isKinematic = true; // kinematic Rigidbody still allows trigger callbacks
        rb.useGravity = false;
    }
}
// ...existing code...