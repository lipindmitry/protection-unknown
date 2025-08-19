using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnView : MonoBehaviour
{
    [SerializeField] private NetworkSpawner _networkSpawner;
    [SerializeField] private TMP_InputField _xPosition;
    [SerializeField] private TMP_InputField _yPosition;
    [SerializeField] private TMP_InputField _zPosition;
    [SerializeField] private Button _spawn;
    [SerializeField] private Following _following;
    [SerializeField] private SkillDistributor _skillDistributor;
    [SerializeField] private Commander _commander;

    private void Start()
    {
        _spawn.onClick.AddListener(Spawn);
    }

    private void OnDestroy()
    {
        _spawn.onClick.RemoveAllListeners();
    }

    public void Spawn()
    {
        float x = float.Parse(_xPosition.text);
        float y = float.Parse(_yPosition.text);
        float z = float.Parse(_zPosition.text);
        var position = new Vector3(x, y, z);

        var player = _networkSpawner.SpawnOwnedObject(1, position, Quaternion.identity);
        _following.SetTarget(player.transform);
        _skillDistributor.Initialize(player.GetComponent<Player>());
        _commander.Initialize(player.GetComponent<Player>());
    }
}

