using UnityEngine;

public class CharacterManager : MonoSingleton<CharacterManager>
{
    public Player player;

    public Player Player
    {
        get { return player; }
        set { player = value; }
    }
}
