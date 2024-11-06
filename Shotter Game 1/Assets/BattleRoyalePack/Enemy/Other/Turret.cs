using UnityEngine;
public class Turret : Enemy
{
    [SerializeField] Transform bulletPos;
    [SerializeField] LineRenderer lineRenderer;
    RaycastHit hit;    
    public override void Attack()
    {        
        timer += Time.deltaTime;
        transform.LookAt(player.transform);

        if (distance < attackDistance && timer > cooldown)
        {      
            timer = 0;
            
            if (Physics.Raycast(bulletPos.position, transform.forward, out hit))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    lineRenderer.SetPosition(0, bulletPos.position);
                    lineRenderer.SetPosition(1, player.transform.position);
                    //hit.collider.gameObject.GetComponent<PlayerController>().ChangeHealth(damage);
                    player.GetComponent<PlayerController>().SetHealth(damage);
                }
            }
        }  
        else
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}