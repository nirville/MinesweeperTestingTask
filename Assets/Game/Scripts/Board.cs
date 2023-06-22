using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nirville.TestingApp
{
    public class Board : MonoBehaviour 
    {
        [SerializeField]
        private Block block;
        private int w = 15;
        private int h = 15;

        private Block[,] blocks;

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
                    var blockType = GetBlockType();
                    blocks[i,j] = Instantiate(block, new Vector2(i,j), Quaternion.identity);
                    blocks[i, j].blockType = blockType;
                    blocks[i, j].i = i;
                    blocks[i, j].j = j;
                }
            UpdateNearbyBlock();
        }

        void UpdateNearbyBlock()
        {
            for(int i = 0; i < w; i++)
            {
                for(int j = 0; j < h; j++)
                {
                    int n = 0;

                    if(blocks[i, j].blockType == BlockType.Mine)
                        continue;

                    var list = GetAllNearbyBlock(i, j);

                    //increase count of mine
                    foreach(var block in list)
                    {
                        if(block.blockType == BlockType.Mine)
                            n++;
                    }

                    //update block sprite to count
                    if(n > 0)
                        blocks[i, j].blockType = (BlockType)(n + 7);
                }
            }
        }

        BlockType GetBlockType()
        {
            if(Random.Range(1, 1000) % 5==0)
            {
                return BlockType.Mine;
            }
            return BlockType.Empty;
        }

        Block GetBlock(int i, int j)
        {
            if(i < 0 || j < 0 || i >= w || j >= h)
                return null;
            return blocks[i,j];
        }

       List<Block> GetAllNearbyBlock(int i, int j, bool includeCorner = true)
       {
            List<Block> list = new List<Block>();
            list.Add(GetBlock(i + 1, j));
            list.Add(GetBlock(i, j + 1));
            list.Add(GetBlock(i - 1, j));
            list.Add(GetBlock(i, j - 1));

            if(includeCorner)
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

       void RevealAll()
       {
            for(int i = 0; i < w; i++)
                for(int j = 0 ; j < h; j++)
                    blocks[i, j].Reveal();
       }


       public void RevealEmpty(int i, int j)
       {
            List<Block> list = GetAllNearbyBlock(i, j, false);
            list = list.Where(x => !x.show).ToList();

            foreach(var block in list)
            {
                if(block.blockType == BlockType.Empty && !block.show)
                {
                    block.Reveal();
                    RevealEmpty(block.i, block.j);
                }
            }
       }

       public void Lose()
       {
            RevealAll();
       }
    }
}