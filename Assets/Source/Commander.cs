using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Commander : MonoBehaviour
{
    //[SerializeField] private AttackCommand _attackCommand;
    //[SerializeField] private MoveToPositionCommand _moveToPositionCommand;
    //[SerializeField] private CommandStopper _commandStopper;
    //[SerializeField] private Player _player;
    [SerializeField] private TargetCalculator _targetCalculator;

    private StateMachine _stateMachine;

    public void Initialize(Player player)
    {
        _stateMachine = player.GetComponentInChildren<StateMachine>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) 
            && !EventSystem.current.IsPointerOverGameObject()
            && !_targetCalculator.Active
            /*&& !_commandStopper.Stopping*/)
        {
            //_attackCommand.Stop();
            //_moveToPositionCommand.Stop();

            var position = Mouse.current.position.ReadValue();
            var newPosition = new Vector3(position.x, position.y, Camera.main.nearClipPlane);
            var ray = Camera.main.ScreenPointToRay(newPosition);
            var hists = Physics.RaycastAll(ray);
            if (hists.Any(x => x.collider.GetComponent<Enemy>()))
            {
                var enemy = hists.Select(x => x.collider.GetComponent<Enemy>()).First(x => x);
                _stateMachine.ChangeState<PrepareAttackState, Unit>(enemy);
                //_attackCommand.Attack(enemy);

            }
            else if (hists.Length > 0)
            {
                var hit = hists.First();
                var args = new MoveToPositionArgs() { Target = hit.point };
                _stateMachine.ChangeState<MoveToPositionState, MoveToPositionArgs>(args);
                //_moveToPositionCommand.MoveTo(hit.point);
            }
        }
    }
}
