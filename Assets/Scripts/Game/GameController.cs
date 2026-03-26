using Game.Entity.Player;
using Game.Entity.Player.Movement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Manegement
{
    public class GameController : MonoBehaviour
    {
        public static GameController Instance { get; set; }

        public Transform playerTrans;
        public Player_InputHandler playerInput;

        public int currentBindPresset = 0;

        Camera mainCam;

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

        void Start()
        {
            playerTrans = FindAnyObjectByType<Player_Movement>().transform;
            playerInput = playerTrans.GetComponent<Player_InputHandler>();

            mainCam = Camera.main;
        }

        public static Vector3 GetMousePos(Vector3 referencial)
        {
            var mouseWorld = Instance.mainCam.ScreenToWorldPoint(Instance.playerInput.MousePos);

            var mouseDirection = (mouseWorld - referencial).normalized;
            mouseDirection.z = 0;

            return mouseWorld;
        }
    }
}