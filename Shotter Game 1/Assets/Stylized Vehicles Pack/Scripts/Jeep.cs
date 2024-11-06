using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jeep : Vehicle
{
    public override void ArizaYap()
    {
        Debug.Log("Motor arizasi yapti!");
    }
    public override void AracBilgileri()
    {
        Debug.Log($"Marka: {marka}");
        base.AracBilgileri();
        Debug.Log($"Uretim yili: {uretimYili}");
        Debug.Log($"Tekerlek sayisi: {tekerSayisi}");
        Debug.Log($"Kapi sayisi: {kapiSayisi}");
    }
}
