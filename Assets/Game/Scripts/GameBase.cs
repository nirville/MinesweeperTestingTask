using UnityEngine;

namespace Nirville.TestingApp
{
    /// <summary>
    /// Base class for the Game. Will acts as an entry point for all game architecture.
    ///
    /// Can be converted to Singleton, A single static instance of GameApp will only be present at once.
    /// FIX: Need to avoid 'Find' functions.
    /// </summary>
    public class GameBase : MonoBehaviour
    {
        public GameApp App
        {
            get
            {
                return GameObject.FindObjectOfType<GameApp>();
            }
        }
    }
}
