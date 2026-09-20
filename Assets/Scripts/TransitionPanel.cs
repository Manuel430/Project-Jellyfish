using UnityEngine;
using UnityEngine.UI;

public class TransitionPanel : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] JellineMovement player;

    public void FinishTransition()
    {
        gameObject.SetActive(false);
        player.StartMoving();
    }
}
