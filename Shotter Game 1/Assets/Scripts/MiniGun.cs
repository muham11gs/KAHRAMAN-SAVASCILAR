using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGun : Weapon
{
    // Start is called before the first frame update
    void Start()
    {
        auto = true;
        cooldown = 0.1f;
        ammoMax = 800;
        ammoBackPack = 800;
        ammoCurrent = 800;
    }

    protected override void OnShoot()
    {
        Vector3 rayStartPosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 drift = new Vector3(Random.Range(-15f, 15f), Random.Range(-15f, 15f), Random.Range(-15f, 15f));
        Ray ray = cam.GetComponent<Camera>().ScreenPointToRay(rayStartPosition + drift);
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
