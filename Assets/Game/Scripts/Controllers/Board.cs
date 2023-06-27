using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Nirville.TestingApp
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    [System.Serializable]
    public struct DifficultySetting
    {
        public int boardWidth;
        public int boardHeight;
        public int boardMines;
    }

    public struct DifficultySettings
    {
        public DifficultySetting hardDifficulty;
        public DifficultySetting medDifficulty;
        public DifficultySetting easyDifficulty;
    }

    internal class Board : GameController, IBoardController
    {
        [SerializeField]
        public Difficulty difficulty;
        [SerializeField]
        private Block block;
        [SerializeField]
        private int w = 20;
        [SerializeField]
        private int h = 15;
        [SerializeField]
        private int mines = 25;

        private Block[,] blocks;

        public int Width { get => w; set => w = value; }
        public int Height { get => h; set => h = value; }
        public int TotalMines { get => mines - TotalFlaggedMines; set => mines = value; }
        public int TotalFlaggedMines { get; set; }

        void ReadSettingsFromFile()
        {
            var jsonPath = Application.streamingAssetsPath + "/GameSettings.json";
            var jsonString = File.ReadAllText(jsonPath);
            DifficultySettings settings = JsonUtility.FromJson<DifficultySettings>(jsonString);

            switch(difficulty)
            {
                case Difficulty.Easy:
                    Width = settings.easyDifficulty.boardWidth;
                    Height = settings.easyDifficulty.boardHeight;
                    TotalMines = settings.easyDifficulty.boardMines;
                    break;

                case Difficulty.Medium:
                    Width = settings.medDifficulty.boardWidth;
                    Height = settings.medDifficulty.boardHeight;
                    TotalMines = settings.medDifficulty.boardMines;
                    break;

                case Difficulty.Hard:
                    Width = settings.hardDifficulty.boardWidth;
                    Height = settings.hardDifficulty.boardHeight;
                    TotalMines = settings.hardDifficulty.boardMines;
                    break;
            }
        }

        // Register Presenters/Controllers initially.
        private void Awake()
        {
            App.controller = this;
            boardController = this;
            ReadSettingsFromFile();
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

        [ContextMenu("Update All Blocks")]
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
                if (block.BlockType == BlockType.N1 && !block.IsRevealed)
                {
                    block.Reveal();
                }
            }
       }
    }
}