using System;

namespace Nirville.TestingApp
{

    /// <summary>
    /// Controller class to control all gameplay elements.
    /// Controls flow of gameplay.
    /// </summary>
    public class GameController : GameBase
    {
        public IBoardController boardController;

        public event Action OnLose;
        public event Action OnWin;
    }
}