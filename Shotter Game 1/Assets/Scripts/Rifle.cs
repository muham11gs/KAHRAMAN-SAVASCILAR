using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Pistol
{
    // Start is called before the first frame update
    void Start()
    {
        auto = true;
        cooldown = 0.2f;
        ammoMax = 25;
        ammoBackPack = 75;
        ammoCurrent = 25;
    }
}
