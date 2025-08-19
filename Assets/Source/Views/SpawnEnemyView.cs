using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemyView : MonoBehaviour
{
    [SerializeField] private NetworkSpawner _networkSpawner;
    [SerializeField] private Button _spawn;
    [SerializeField] private TMP_InputField _x;
    [SerializeField] private TMP_InputField _z;
    [SerializeField] private SpawnView _spawnView;

    private bool _spawned;

    private void Start()
    {
        _spawn.onClick.AddListener(SpawnEnemy);
    }

    private void SpawnEnemy()
    {
        if (!_spawned)
            _spawnView.Spawn();
        else
            _networkSpawner.SpawnOwnedObject(10, new Vector3(int.Parse(_x.text), 0, int.Parse(_z.text)), Quaternion.identity);
        _spawned = true;
    }
}

