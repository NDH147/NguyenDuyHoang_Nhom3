using UnityEngine;
using System.Collections;
using System;

public class Health : MonoBehaviour
{
    [Header ("Health")]
    [SerializeField] private GameObject checkPoint;
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Deadth Sound")]
    [SerializeField] private AudioClip deadthSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        if(checkPoint != null) {
            checkPoint.SetActive(false);
        }
    }
    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
            SoundManager.instance.PlaySound(hurtSound); 
        }
        else
        {
            if (!dead)
            {
                anim.SetTrigger("die");
                if (invulnerable) return;

                currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                //Hủy kích hoạt tất cả các component được đính kèm
                //Vô hiệu hóa các chuyển động của enemy và player
                foreach (Behaviour component in components)
                    component.enabled = false;

                anim.SetBool("grounded", true);
                anim.SetTrigger("die");
                dead = true;
                SoundManager.instance.PlaySound(deadthSound);
            }
        }
                foreach (Behaviour component in components)
                    component.enabled = false;
                if(checkPoint != null) {
                    checkPoint.SetActive(true);
                }
                dead = true;
            }
        }
    }
    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
    }
    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate() { 
        gameObject.SetActive(false);
    }

    public void Respawn()
    {
        //Max health
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("Idle");
        //Khả năng bất tử
        StartCoroutine(Invunerability());

        //kích hoạt tất cả các component được đính kèm
        //active các chuyển động của enemy và player
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }

        /*dead = false;*/
    }
}