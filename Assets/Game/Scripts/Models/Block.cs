using System.Collections.Generic;
using UnityEngine;

namespace Nirville.TestingApp
{
    public enum BlockType
    {
        Block = 0,  
        Empty = 1,
        Flag = 2,
        Q1 = 3,
        Q2 = 4,
        Mine = 5,
        MineExplode = 6,
        MineDelete = 7,
        N1 = 8,
        N2 = 9,
        N3 = 10,
        N4 = 11,
        N5 = 12,
        N6 = 13,
        N7 = 14,
        N8 = 15
    }

    /// <summary>
    /// Define Block Model. How it will be displayed on the screen.
    /// Each and every block in the game has this script attached.
    /// </summary>
    internal class Block : GameModel
    {
        [SerializeField]
        private List<Sprite> blockSprites;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        public BlockType BlockType { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsRevealed { get; set; }
        public bool IsFlagged { get; set; }
        public IBoardController Board => App.controller.boardController;

        InputHandler input;

        private void Awake()
        {
            input = GetComponent<InputHandler>();
        }

        private void Start()
        {
            input.OnClick += Click;
        }


        /// <summary>
        /// After a click is determined, checks and reveals block type
        /// </summary>
        public void Click(int mouseButton)
        {
            if (mouseButton == 0)
            {
                if (BlockType == BlockType.Mine)
                {
                    BlockType = BlockType.MineExplode;
                    Board.RevealAll();
                }
                else if (BlockType == BlockType.Empty)
                {
                    Board.RevealEmpty(Row, Column);
                }
                Reveal();
            }
            else if (mouseButton == 1)
            {
                ShowFlag();
            }    
        }

        /// <summary>
        /// Reveals the clicked block type
        /// </summary>
        public void Reveal()
        {
            if(!IsRevealed && !IsFlagged)
            {
                IsRevealed = true;
                spriteRenderer.sprite = blockSprites[(int)BlockType];
            }
        }

        /// <summary>
        /// Flags the clicked block type.
        /// </summary>
        public void ShowFlag()
        {
            if(!IsFlagged)
            {
                IsFlagged = true;
                spriteRenderer.sprite = blockSprites[(int)BlockType.Flag];
            }
        }
    }
}