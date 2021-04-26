using System;

[Serializable]
public struct LevelStatus
{
    /// <summary>
    /// Describes the scene's name that can be loaded as a file.
    /// </summary>
    public readonly string Scene;
    
    /// <summary>
    /// Describes a unique identifier that can be used to execute additional instructions to achieve the desired state
    /// of the game. For example "rocketcene" could execute instructions to move the player's transform to the ship,
    /// equip rocket boots and a gun.
    /// </summary>
    public readonly string Place;

    public LevelStatus(string scene = "", string place = "")
    {
        Scene = scene;
        Place = place;
    }
}
