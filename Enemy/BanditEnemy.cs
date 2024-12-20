using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanditEnemy : MonoBehaviour
{
   [Header("Attack Parameters")]
    //thời gian hồi chiêu
    [SerializeField] private float attackCooldown;
    //sát thương
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    [Header("Attack Sound")]
    [SerializeField] private AudioClip attackSound;

    //References
    private Animator anim;
    private HealthBandit playerHealth;
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
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                anim.SetTrigger("Attack");
                SoundManager.instance.PlaySound(attackSound);
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();
    }

    //kiểm tra xem player có ở trong tầm nhìn hay không
    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<HealthBandit>();

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

    //Gây sát thương lên player
    private void DamagePlayer()
    {
        //kiểm tra player có trong vùng nhận dạng hay không
        if (PlayerInSight())
            playerHealth.TakeDamage(damage);
    }
}
