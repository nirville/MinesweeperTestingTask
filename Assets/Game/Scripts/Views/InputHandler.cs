using UnityEngine;
using System;

namespace Nirville.TestingApp
{

    public class InputHandler : MonoBehaviour
    {
        public event Action<int> OnClick;

        void OnMouseOver()
        {
            if (Input.GetMouseButton(0))
                OnClick.Invoke(0);
            else if (Input.GetMouseButton(1))
                OnClick.Invoke(1);
        }
    }
}
