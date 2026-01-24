using UnityEngine;
using TMPro;

public class Locker : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject micro;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI interactText;

    [Header("Détection")]
    [SerializeField] private Vector3 detectionSize = new Vector3(1f, 2f, 1f);
    [SerializeField] private LayerMask detectionLayer = ~0; // tous les layers par défaut

    private bool isOpen = false;

    private void Awake()
    {
        Debug.Log($"[Locker] Script actif sur {gameObject.name}");
        if (micro != null) micro.SetActive(false);
        if (interactText != null) interactText.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Détection du joueur avec OverlapBox
        Collider[] hits = Physics.OverlapBox(transform.position, detectionSize / 2, Quaternion.identity, detectionLayer);

        // Debug pour voir combien de colliders sont détectés
        Debug.Log($"[Locker] Nombre de hits : {hits.Length}");

        bool playerDetected = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerDetected = true;

                // Affichage du texte
                if (!isOpen && interactText != null)
                {
                    interactText.text = "Appuyez sur E pour ouvrir";
                    interactText.gameObject.SetActive(true);
                }

                // Ouverture du casier
                if (Input.GetKeyDown(KeyCode.E) && !isOpen)
                {
                    OpenLocker();
                }
            }
        }

        // Masquer le texte si aucun joueur détecté
        if (!playerDetected && interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    private void OpenLocker()
    {
        isOpen = true;

        Debug.Log("[Locker] Casier ouvert !");

        if (animator != null)
            animator.SetTrigger("OpenLocker");

        if (micro != null)
            micro.SetActive(true);

        if (interactText != null)
        {
            interactText.text = "Micro récupéré !";
            Invoke(nameof(HideText), 2f);
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.hasMicro = true;
        }
    }

    private void HideText()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    // Dessine le cube de détection pour debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, detectionSize);
    }
}
