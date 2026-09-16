using UnityEngine;

public class RoomManagement : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] GameObject virtualCamera;

    [Header("Player")]
    [SerializeField] GameObject player;

    //Add enemy spawn system later

    private void Awake()
    {
        //virtualCamera.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            virtualCamera.SetActive(false);
        }
    }
}
