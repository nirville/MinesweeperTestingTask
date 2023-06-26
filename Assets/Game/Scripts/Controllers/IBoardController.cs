using System;

namespace Nirville.TestingApp
{
    public interface IBoardController
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public int TotalMines { get; set; }

        /// <summary>
        /// Reveals the block identity at the specified location.
        /// </summary>
        /// <param name="i">Row Element Index</param>
        /// <param name="j">Column Element Index</param>
        public void RevealEmpty(int i, int j);

        /// <summary>
        /// Reveals every block identity in the board.
        /// </summary>
        public void RevealAll();
    }
}