using UnityEngine;

public class RoomManagement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] GameObject virtualCamera;

    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("Enemy")]
    [SerializeField] GameObject[] enemiesInRoom;

    [Header("Starting Room")]
    [SerializeField] bool isStartingRoom;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }

        if(enemiesInRoom.Length != 0)
        {
            if(isStartingRoom)
            {
                Debug.Log("Room keeps enemies");
                return;
            }
            foreach (GameObject enemy in enemiesInRoom)
            {
                int enemyIndex = System.Array.IndexOf(enemiesInRoom, enemy);
                enemiesInRoom[enemyIndex].SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.SetActive(true);

            if (enemiesInRoom.Length != 0)
            {
                foreach (GameObject enemy in enemiesInRoom)
                {
                    int enemyIndex = System.Array.IndexOf(enemiesInRoom, enemy);
                    enemiesInRoom[enemyIndex].SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.SetActive(false);

            if (enemiesInRoom.Length != 0)
            {
                foreach (GameObject enemy in enemiesInRoom)
                {
                    int enemyIndex = System.Array.IndexOf(enemiesInRoom, enemy);
                    enemiesInRoom[enemyIndex].SetActive(false);
                }
            }
        }
    }
}
