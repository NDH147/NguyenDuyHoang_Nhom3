using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("SpikeHead Attributes")]
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    [SerializeField] private LayerMask playerLayer;
    [Header("SFX")]
    [SerializeField] private AudioClip spikeHeadSound;

    private Vector3[] directions = new Vector3[4];
    private Vector3 destination;
    private float checkTimer;
    private bool attacking;

    private void OnEnable()
    {
        Stop();
    }
    private void Update()
    {
        // Chỉ di chuyển đầu nhọn đến đích nếu tấn công
        if (attacking)
            transform.Translate(destination * Time.deltaTime * speed);
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                CheckForPlayer();
        }
    }
    private void CheckForPlayer()
    {
        CalculateDirections();

        // Kiểm tra xem Spikehead có nhìn thấy người chơi ở cả 4 hướng không
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i], Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }
        }
    }
    private void CalculateDirections()
    {
        directions[0] = transform.right * range; // bên phải
        directions[1] = -transform.right * range; // bên trái
        directions[2] = transform.up * range; // bên trên
        directions[3] = -transform.up * range; //bên dưới
    }
    private void Stop()
    {
        destination = transform.position; // Đặt đích đến là vị trí hiện tại để nó không di chuyển
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.PlaySound(spikeHeadSound);
        base.OnTriggerEnter2D(collision);
        Stop(); // Dừng lại khi player ta chạm vào thứ gì đó
    }
}