using DG.Tweening;
using UnityEngine;

public class AttackCommand : MonoBehaviour
{
    [SerializeField] private MoveToTargetCommand _moveCommand;
    [SerializeField] private Unit _unit;

    [SerializeField] private float _attackInterval = 1.8f;
    [SerializeField] private float _damageDelay = 0.4f;

    private Unit _target;

    private float _attackDelay;
    private bool _isAttacking = false;

    public event System.Action AttackStarted;
    public event System.Action AttackFinished;

    public void Attack(Unit target)
    {
        _target = target;
        _isAttacking = true;
    }

    public void Stop()
    {
        if (_isAttacking)
        {
            if (_moveCommand.IsMoving)
                _moveCommand.Stop();
            _isAttacking = false;
            AttackFinished?.Invoke();
        }
    }

    private void Attack()
    {
        if (_target.IsDead)
        {
            Stop();
            return;
        }

        _attackDelay = _attackInterval;

        _unit.transform.DOLookAt(_target.transform.position, 0.2f)
            .SetEase(Ease.OutSine) // Плавное замедление
            .SetUpdate(UpdateType.Fixed); // Для синхронизации с физикой

        AttackStarted?.Invoke();
        Invoke(nameof(DealDamage), _damageDelay);
    }

    private void DealDamage()
    {
        if (!_isAttacking)
            return;

        var weapon = _unit.Weapon;
        float damage = weapon.RandomDamage;
        _target.TakeDamage(damage, _unit);
    }

    private void Update()
    {
        if (_attackDelay > 0)
            _attackDelay -= Time.deltaTime;

        if (_isAttacking)
        {
            bool unitInRange = _unit.InRange(_target);

            if (_attackDelay <= 0 
                && unitInRange)
                Attack();

            if (!unitInRange 
                && !_moveCommand.IsMoving 
                && !_target.IsDead 
                && _attackDelay <= 0)
            {
                AttackFinished?.Invoke();
                _moveCommand.MoveTo(_target.transform, _unit.Weapon.Range);
            }
        }
    }
}

