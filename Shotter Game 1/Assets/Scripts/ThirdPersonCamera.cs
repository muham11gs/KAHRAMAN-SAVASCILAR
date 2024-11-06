using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField]
    [Range(0.5f, 2f)]
    float mouseSense = 1;
    [SerializeField]
    [Range(-20, -10)]
    int lookUp = -15;
    [SerializeField]
    [Range(15, 25)]
    int lookDown = 20;
    public bool isSpectator;
    [SerializeField] float speed = 50f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = transform.parent.gameObject;
    }
    void Update()
    {
        float rotateX = Input.GetAxis("Mouse X") * mouseSense;
        float rotateY = Input.GetAxis("Mouse Y") * mouseSense;

        if (!isSpectator)
        {
            Vector3 rotCamera = transform.rotation.eulerAngles;
            Vector3 rotPlayer = player.transform.rotation.eulerAngles;

            rotCamera.x = (rotCamera.x > 180) ? rotCamera.x - 360 : rotCamera.x;
            rotCamera.x = Mathf.Clamp(rotCamera.x, lookUp, lookDown);
            rotCamera.x -= rotateY;

            rotCamera.z = 0;
            rotPlayer.y += rotateX;

            transform.rotation = Quaternion.Euler(rotCamera);
            player.transform.rotation = Quaternion.Euler(rotPlayer);
        }
        else
        {
            // Mevcut kamera açısına bakalım
            Vector3 rotCamera = transform.rotation.eulerAngles;
            // Farenin hareketine bağlı olarak kameranın dönüşünü değiştirme
            rotCamera.x -= rotateY;
            rotCamera.z = 0;
            rotCamera.y += rotateX;
            transform.rotation = Quaternion.Euler(rotCamera);
            // WASD tuşlarına basışları okuma 
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            // Kamera hareket vektörünü ayarlama
            Vector3 dir = transform.right * x + transform.forward * z;
            // Kameranın konumunu değiştirme
            transform.position += dir * speed * Time.deltaTime;
        }

        // Eğer oyuncu ESC tuşuna basarsa
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Eğer imleç kitliyse...
            if (Cursor.lockState == CursorLockMode.Locked)
            {
                // Kilidi açalım
                Cursor.lockState = CursorLockMode.None;
            }
            // Diğer durumda...
            else
            {
                // İmleci kitleyelim
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}