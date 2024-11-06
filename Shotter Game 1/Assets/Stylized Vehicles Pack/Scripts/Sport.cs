using System.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sport : Vehicle
{
    [SerializeField] KeyCode kornaTusu;
    // Start is called before the first frame update
    protected override void Start()
    {
        Debug.Log("Spor arabaya ozel debug log");
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown(kornaTusu))
        {
            KornaCal();
        }
    }

    protected override void KornaCal()
    {
        Debug.Log("Bip bip! spor araba kornasi!!");
    }
    public override void ArizaYap()
    {
        Debug.Log("Arac beyninde hata var!");
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
