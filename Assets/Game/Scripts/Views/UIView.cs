using UnityEngine;
using TMPro;

namespace Nirville.TestingApp
{
    /// <summary>
    /// FIX: use event to update mine count.
    /// </summary>
    public class UIView : GameView
    {
        [SerializeField]
        private TMP_Text mineCount;

        private void Start()
        {
            mineCount.text = "Mines : " + App.controller.boardController.TotalMines;
        }

        void Update()
        {
            if(Input.anyKeyDown)
            {
                mineCount.text = "Mines : " + App.controller.boardController.TotalMines;   
            }    
        }
    }
}