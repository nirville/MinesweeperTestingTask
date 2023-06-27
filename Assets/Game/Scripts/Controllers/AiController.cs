using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nirville.TestingApp
{

    public class AiController : MonoBehaviour
    {
        private Board board;

        [SerializeField]
        private float timeToAutoplay = 10;

        private void Awake()
        {
            board = GetComponent<Board>();
        }

        IEnumerator Start()
        {
            yield return new WaitForSeconds(timeToAutoplay);
            Autoplay();
        }

        void Autoplay()
        {
            //randomly choose an opening area with knowledge of first successfully blank element.

            //traverse through the list of all adjacent tiles
            //find out minimum number from the traversed tiles
        }
    }
}