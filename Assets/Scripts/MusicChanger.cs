using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField]AudioSource musicManager;
    [SerializeField]AudioClip bossMusic;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            musicManager.Stop();

            musicManager.clip = bossMusic;
            musicManager.Play();

            gameObject.SetActive(false);
        }
    }
}
