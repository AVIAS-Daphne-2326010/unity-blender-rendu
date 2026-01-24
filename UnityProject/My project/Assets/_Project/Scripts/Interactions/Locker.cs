using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Locker : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject micro;        // objet dans la scène
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image microUI;          // icône dans le HUD

    [Header("Détection")]
    [SerializeField] private Vector3 detectionSize = new Vector3(1f, 2f, 1f);
    [SerializeField] private LayerMask detectionLayer = ~0;

    private bool isOpen = false;

    private void Awake()
    {
        if (micro != null) micro.SetActive(false);
        if (interactText != null) interactText.gameObject.SetActive(false);
        if (microUI != null) microUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, detectionSize / 2, Quaternion.identity, detectionLayer);

        bool playerDetected = false;

        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerDetected = true;

                if (!isOpen && interactText != null)
                {
                    interactText.text = "Appuyez sur E pour ouvrir";
                    interactText.gameObject.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.E) && !isOpen)
                {
                    OpenLocker();
                }
            }
        }

        if (!playerDetected && interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private void OpenLocker()
    {
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("OpenLocker");  // clip non loopé

        if (micro != null)
            micro.SetActive(true); 

        if (microUI != null)
            microUI.gameObject.SetActive(true); // visible dans le HUD

        if (interactText != null)
        {
            interactText.text = "Micro récupéré !";
            Invoke(nameof(HideText), 2f);
        }

        if (GameManager.instance != null)
            GameManager.instance.hasMicro = true;
    }

    public void CollectMicro()
    {
        Debug.Log("CollectMicro appelée");

        if (micro != null)
            micro.SetActive(false);

        if (microUI != null)
            microUI.gameObject.SetActive(true);
    }

    private void HideText()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, detectionSize);
    }
}
