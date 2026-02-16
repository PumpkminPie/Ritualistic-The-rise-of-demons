using Unity.VisualScripting;
using UnityEngine;

public class Player_CamSystem : MonoBehaviour
{
    [SerializeField] Transform playerBody;
    [SerializeField] float camMoveFactor = 10f;

    [SerializeField] Camera mainCamera;
    [SerializeField] float mainCamMaxZ;

    private void Start()
    {
        if (playerBody == null) 
            mainCamera = Camera.main;

        mainCamMaxZ = mainCamera.transform.position.z;
    }

    void Update()
    {
        Vector3 goTo = new Vector3(playerBody.position.x, playerBody.position.y, mainCamMaxZ);

        transform.position = Vector3.Lerp(transform.position, goTo, camMoveFactor * Time.deltaTime);
    }
}
