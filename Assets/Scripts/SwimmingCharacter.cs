using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingCharacter : MonoBehaviour
{
    // Yüzme ayarları
    public float swimSpeed = 5f;  // Yüzme hızı
    public float floatForce = 10f; // Suya çıkma kuvveti
    public float waterLevel = 0f; // Su seviyesinin y konumu
    public float upForce = 2f;  // Yüzeyden yukarı çıkmak için kuvvet
    public float maxUnderwaterDepth = -2f; // Karakterin suyun altına ne kadar inebileceğini sınırla
    public string swimmingZoneTag = "SwimmingZone"; // Suya giren yüzme alanının tag'i (SwimmingZone)

    private bool isUnderWater = false; // Karakter su altında mı?
    private Rigidbody rb;

    void Start()
    {
        // Rigidbody'yi alıyoruz
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true; // Yerçekimini aktif tutuyoruz
    }

    void Update()
    {
        HandleSwimming();
    }

    void HandleSwimming()
    {
        // Suya girip girmediğimizi kontrol et
        isUnderWater = transform.position.y < waterLevel;

        // Su yüzeyinin altına geçmeyi engelle (karakterin suyun altına düşmesini engelle)
        if (transform.position.y < maxUnderwaterDepth)
        {
            rb.velocity = Vector3.zero; // Hızını sıfırla
            transform.position = new Vector3(transform.position.x, maxUnderwaterDepth, transform.position.z); // Derinliği sınırlayalım
        }

        // Suyun altındaysak yukarı doğru kuvvet uygula
        if (isUnderWater)
        {
            rb.AddForce(Vector3.up * upForce, ForceMode.Acceleration);
        }

        // Yüzme hareketi
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);

        if (isUnderWater)
        {
            // Su altında hareket
            rb.AddForce(movement * swimSpeed, ForceMode.Force);
        }
        else
        {
            // Suyun üstünde hareket
            transform.Translate(movement * swimSpeed * Time.deltaTime);
        }

        // Su seviyesinde tutmak için yukarı doğru kuvvet ekle
        if (!isUnderWater && transform.position.y < waterLevel)
        {
            // Karakter suya girmemeli, su yüzeyine yukarıya doğru kuvvet uygulama
            rb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Suya giren nesne karakterse
        if (other.CompareTag(swimmingZoneTag))
        {
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (otherRb != null)
            {
                // Suya giren karakterin yerçekimini durdur
                otherRb.useGravity = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Eğer suyun içinde hareket eden nesne karakterse
        if (other.CompareTag(swimmingZoneTag))
        {
            // Karakterin su seviyesini ve yüzme limitini kontrol et
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Karakter suyun altına düşmesin diye yukarı doğru kuvvet uygula
                if (other.transform.position.y < waterLevel)
                {
                    rb.AddForce(Vector3.up * floatForce, ForceMode.Acceleration);
                }

                // Su seviyesinin altına düşerse, belirlenen derinlikte sınırlama yap
                if (other.transform.position.y < maxUnderwaterDepth)
                {
                    rb.velocity = Vector3.zero; // Hızını sıfırla
                    other.transform.position = new Vector3(other.transform.position.x, maxUnderwaterDepth, other.transform.position.z); // Derinliği sınırlayalım
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Eğer su yüzeyinden çıkan nesne karakterse
        if (other.CompareTag(swimmingZoneTag))
        {
            Rigidbody otherRb = other.GetComponent<Rigidbody>();
            if (otherRb != null)
            {
                // Su yüzeyinden çıkan karakterin yerçekimini aktif hale getir
                otherRb.useGravity = true;
            }
        }
    }
}
