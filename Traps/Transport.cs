using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transport : MonoBehaviour
{
    [SerializeField] private float movementDistance;
    [SerializeField] private float speed;
    private bool movingLeft;
    private float leftEdge;
    private float rightEdge;


    private void Awake()
    {
        leftEdge = transform.position.x - movementDistance;
        rightEdge = transform.position.x + movementDistance;
    }

    private void Update()
    {
        if (movingLeft)
        {
            if (transform.position.x > leftEdge)
            {
                transform.position = new Vector3(transform.position.x - speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < rightEdge)
            {
                transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
                movingLeft = true;
        }
    }


   private void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi Player chạm vào ván trượt, đặt nó làm con của ván
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);

            // Lưu scale ban đầu của Player
            collision.transform.localScale = collision.transform.lossyScale; // lossless scale
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Khi Player rời khỏi ván trượt, bỏ cha (null)
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

}
