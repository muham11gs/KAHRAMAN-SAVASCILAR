using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coupe : Vehicle
{
    public override void ArizaYap()
    {
        Debug.Log("Coupe arabamiz ariza yapti. Sebebi belirsiz..");
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        marka = "Ford";
        Debug.Log("Base almadan önce coupe debug");
        base.Start();
        Debug.Log("Base aldiktan sonraki coupe debug");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        
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
