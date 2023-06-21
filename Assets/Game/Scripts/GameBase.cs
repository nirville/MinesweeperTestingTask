using UnityEngine;

public class GameBase : MonoBehaviour 
{
    public GameApp App
    {
        get
        {
            return GameObject.FindObjectOfType<BounceApplication>();
        }
    }
}