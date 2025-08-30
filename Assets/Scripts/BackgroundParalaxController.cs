using UnityEngine;

public class BackgroundParalaxController : MonoBehaviour
{
    [SerializeField] float parallaxAmmountX = 1;
    [SerializeField] float parallaxAmmountY = 1;

    private float startPosX;
    private float startPosY;
    private void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
    }

    private void FixedUpdate()
    {
        float distanceX = Camera.main.transform.position.x * parallaxAmmountX;
        float distanceY = Camera.main.transform.position.y * parallaxAmmountY;

        transform.position = new Vector3 (startPosX + distanceX, startPosY + distanceY, transform.position.z);
    }
}
