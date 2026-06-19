using SledSurfers.Scripts.Data.ScriptableObjects;

namespace SledSurfers.Scripts.Player
{
    public interface IPlayerConfigSetter
    {
        void SetConfig(PlayerPhysicsConfig config);
    }
}