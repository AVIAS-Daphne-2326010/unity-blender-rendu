using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class MoneyPickup : MonoBehaviour
{
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
            Debug.Log("Butin récupéré !");

            if (GameManager.instance != null)
                GameManager.instance.WinGame();

            // Jouer le son
            audioSource.Play();

            // Désactiver visuellement l'argent
            GetComponent<Collider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;

            // Détruire après la fin du son
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}
