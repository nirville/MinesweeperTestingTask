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

    public class Block : MonoBehaviour 
    {
        [SerializeField]
        private List<Sprite> blockSprites;

        [SerializeField]
        private SpriteRenderer sb;
        public BlockType blockType = 0;
        public int i = 0;
        public int j = 0;
        public bool show = false;
        public bool flag = false;

        /// <summary>
        /// Called every frame while the mouse is over the GUIElement or Collider.
        /// </summary>
        void OnMouseOver()
        {
            if(Input.GetMouseButton(0))
            {
                Click();
            }

            if(Input.GetMouseButton(1))
            {
                ShowFlag();
            }
        }

        /// <summary>
        /// After a click is determined, checks and reveals block type
        /// </summary>
        public void Click()
        {
            if(blockType == BlockType.Mine)
            {
                blockType = BlockType.MineExplode;
                FindObjectOfType<Board>().Lose();
            }
            else if(blockType == BlockType.Empty)
            {
                FindObjectOfType<Board>().RevealEmpty(i, j);
            }
            Reveal();
        }

        /// <summary>
        /// Reveals the clicked block type
        /// </summary>
        public void Reveal()
        {
            if(!show)
            {
                show = true;
                sb.sprite = blockSprites[(int)blockType];
            }
        }

        
        public void ShowFlag()
        {
            if(!flag)
            {
                flag = true;
                sb.sprite = blockSprites[(int)BlockType.Flag];
            }
        }
    }
}