using UnityEngine;

namespace Game.Character.Player.Cam
{
    public class Player_CamSystem : MonoBehaviour
    {
        [SerializeField] Transform playerBody;
        [SerializeField] float camMoveFactor = 10f;

        [SerializeField] Camera mainCamera;
        [SerializeField] Vector2 camMaxPos;
        [SerializeField] bool freezeXAxis, freezeYAxis, freezeZAxis;

        private void Start()
        {
            if (playerBody == null)
                mainCamera = Camera.main;

            //camMaxPos = mainCamera.transform.position.z;
        }

        void LateUpdate()
        {
            var _x = !freezeXAxis ? playerBody.position.x : camMaxPos.x;

            var _y = !freezeYAxis ? playerBody.position.y : camMaxPos.y;

            //var _z = !freezeZAxis ? mainCamera.transform.localPosition.z : -10;

            Vector3 goTo = new Vector3(_x, _y, mainCamera.transform.localPosition.z);

            transform.position = Vector3.Lerp(transform.position, goTo, camMoveFactor * Time.deltaTime);
        }

        public void SetCamMaxPos(Vector2 pos)
        {
            camMaxPos = pos;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(0, 255, 0, 0.1f);
            Gizmos.DrawWireCube(transform.position, camMaxPos);
        }
    }
}
