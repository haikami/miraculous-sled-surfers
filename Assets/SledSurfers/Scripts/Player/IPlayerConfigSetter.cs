using SledSurfers.Scripts.Data.ScriptableObjects;

namespace SledSurfers.Scripts.Player
{
    //Each subsystem inside player prefab implementing this will have access to the config and set it up when a game starts
    public interface IPlayerConfigSetter
    {
        void SetConfig(PlayerPhysicsConfig config);
    }
}