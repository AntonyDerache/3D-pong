using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Blockers")]
    [SerializeField][Range(0, 1)] private float _blockerSpawnRate = 0.2f;
    [SerializeField] private float _blockerSpawnTime = 1f;
    [SerializeField] private GameObject _blockerPrefab;
    [Header("Powerups")]
    [SerializeField][Range(0, 1)] private float _powerupsSpawnRate = 0.2f;
    [SerializeField] private float _powerupsSpawnTime = 1f;
    [SerializeField] private List<GameObject> _powerupsPrefabs;

    private GameManager _gm;
    private System.Random random = new System.Random();
    private float _blockersElapsedTime;
    private float _powerupsElapsedTime;

    private List<GameObject> _powerupsList = new List<GameObject>();
    private List<GameObject> _blockersList = new List<GameObject>();

    private void Awake()
    {
        GameObject obj = GameObject.Find("GameManager");
        if (obj) {
            _gm = obj.gameObject.GetComponent<GameManager>();
            _gm.SetResetObjectsOnField(ResetObjects);
        } else {
            Debug.LogError("GameManager is missing in the scene");
        }
    }

    private void Update()
    {
        if (_gm.isPlaying) {
            BlockersSpawn();
            PowerupsSpawn();
        }
    }

    private void BlockersSpawn()
    {
        _blockersElapsedTime += Time.deltaTime;
        if (_blockersElapsedTime >= _blockerSpawnTime) {
            if (random.NextDouble() < _blockerSpawnRate) {
                float maxZ = transform.localScale.z / 2;
                float maxX = transform.localScale.x / 2;
                Vector3 spawnPos = new Vector3(Random.Range(maxX * -1, maxX), 0, Random.Range(maxZ * -1, maxZ));
                _blockersList.Add(Instantiate(_blockerPrefab, spawnPos, Quaternion.identity));
            }
            _blockersElapsedTime = 0;
        }
    }

    private void PowerupsSpawn()
    {
        _powerupsElapsedTime += Time.deltaTime;
        if (_powerupsElapsedTime >= _powerupsSpawnTime) {
            if (random.NextDouble() < _powerupsSpawnRate) {
                float maxZ = transform.localScale.z / 2;
                float maxX = transform.localScale.x / 2;
                Vector3 spawnPos = new Vector3(Random.Range(maxX * -1, maxX), 0, Random.Range(maxZ * -1, maxZ));
                int rdmIndex = Random.Range(0, 5);
                _powerupsList.Add(Instantiate(_powerupsPrefabs[rdmIndex], spawnPos, Quaternion.identity));
            }
            _powerupsElapsedTime = 0;
        }
    }

    private void ResetObjects() {
        foreach (var powerup in _powerupsList) {
            Destroy(powerup);
        }
        _powerupsList.Clear();
        foreach (var blocker in _blockersList) {
            Destroy(blocker);
        }
        _blockersList.Clear();
    }
}
