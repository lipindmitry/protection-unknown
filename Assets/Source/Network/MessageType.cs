using NUnit.Framework.Constraints;

public enum MessageType : byte
{
    None = 0,
    Connect = 1,
    Disconnect = 2,
    Number = 3,
    ObjectSpawned = 9,
    TransformSync = 10,
    AnimationSync = 11,
    NavMeshAgentSync = 12,
    HealthSync = 13,
}

public static class MessagesLength
{
    public static int Get(MessageType messageType)
    {
        switch (messageType)
        {
            case MessageType.TransformSync:
                return 31;
            case MessageType.AnimationSync:
                return 20;
            case MessageType.NavMeshAgentSync:
                return 17;
            case MessageType.ObjectSpawned:
                return 33;
            case MessageType.Number:
                return 2;
            case MessageType.HealthSync:
                return 7;
            default:
                throw new System.Exception($"Unsupported type message {messageType}");
        }
    }
}
