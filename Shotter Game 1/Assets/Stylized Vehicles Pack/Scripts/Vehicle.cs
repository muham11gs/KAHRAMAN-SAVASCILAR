using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    public string marka;
    [SerializeField] private string model;
    [SerializeField] protected int uretimYili;
    [SerializeField] protected int tekerSayisi;
    [SerializeField] protected int kapiSayisi;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Debug.Log(gameObject.name);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            KornaCal();
        }
    }


    protected virtual void KornaCal()
    {
        Debug.Log($"{gameObject.name} OBJESI VEHICLE SCRIPTTEN KORNA CALDI!");
    }

    public virtual void AracBilgileri()
    {
        Debug.Log($"Model: {model}");
    }

    public abstract void ArizaYap();

}
