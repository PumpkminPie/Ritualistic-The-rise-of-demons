using UnityEngine;

namespace Game.Manegement
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; set; }

        public int currentBindPresset = 0;

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