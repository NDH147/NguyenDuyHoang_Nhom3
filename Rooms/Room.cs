using UnityEngine;

public class Room : MonoBehaviour
{
    //lưu tất cả enermy của 1 room
    [SerializeField] private GameObject[] enemies;
    //chứa vị trí ban đầu của các enermy
    private Vector3[] initialPosition;

    private void Awake()
    {
        // Lưu vị trí ban đầu của các enermy
        initialPosition = new Vector3[enemies.Length];
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
                initialPosition[i] = enemies[i].transform.position;
        }

        //Deactivate rooms
        if (transform.GetSiblingIndex() != 0)
            ActivateRoom(false);
    }
    public void ActivateRoom(bool _status)
    {
        //Activate/deactivate enemies
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].SetActive(_status);
                //đặt lại ví trí cho các enermy
                enemies[i].transform.position = initialPosition[i];
            }
        }
    }
}