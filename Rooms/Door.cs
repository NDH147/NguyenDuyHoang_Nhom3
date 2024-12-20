using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    /*    [SerializeField] private Transform previousRoom;
        [SerializeField] private Transform nextRoom;*/
    [SerializeField] private CameraController cam;
    private UIManager uiManager;
    

    private void Awake()
    {
        cam = Camera.main.GetComponent<CameraController>();
        uiManager = FindObjectOfType<UIManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

                uiManager.GameWin();
                return;
            // else
            // {
            //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            // }

            /*if (collision.transform.position.x < transform.position.x)
            {
                cam.MoveToNewRoom(nextRoom);
                nextRoom.GetComponent<Room>().ActivateRoom(true);
                previousRoom.GetComponent<Room>().ActivateRoom(false);
            }
            else
            {
                cam.MoveToNewRoom(previousRoom);
                previousRoom.GetComponent<Room>().ActivateRoom(true);
                nextRoom.GetComponent<Room>().ActivateRoom(false);
            }*/
        }
    }
}