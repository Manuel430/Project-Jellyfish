using UnityEngine;
using UnityEngine.InputSystem;

public class EnterDoor : MonoBehaviour
{
    [Header("Teleport Location")]
    [SerializeField] Transform nextPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JellineMovement playerLoc = collision.GetComponent<JellineMovement>();
            if (playerLoc != null)
            {
                playerLoc.SetTeleportLocation(nextPoint);
                playerLoc.SetEnterState(true);
                Debug.Log("Teleporting to: " +  nextPoint.position);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            JellineMovement playerLoc = collision.GetComponent <JellineMovement>();
            if (playerLoc != null)
            {
                playerLoc.SetTeleportLocation(null);
                playerLoc.SetEnterState(false);
                Debug.Log("In New Location");
            }
        }
    }
}
