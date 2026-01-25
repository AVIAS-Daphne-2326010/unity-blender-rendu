using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class MoneyPickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText; 

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
            Debug.Log("Argent récupéré !");

            // Affichage du texte de victoire
            if (winText != null)
            {
                winText.text = "Vous avez gagné !";
                winText.gameObject.SetActive(true);
            }

            // Victoire
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
