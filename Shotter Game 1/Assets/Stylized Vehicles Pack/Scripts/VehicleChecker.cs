using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleChecker : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        var vehicle = other.GetComponent<Vehicle>();
        if (vehicle != null) 
        {
            vehicle.AracBilgileri();
            vehicle.ArizaYap();
        }
    }
}
