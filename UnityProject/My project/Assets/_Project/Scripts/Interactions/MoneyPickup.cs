using UnityEngine;
using TMPro;

public class MoneyPickup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winText; // texte "Vous avez gagné !"

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Argent récupéré !");

            // Affichage du texte de victoire
            if (winText != null)
            {
                winText.text = "Vous avez gagné !";
                winText.gameObject.SetActive(true);
            }

            // Appeler GameManager pour marquer la victoire
            if (GameManager.instance != null)
                GameManager.instance.WinGame();

            // Désactiver l'objet argent pour qu'il disparaisse
            gameObject.SetActive(false);
        }
    }
}
