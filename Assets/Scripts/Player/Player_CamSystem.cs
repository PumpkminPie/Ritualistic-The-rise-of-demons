using UnityEngine;

namespace Game.Character.Player.Cam
{
    public class Player_CamSystem : MonoBehaviour
    {
        [SerializeField] Transform playerBody;
        [SerializeField] float camMoveFactor = 10f;

        [SerializeField] Camera mainCamera;
        [SerializeField] Vector3 camMaxPos;

        private void Start()
        {
            if (playerBody == null)
                mainCamera = Camera.main;

            //camMaxPos = mainCamera.transform.position.z;
        }

        void Update()
        {
            Vector3 goTo = new Vector3(playerBody.position.x, 0, -10);

            transform.position = Vector3.Lerp(transform.position, goTo, camMoveFactor * Time.deltaTime);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 255, 0, 0.1f);
            Gizmos.DrawWireCube(transform.position, camMaxPos);
        }
    }
}
