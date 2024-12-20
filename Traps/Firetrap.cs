using UnityEngine;
using System.Collections;

public class Firetrap : MonoBehaviour
{
    
    [SerializeField] private float damage;

    [Header("Firetrap Timers")]
    //thời gian delay khi kích hoạt bẫy
    [SerializeField] private float activationDelay;
    //thời gian tồn tại của bẫy
    [SerializeField] private float activeTime;
    private Animator anim;
    private SpriteRenderer spriteRend;

    [Header("SFX")]
    [SerializeField] private AudioClip fireSound;

    private bool triggered; //khi cái bẫy được kích hoạt
    private bool active; //khi bẫy hoạt động và có thể làm tổn thương người chơi

    private Health playerHealth;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerHealth != null && active)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerHealth = collision.GetComponent<Health>();

            if (!triggered) //kiểm tra bẫy được kích hoạt chưa
                StartCoroutine(ActivateFiretrap());

            if (active)
                collision.GetComponent<Health>().TakeDamage(damage);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") playerHealth = null;

    }
    private IEnumerator ActivateFiretrap()
    {
        //chuyển sprite sang màu đỏ để thông báo cho người chơi và kích hoạt bẫy
        triggered = true;
        spriteRend.color = Color.red;

        // Đợi độ trễ, kích hoạt bẫy, bật animator, trả lại màu bình thường
        yield return new WaitForSeconds(activationDelay);
        SoundManager.instance.PlaySound(fireSound);
        spriteRend.color = Color.white; //đổi sprite về màu ban đầu
        active = true;
        anim.SetBool("activated", true);

        //Đợi đến X giây, tắt bẫy và đặt lại tất cả các biến và animatior
        yield return new WaitForSeconds(activeTime);
        active = false;
        triggered = false;
        anim.SetBool("activated", false);
    }
}