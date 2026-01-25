using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WaterPickup : MonoBehaviour
{
    public int value = 1;

    private AudioSource audioSource;
    private bool collected = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (other.CompareTag("Player"))
        {
            collected = true;

            if (ScoreManager.instance != null)
                ScoreManager.instance.AddScore(value);

            audioSource.Play();

            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
