using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class Weapon : MonoBehaviourPunCallbacks
{
    [SerializeField] protected GameObject particle, cam;
    protected bool auto;
    protected float cooldown, timer;
    protected int ammoCurrent, ammoMax, ammoBackPack;
    [SerializeField] TMP_Text ammoText;
    [SerializeField] AudioSource shoot;
    [SerializeField] AudioClip bulletSound, reload, noBulletSound;


    // Start is called before the first frame update
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            timer += Time.deltaTime;

            if (Input.GetMouseButton(0))
            {
                Shoot();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (ammoCurrent != ammoMax || ammoBackPack != 0)
                {
                    Invoke("Reload", 1);
                    shoot.PlayOneShot(reload);
                }
            }
            TextUpdate();
        }
    }

    public void Shoot()
    {
        if (Input.GetMouseButtonDown(0) || auto)
        {
            if (timer > cooldown)
            {
                if (ammoCurrent > 0)
                {
                    timer = 0;
                    OnShoot();
                    ammoCurrent -= 1;
                    shoot.PlayOneShot(bulletSound);
                    shoot.pitch = Random.Range(1f, 1.5f);
                }
                else
                {
                    shoot.PlayOneShot(noBulletSound);
                }
            }
        }
    }

    protected virtual void OnShoot()
    {
        // bu fonksiyonu alt sınıflarda dolduracağız
    }

    void TextUpdate()
    {
        ammoText.text = ammoCurrent.ToString() + " / " + ammoBackPack.ToString();
    }

    void Reload()
    {
        int ammoNeed = ammoMax - ammoCurrent;

        if (ammoBackPack >= ammoNeed)
        {
            ammoBackPack -= ammoNeed;
            ammoCurrent += ammoNeed;
        }
        else
        {
            ammoCurrent += ammoBackPack;
            ammoBackPack = 0;
        }
    }
}
