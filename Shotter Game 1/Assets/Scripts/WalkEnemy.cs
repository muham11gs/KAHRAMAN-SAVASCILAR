using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkEnemy : Enemy
{
    [SerializeField] float speed, detectionDistance;

    public override void Move()
    {
        if (distance < detectionDistance && distance > attackDistance)
        {
            transform.LookAt(player.transform);
            anim.SetBool("Run", true);
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Run", false);
        }
    }

    public override void Attack()
    {
        timer += Time.deltaTime;

        if (distance <= attackDistance && timer > cooldown)
        {
            timer = 0;
            //player.GetComponent<PlayerController>().ChangeHealth(damage);
            player.GetComponent<PlayerController>().SetHealth(damage);
            anim.SetBool("Attack", true);
        }
        else
        {
            anim.SetBool("Attack", false);
        }
    }
}
