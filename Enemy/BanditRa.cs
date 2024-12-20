using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditRa : MonoBehaviour
{
    [Header("Attack Parameters")]
    //thời gian hồi chiêu
    [SerializeField] private float attackCooldown;
    //sát thương
    [SerializeField] private float range;
    [SerializeField] private int damage;

    //bắn đạn
    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Fireball Sound")]
    [SerializeField] private AudioClip fireballSound;

    //References
    private Animator anim;
    private BanditPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<BanditPatrol>();
    }
    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Chỉ tấn công khi nhìn thấy người chơi
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetBool("rangedAttack", true);
            }
        }else{
            anim.SetBool("rangedAttack", false);
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
        else
              Debug.Log("Ranged Attack Triggered");
    }

    private void RangedAttack()
    {
        SoundManager.instance.PlaySound(fireballSound);
        cooldownTimer = 0;

        fireballs[FindFireball()].transform.position = firePoint.position;
        fireballs[FindFireball()].GetComponent<Projectile>().SetDirection(Mathf.Sign(-transform.localScale.x));
    }

    private int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i;
        }
        return 0;
    }

    //kiểm tra xem player có ở trong tầm nhìn hay không
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    //vẽ vùng nhận dạng player
    private void OnDrawGizmos()
    {
        //màu của vùng  
        Gizmos.color = Color.red;
        //khối vùng nhận dạng
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
