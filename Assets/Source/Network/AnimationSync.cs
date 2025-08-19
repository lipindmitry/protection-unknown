using System;
using System.Collections;
using UnityEngine;

public class AnimationSync : MonoBehaviour, INetworkSingleActivated
{
    [SerializeField] private Animator _animator;

    byte[] _buffer = new byte[MessagesLength.Get(MessageType.AnimationSync)];
    public event Action<byte[]> NeedSync;

    private int _currentStateHash;
    private bool _owner;

    public void SetTrigger(string name)
    {
        if (!_owner)
            return;

        int hash = Animator.StringToHash(name);
        _animator.SetTrigger(hash);

        byte[] hashBytes = BitConverter.GetBytes(hash);
        Buffer.BlockCopy(hashBytes, 0, _buffer, 3, 4);
        _buffer[7] = (byte)ParameterType.Trigger;
        _buffer[8] = 0;

        NeedSync?.Invoke(_buffer);
    }

    public bool GetBool(string name)
    {
        return _animator.GetBool(name);
    }

    public void SetBool(string name, bool value)
    {
        if (!_owner)
            return;

        int hash = Animator.StringToHash(name);
        _animator.SetBool(hash, value);

        byte[] hashBytes = BitConverter.GetBytes(hash);
        Buffer.BlockCopy(hashBytes, 0, _buffer, 3, 4);
        _buffer[7] = (byte)ParameterType.Bool;
        _buffer[8] = Convert.ToByte(value);

        NeedSync?.Invoke(_buffer);
    }

    public void Initialize(short id, bool owner)
    {
        _buffer[0] = (byte)MessageType.AnimationSync;
        byte[] idBytes = BitConverter.GetBytes(id);
        Buffer.BlockCopy(idBytes, 0, _buffer, 1, 2);
        _owner = owner;
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        _currentStateHash = stateInfo.shortNameHash;
    }

    public void SetMessage(byte[] message)
    {
        if (message[0] != (byte)MessageType.AnimationSync)
            return;

        int nameHash = BitConverter.ToInt32(message, 3);
        var type = (ParameterType)message[7];

        if (type == ParameterType.Bool)
        {
            var value = Convert.ToBoolean(message[8]);
            _animator.SetBool(nameHash, value);
        }
        else if (type == ParameterType.Trigger)
        {
            _animator.SetTrigger(nameHash);
        }
        //else
        //{
        //    throw new Exception($"Unsupported animatin type {type}");
        //}
        //Debug.Log($"Устанавливаю значение анимации {nameHash}");
    }

    public enum ParameterType : byte
    {
        None = 0,
        Trigger = 1,
        Bool = 2
    }
}
