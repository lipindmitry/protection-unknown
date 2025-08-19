using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Data/Skills/Earthquake")]
public class Earthquake : Skill
{
    [SerializeField] private float _length;
    [SerializeField] private float _width;
    [SerializeField] private float _height;
    [SerializeField] private float _damageMultiplier;
    [SerializeField] private float _stunDuration;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] private Transform _cube;

    private SkillArguments _args;

    public override void Activate(SkillArguments skillArguments)
    {
        _args = skillArguments;
        _args.Unit.transform.DOLookAt(_args.Position, 0.2f)
            .SetEase(Ease.OutSine)
            .SetUpdate(UpdateType.Fixed);
        
        ActivateEvent();
    }

    public override void Use()
    {
        var transform = _args.Unit.transform;
        var direction = (_args.Position - transform.position).normalized;

        Vector3 worldOffset = direction * _length / 2;
        Vector3 center = transform.position + worldOffset;
        Vector3 halfExtents = new (_width / 2f, _height / 2f, _length / 2f);
        Quaternion rotation = Quaternion.LookRotation(direction);

        //var cube = GameObject.Instantiate(_cube, center, rotation);
        //cube.localScale = new Vector3(_width, _height, _length);

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, rotation, _layerMask);

        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<Unit>();
            if (unit != _args.Unit)
            {
                Debug.Log($"Collider {collider.name}");
                Debug.Log($"Unit {unit.name}");
                unit.TakeDamage(_damageMultiplier * _args.Unit.Weapon.RandomDamage, _args.Unit);
                var stateMachine = unit.GetComponentInChildren<StateMachine>();
                var args = new StunStateArts() 
                { 
                    Duration = _stunDuration 
                };
                stateMachine.ChangeState<StunState, StunStateArts>(args);
            }
        }
    }
}

