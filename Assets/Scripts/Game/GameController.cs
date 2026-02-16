using UnityEngine;

namespace Game
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; set; }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DontDestroyOnLoad(Instance);
            }
            else
                Destroy(gameObject);
        }
    }
}