using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private StateMachine _stateMachine;

    private Player[] _players;
    private Player _currentTarget;
    private Coroutine _checkTargetCoroutine;

    private void Awake()
    {
        _players = FindObjectsByType<Player>(FindObjectsSortMode.None);
        _checkTargetCoroutine = StartCoroutine(CheckClosestPlayer());
        _stateMachine.StateChanged += OnStateChanged;
    }

    private void OnStateChanged(IState newState, IState previousState)
    {
        if (newState is IdleState)
            Invoke(nameof(NewAim), 0.25f);
    }

    private void NewAim()
    {
        Player closestPlayer = GetClosestPlayer();
        _currentTarget = closestPlayer;
        _stateMachine.ChangeState<MoveToTargetState, Unit>(_currentTarget);
    }

    private void OnDisable()
    {
        // Остановка корутины при отключении объекта
        if (_checkTargetCoroutine != null)
        {
            StopCoroutine(_checkTargetCoroutine);
        }
    }

    private IEnumerator CheckClosestPlayer()
    {
        while (!_unit.IsDead)
        {
            Player closestPlayer = GetClosestPlayer();

            if (closestPlayer != null && closestPlayer != _currentTarget)
            {
                _currentTarget = closestPlayer;
                _stateMachine.ChangeState<MoveToTargetState, Unit>(_currentTarget);
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private Player GetClosestPlayer()
    {
        Player closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (Player player in _players.Where(x => !x.IsDead))
        {
            if (player == null || !player.gameObject.activeInHierarchy) continue;

            float distance = (transform.position - player.transform.position).sqrMagnitude;
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player;
            }
        }

        return closestPlayer;
    }

}
