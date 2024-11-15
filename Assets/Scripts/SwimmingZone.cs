using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingZone : MonoBehaviour
{
    public string swimmerTag = "Player"; // Suya girmesini istediğimiz karakterin tag'i
    public float waterLevel = 0f; // Su seviyesinin y konumu
    public float maxUnderwaterDepth = -2f; // Karakterin suyun altına ne kadar inebileceğini sınırla
    public float floatForce = 10f; // Karakterin su yüzeyine çıkmasını sağlamak için kuvvet

    private void OnTriggerEnter(Collider other)
    {
        // Eğer suya giren nesne karakterse
        if (other.CompareTag(swimmerTag))
        {
            // Karakterin Rigidbody bileşenini al
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false; // Suya giren karakterin yerçekimini durdur (yüzme hareketi için)
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Eğer suyun içinde hareket eden nesne karakterse
        if (other.CompareTag(swimmerTag))
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
        if (other.CompareTag(swimmerTag))
        {
            // Karakterin yerçekimini tekrar aktif hale getir
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }
        }
    }
}
