using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private Transform destination;

    private void OnTriggerEnter(Collider other)
    {
        // Vérifier que c'est le joueur
        if (other.CompareTag("Player"))
        {
            // Vérifier que le joueur a récupéré le micro
            if (GameManager.instance != null && GameManager.instance.hasMicro)
            {
                // Téléporter le joueur
                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc != null)
                {
                    // Désactiver temporairement le CharacterController pour éviter les collisions
                    cc.enabled = false;
                    other.transform.position = destination.position;
                    other.transform.rotation = destination.rotation;

                    // Réactiver le CharacterController dans la prochaine frame
                    StartCoroutine(ReactivateController(cc));
                }
                else
                {
                    // Si pas de CharacterController, téléportation simple
                    other.transform.position = destination.position;
                    other.transform.rotation = destination.rotation;
                }
            }
            else
            {
                Debug.Log("Téléporteur bloqué : récupérez le micro !");
            }
        }
    }

    private IEnumerator ReactivateController(CharacterController cc)
    {
        yield return null; // attendre 1 frame
        cc.enabled = true;
    }
}
