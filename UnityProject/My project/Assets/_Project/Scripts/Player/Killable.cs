using UnityEngine;
using BUT;

public class Killable : MonoBehaviour
{
    private bool isDead = false;

    [SerializeField] private Animator animator;

    public void Kill()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log(name + " est mort");

        // Animation de mort
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        // Désactiver le déplacement
        PlayerMovement movement = GetComponent<PlayerMovement>();
        if (movement != null)
        {
            movement.enabled = false;
        }

        // Désactiver les collisions
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }
}
