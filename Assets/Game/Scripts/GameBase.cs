using UnityEngine;

namespace Nirville.TestingApp
{
    /// <summary>
    /// Base class for the Game. Will acts as an entry point for all game architecture.
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
