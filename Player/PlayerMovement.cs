using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //serializeField cho phep speed duoc sua truc tiep tu Unity
    [Header("Movement Parameters")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTime;//player có thể lơ lửng trên không bao lâu trước khi nhảy
    private float coyoteCounter; //thời gian trôi qua kể từ khi người chơi chạy ra khỏi mép

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumping")]
    [SerializeField] private float wallJumpX; //Lực nhảy qua tường ngang
    [SerializeField] private float wallJumpY; //Lực nhảy tường thẳng đứng

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [Header ("SFX")]
    [SerializeField] private AudioClip jumpSound;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        //Grab references for rigidbody and animator from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        //gia tri truc ngang
        horizontalInput = Input.GetAxis("Horizontal");

        //Flip player when moving left-right
        if (horizontalInput > 0.01f) //player di chuyển sang phải
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)  //player di chuyển sang trái
            transform.localScale = new Vector3(-1, 1, 1);

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Wall jump logic
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Jump();

        //điều chỉnh độ cao nhảy
        if (Input.GetKeyUp(KeyCode.UpArrow) && body.velocity.y > 0)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

        if (onWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            //kiểm tra xem có tường hay không
            if (isGrounded())
            {
                coyoteCounter = coyoteTime; //reset coyoteCounter khi ở trên mặt đất
                jumpCounter = extraJumps; //reset jump counter về giá trị extra jump
            }
            else
                coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
        }
    }

    private void Jump()
    {

        if (coyoteCounter <= 0 && !onWall() && jumpCounter <= 0) return;
        //Nếu coyote counter bằng 0 hoặc nhỏ hơn và không ở trên tường và không có cú nhảy bổ sung nào thì không làm gì cả

        SoundManager.instance.PlaySound(jumpSound);
        if (onWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            else
            {
                //Nếu player không ở trên mặt đất và coyote counter lớn hơn 0 thì nhảy bình thường
                if (coyoteCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                else
                {
                    if (jumpCounter > 0) //Nếu có thêm bước nhảy thì hãy nhảy và giảm jump counter
                    {
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
                        jumpCounter--;
                    }
                }
            }

            //Reset coyote counter về 0 để tránh nhảy 2 lần
            coyoteCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0;
    }



    private bool isGrounded()
    {
        //lay thong tin vat the khi cham vao
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }
    private bool onWall()
    {
        //lay thong tin vat the khi cham vao
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        //kiem tra player co o tren mat dat va o tren tuong hay khong de con ban
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}