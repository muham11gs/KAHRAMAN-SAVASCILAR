using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class Enemy : MonoBehaviourPunCallbacks
{
    [SerializeField] protected int health, damage;
    [SerializeField] protected float attackDistance, cooldown;

    protected GameObject player;
    protected GameObject[] players;
    protected Animator anim;
    protected Rigidbody rb;
    protected float distance, timer;
    protected bool dead = false;
    [SerializeField] Image healthBar;

    // Start is called before the first frame update
    void Start()
    {
        CheckPlayers();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void CheckPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        Invoke("CheckPlayers", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        // en yakın oyuncuyu bul

        float closestDistance = Mathf.Infinity;
        foreach (GameObject closestPlayer in players)
        {
            float checkDistance = Vector3.Distance(transform.position, closestPlayer.transform.position);
            if (checkDistance < closestDistance)
            {
                if (closestPlayer.GetComponent<PlayerController>().health > 0)
                {
                    player = closestPlayer;
                    closestDistance = checkDistance;
                }
            }
        }

        if (player != null)
        {
            distance = Vector3.Distance(transform.position, player.transform.position);
            if (!dead)
            {
                Attack();
            }
        }
    }

    void FixedUpdate()
    {
        if (!dead && player != null)
        {
            Move();
        }
    }

    public virtual void Move()
    {

    }

    public virtual void Attack()
    {

    }

    public void SetHealth(int count)
    {
        photonView.RPC("ChangeHealth", RpcTarget.All, count);
    }

    [PunRPC]
    public void ChangeHealth(int count)
    {
        health -= count;
        // 0-100
        // 0-1
        float fillPercent = health / 100f;
        healthBar.fillAmount = fillPercent;
        if (health <= 0)
        {
            //toprağı bol olsun
            dead = true;
            GetComponent<Collider>().enabled = false;
            anim.SetBool("Die", true);
        }
    }
}
