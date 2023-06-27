using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Nirville.TestingApp
{

    internal class AiController : MonoBehaviour
    {
        private Board board;

        [SerializeField]
        private float timeToAutoplay = 5;

        bool initialInput = false;
        bool gameLost = false;

        List<Block> _revealedBlocks;

        private void Awake()
        {
            board = GetComponent<Board>();
        }

        IEnumerator Start()
        {
            _revealedBlocks = new List<Block>();

            yield return new WaitForSeconds(timeToAutoplay);
            StartCoroutine(Autoplay());
        }

        [ContextMenu("Go")]
        void Go() => StartCoroutine(Autoplay());

        IEnumerator Autoplay()
        {
            Debug.Log("Autoplaying");

            //randomly choose an opening area with knowledge of first successfully blank element.
            OpenInitialBlocks();

            //while (!gameLost)
            //{

            //}
            //traverse through the list of all adjacent tiles


            //find out minimum number from the traversed tiles

            yield return new WaitForSeconds(2);
            GetRevealedBlocks(firstBlockPos.x, firstBlockPos.y);
        }

        Vector2Int firstBlockPos;

        void OpenInitialBlocks()
        {
            var randW = Random.Range(0, board.Width);
            var randH = Random.Range(0, board.Height);
            var block = board.GetBlock(randW, randH);
            firstBlockPos = new Vector2Int(randW, randH);

            if (block.BlockType != BlockType.Empty)
                OpenInitialBlocks();
            board.RevealEmpty(randW, randH);

            initialInput = true;
        }

        void GetRevealedBlocks(int i, int j)
        {
            List<Block> list = board.GetAllNearbyBlock(i, j, false);
            list = list.Where(x => x.IsRevealed).ToList();

            foreach (var b in list)
            {
                if (b.IsRevealed && b.BlockType != BlockType.Empty)
                {
                    if (!_revealedBlocks.Contains(b))
                    {
                        _revealedBlocks.Add(b);
                        GetRevealedBlocks(b.Row, b.Column);
                    }
                }
            }
        }

        [ContextMenu("Neighbour")]
        void GetLowestNeighbor()
        {
            var randomBlock = Random.Range(0, _revealedBlocks.Count);
            var randW = Random.Range(0, board.Width);
            var randH = Random.Range(0, board.Height);
            var block = _revealedBlocks[randomBlock];

            var list = _revealedBlocks.Where(x => x.BlockType != BlockType.Empty).ToList();

            //var list = board.GetAllNearbyBlock(block.Row, block.Column, true);
            list = list.Where(x => x.IsRevealed).ToList();
            int neighbourCount = list.Where(x => !x.IsRevealed).Count();

            block.gameObject.SetActive(false);
            Debug.Log(neighbourCount);
        }
    }
}