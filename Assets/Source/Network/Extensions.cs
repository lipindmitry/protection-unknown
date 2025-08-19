using System;
using UnityEngine;

public static class Extensions
{
    public static Vector3 GetVector3(this byte[] bytes, int offset)
    {
        float x = BitConverter.ToSingle(bytes, offset);
        float y = BitConverter.ToSingle(bytes, offset + 4);
        float z = BitConverter.ToSingle(bytes, offset + 8);

        var vector = new Vector3(x, y, z);

        return vector;
    }

    public static void SetVector3(this byte[] bytes, Vector3 vector3, int offset)
    {
        byte[] x = BitConverter.GetBytes(vector3.x);
        byte[] y = BitConverter.GetBytes(vector3.y);
        byte[] z = BitConverter.GetBytes(vector3.z);
        Buffer.BlockCopy(x, 0, bytes, offset, 4);
        Buffer.BlockCopy(y, 0, bytes, offset + 4, 4);
        Buffer.BlockCopy(z, 0, bytes, offset + 8, 4);
    }

    public static Quaternion GetQuaternion(this byte[] bytes, int offset)
    {
        float xRotation = BitConverter.ToSingle(bytes, offset);
        float yRotation = BitConverter.ToSingle(bytes, offset + 4);
        float zRotation = BitConverter.ToSingle(bytes, offset + 8);
        float wRotation = BitConverter.ToSingle(bytes, offset + 12);

        var quaternion = new Quaternion(xRotation, yRotation, zRotation, wRotation);

        return quaternion;
    }

    public static void SetQuartenion(this byte[] bytes, Quaternion quaternion, int offset)
    {
        byte[] xRotationBytes = BitConverter.GetBytes(quaternion.x);
        byte[] yRotationBytes = BitConverter.GetBytes(quaternion.y);
        byte[] zRotationBytes = BitConverter.GetBytes(quaternion.z);
        byte[] wRotationBytes = BitConverter.GetBytes(quaternion.w);

        Buffer.BlockCopy(xRotationBytes, 0, bytes, offset, 4);
        Buffer.BlockCopy(yRotationBytes, 0, bytes, offset + 4, 4);
        Buffer.BlockCopy(zRotationBytes, 0, bytes, offset + 8, 4);
        Buffer.BlockCopy(wRotationBytes, 0, bytes, offset + 12, 4);
    }

    public static void SetId(this byte[] bytes, short id)
    {
        byte[] idBytes = BitConverter.GetBytes(id);
        Buffer.BlockCopy(idBytes, 0, bytes, 1, 2);
    }

    public static void SetFloat(this byte[] bytes, float value, int index)
    {
        byte[] idBytes = BitConverter.GetBytes(value);
        Buffer.BlockCopy(idBytes, 0, bytes, index, 4);
    }
}
