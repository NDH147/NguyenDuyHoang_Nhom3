using UnityEngine;

public class EnemyProjectile : EnemyDamage
{
    [SerializeField] private float speed;
    //hủy kích hoạt đối tượng sau một thời gian nhất định
    [SerializeField] private float resetTime;
    private float lifetime;

    private Animator anim;
    private BoxCollider2D coll;

    private bool hit;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }
    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true);
        coll.enabled = true;
    }
    private void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); // Thực thi logic từ script gốc trước
        /*gameObject.SetActive(false); // Khi mũi tên này chạm vào bất kỳ đối tượng nào, hãy hủy kích hoạt mũi tên*/
        coll.enabled = false;

        if (anim != null)
            anim.SetTrigger("explode"); //Khi object là một quả cầu lửa thì nó sẽ nổ tung
        else
            gameObject.SetActive(false); //Khi điều này chạm vào một object, cầu lửa sẽ vô hiệu hóa
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}