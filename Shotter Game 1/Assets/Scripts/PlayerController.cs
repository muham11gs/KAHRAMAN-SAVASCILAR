using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{
    // sıralı küme
    public enum Weapons
    {
        Pistol, //0
        Rifle, // 1
        MiniGun,
        NoWeapon,
    }

    [SerializeField] float movementSpeed = 5f;
    float currentSpeed;
    Rigidbody rb;
    Vector3 direction;
    float stamina = 5f;

    [SerializeField] float shiftSpeed = 10f, jumpForce = 7f;
    bool isGrounded = true;

    Animator anim;

    [SerializeField] GameObject pistol, rifle, miniGun;
    bool isPistol, isRifle, isMiniGun;

    [SerializeField] Image pistolUI, rifleUI, miniGunUI, cursorUI;

    [SerializeField] AudioSource characterSounds;
    [SerializeField] AudioClip jump;
    TextUpdate textUpdate;
    GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        textUpdate = GetComponent<TextUpdate>();
        gameManager = FindObjectOfType<GameManager>();

        gameManager.ChangePlayerList();
        currentSpeed = movementSpeed;
        if (!photonView.IsMine)
        {
            enabled = false;
            transform.Find("Main Camera").gameObject.SetActive(false);
            transform.Find("Canvas").gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //-1..0..1 a => -1 , d => 1
        float moveHorizontal = Input.GetAxis("Horizontal");
        //-1..0..1 s => -1 , w => 1
        float moveVertical = Input.GetAxis("Vertical");

        direction = new Vector3(moveHorizontal, 0.0f, moveVertical);
        direction = transform.TransformDirection(direction);
        if (direction.x != 0 || direction.z != 0)
        {
            anim.SetBool("Run", true);
            if (!characterSounds.isPlaying && isGrounded)
            {
                characterSounds.Play();
            }
        }
        if (direction.x == 0 && direction.z == 0)
        {
            anim.SetBool("Run", false);
            characterSounds.Stop();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            anim.SetBool("Jump", true);
            characterSounds.Stop();
            AudioSource.PlayClipAtPoint(jump, transform.position);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1) && isPistol)
        {
            ChangeWeapon(Weapons.Pistol);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && isRifle)
        {
            ChangeWeapon(Weapons.Rifle);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && isMiniGun)
        {
            ChangeWeapon(Weapons.MiniGun);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon(Weapons.NoWeapon);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (stamina > 0)
            {
                stamina -= Time.deltaTime;
                currentSpeed = shiftSpeed;
            }
            else
            {
                currentSpeed = movementSpeed;
            }
        }

        else if (!Input.GetKey(KeyCode.LeftShift))
        {
            stamina += Time.deltaTime;
            currentSpeed = movementSpeed;
        }

        if (stamina > 5f)
        {
            stamina = 5f;
        }
        else if (stamina < 0)
        {
            stamina = 0;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * currentSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        isGrounded = true;
        anim.SetBool("Jump", false);
    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "pistol":
                if (!isPistol)
                {
                    isPistol = true;
                    pistolUI.color = Color.black;
                    ChangeWeapon(Weapons.Pistol);
                    Destroy(other.gameObject);
                }
                break;
            case "rifle":
                if (!isRifle)
                {
                    isRifle = true;
                    rifleUI.color = Color.white;
                    ChangeWeapon(Weapons.Rifle);
                    Destroy(other.gameObject);
                }
                break;
            case "minigun":
                if (!isMiniGun)
                {
                    isMiniGun = true;
                    // r = umut g= kayra b=tolga a=demir
                    miniGunUI.color = new Color(145.0f, 57.01f, 255f, 0.5f);
                    ChangeWeapon(Weapons.MiniGun);
                    Destroy(other.gameObject);
                }
                break;
        }
    }

    // wrapper
    void ChangeWeapon(Weapons weapons)
    {
        photonView.RPC("ChooseWeapon", RpcTarget.All, weapons);
    }

    [PunRPC]
    public void ChooseWeapon(Weapons weapons)
    {
        anim.SetBool("Pistol", weapons == Weapons.Pistol);
        anim.SetBool("Assault", weapons == Weapons.Rifle);
        anim.SetBool("MiniGun", weapons == Weapons.MiniGun);
        anim.SetBool("NoWeapon", weapons == Weapons.NoWeapon);
        pistol.SetActive(weapons == Weapons.Pistol);
        rifle.SetActive(weapons == Weapons.Rifle);
        miniGun.SetActive(weapons == Weapons.MiniGun);

        cursorUI.enabled = weapons != Weapons.NoWeapon;
    }

    public int health = 100;
    //wrapper CH
    public void SetHealth(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }

    [PunRPC]
    public void ChangeHealth(int count)
    {
        health -= count;
        textUpdate.SetHealth(health);
        if (health <= 0)
        {
            //toprağı bol olsun
            transform.Find("Main Camera").GetComponent<ThirdPersonCamera>().isSpectator = true;
            enabled = false;
            anim.SetBool("Die", true);
            //ChooseWeapon(Weapons.NoWeapon);
            ChangeWeapon(Weapons.NoWeapon);
            gameManager.ChangePlayerList();
        }
    }

}
