using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nirville.TestingApp
{
    internal class Board : GameController, IBoardController
    {
        [SerializeField]
        private Block block;
        private int w = 20;
        private int h = 15;
        [SerializeField]
        private int mines = 25;

        private Block[,] blocks;

        public int Width { get => w; set => w = value; }
        public int Height { get => h; set => h = value; }
        public int TotalMines { get => mines; set => mines = value; }

        // Register Presenters/Controllers initially.
        private void Awake()
        {
            App.controller = this;
            boardController = this;
        }

        private void Start() 
        {
            Init();
        }

        void Init()
        {
            blocks = new Block[w, h];

            for(int i = 0; i < w; i++)
                for(int j = 0; j < h; j++)
                {
                    var blockType = BlockType.Empty;
                    blocks[i,j] = Instantiate(block, new Vector2(i,j), Quaternion.identity);
                    blocks[i, j].transform.parent = transform;
                    blocks[i, j].BlockType = blockType;
                    blocks[i, j].Row = i;
                    blocks[i, j].Column = j;
                }
            SetMines();
            UpdateNearbyBlock();
        }

        [ContextMenu("Tree")]
        void UpdateNearbyBlock()
        {
            for(int i = 0; i < w; i++)
            {
                for(int j = 0; j < h; j++)
                {
                    int n = 0;

                    if(blocks[i, j].BlockType == BlockType.Mine)
                        continue;

                    var list = GetAllNearbyBlock(i, j);

                    //increase count of mine
                    foreach(var block in list)
                    {
                        if(block.BlockType == BlockType.Mine)
                            n++;
                    }

                    //update block sprite to count
                    if(n > 0)
                        blocks[i, j].BlockType = (BlockType)(n + 7);
                }
            }
        }

        void SetMines()
        {
            for (int i = 0; i < mines; i++)
            {
                int randW = Random.Range(0, w);
                int randH = Random.Range(0, h);
                blocks[randW, randH].BlockType = BlockType.Mine;
            }
        }

        Block GetBlock(int i, int j)
        {
            if(i < 0 || j < 0 || i >= w || j >= h)
                return null;
            return blocks[i,j];
        }

       List<Block> GetAllNearbyBlock(int i, int j, bool includeCorner = true)
       {
            List<Block> list = new List<Block>
            {
                GetBlock(i + 1, j),
                GetBlock(i, j + 1),
                GetBlock(i - 1, j),
                GetBlock(i, j - 1)
            };

            if (includeCorner)
            {
                list.Add(GetBlock(i + 1, j + 1));
                list.Add(GetBlock(i - 1, j - 1));
                list.Add(GetBlock(i + 1, j - 1));
                list.Add(GetBlock(i - 1, j + 1));
            }

            return list
            .Where(x => x != null)
            .ToList();
       }

       public void RevealAll()
       {
            for(int i = 0; i < w; i++)
                for(int j = 0 ; j < h; j++)
                    blocks[i, j].Reveal();
       }


       public void RevealEmpty(int i, int j)
       {
            List<Block> list = GetAllNearbyBlock(i, j, false);
            list = list.Where(x => !x.IsRevealed).ToList();

            foreach(var block in list)
            {
                if(block.BlockType == BlockType.Empty && !block.IsRevealed)
                {
                    block.Reveal();
                    RevealEmpty(block.Row, block.Column);
                }
            }
       }
    }
}