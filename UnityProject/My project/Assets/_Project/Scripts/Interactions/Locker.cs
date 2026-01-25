using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class Locker : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private GameObject micro;
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI interactText;
    [SerializeField] private Image microUI;

    [Header("Détection")]
    [SerializeField] private Vector3 detectionSize = new Vector3(1f, 2f, 1f);
    [SerializeField] private LayerMask detectionLayer = ~0;

    private bool isOpen = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (micro != null) micro.SetActive(false);
        if (interactText != null) interactText.gameObject.SetActive(false);
        if (microUI != null) microUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        Collider[] hits = Physics.OverlapBox(
            transform.position,
            detectionSize / 2,
            Quaternion.identity,
            detectionLayer
        );

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
                    StartCoroutine(OpenLockerAfterAnimation());
                }
            }
        }

        if (!playerDetected && interactText != null)
            interactText.gameObject.SetActive(false);
    }

    private IEnumerator OpenLockerAfterAnimation()
    {
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("OpenLocker");

        // Afficher le micro pendant l'animation
        if (micro != null)
            micro.SetActive(true);

        // Attendre la fin de l'animation
        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // Récupération du micro
        if (micro != null)
            micro.SetActive(false);

        if (microUI != null)
            microUI.gameObject.SetActive(true);

        // Jouer le son du micro
        if (audioSource != null && audioSource.clip != null)
            audioSource.Play();

        if (interactText != null)
        {
            interactText.text = "Micro récupéré !";
            Invoke(nameof(HideText), 2f);
        }

        if (GameManager.instance != null)
            GameManager.instance.hasMicro = true;
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
