using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        auto = false;
        cooldown = 0.2f;
        ammoMax = 9;
        ammoBackPack = 27;
        ammoCurrent = 8;
    }

    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            GameObject gameBullet = Instantiate(particle, hit.point, transform.rotation);
            Destroy(gameBullet, 1f);
            if (hit.collider.GetComponent<Enemy>())
            {
                hit.collider.GetComponent<Enemy>().SetHealth(100);
            }
            if (hit.collider.GetComponent<PlayerController>())
            {
                hit.collider.GetComponent<PlayerController>().SetHealth(5);
            }
        }
    }
}
