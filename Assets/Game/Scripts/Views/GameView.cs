using UnityEngine.SceneManagement;
using System;

namespace Nirville.TestingApp
{
    /// <summary>
    /// Contains all views in the Game.
    /// UI, Design & Art.
    /// </summary>
    public class GameView : GameBase
    {
        public void RestartScene()
        {
            SceneManager.LoadScene(0);
        }
    }
}