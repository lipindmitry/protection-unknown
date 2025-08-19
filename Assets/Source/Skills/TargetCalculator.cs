using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TargetCalculator : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Texture2D _cursor;

    private Player _player;
    private TargetType _targetType;

    public bool Active { get; private set; }

    public event Action<SkillArguments> TargetDetected;

    private void Update()
    {
        if (Active)
        {
            if (Input.GetMouseButtonDown(0)
            && !EventSystem.current.IsPointerOverGameObject())
            {
                var position = Mouse.current.position.ReadValue();
                var newPosition = new Vector3(position.x, position.y, Camera.main.nearClipPlane);
                var ray = Camera.main.ScreenPointToRay(newPosition);
                var isHit = Physics.Raycast(ray, out var hitInfo, 100, _groundLayer);
                if (isHit)
                {
                    TargetDetected?.Invoke(new SkillArguments()
                    {
                        Unit = _player,
                        Position = hitInfo.point
                    });
                    Active = false;
                    Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                }
            }
        }
    }

    public void DetectSkillArguments(TargetType targetType, Player player)
    {
        _player = player;
        _targetType = targetType;
        Active = true;
        Cursor.SetCursor(_cursor, Vector2.zero, CursorMode.Auto);

        switch (targetType)
        {
            case TargetType.Enemy:
            case TargetType.Friend:
            case TargetType.Area:
                break;
            default:
                throw new Exception($"Incorrect target {targetType}");
        }

    }
}

