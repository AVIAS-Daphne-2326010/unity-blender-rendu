// Teleporter.cs
using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private Transform destination;

    private bool canTeleport = true; 

    private void OnTriggerEnter(Collider other)
    {
        if (!canTeleport) return;

        if (other.CompareTag("Player"))
        {
            if (GameManager.instance != null && GameManager.instance.hasMicro)
            {
                CharacterController cc = other.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                    other.transform.position = destination.position;
                    other.transform.rotation = destination.rotation;
                    StartCoroutine(ReactivateController(cc));
                }
                else
                {
                    other.transform.position = destination.position;
                    other.transform.rotation = destination.rotation;
                }

                // Empêche le retour immédiat
                canTeleport = false;
                StartCoroutine(ResetTeleportCooldown());
            }

            else
            {
                Debug.Log("Téléporteur bloqué : récupérez le micro !");
            }
        }
    }

    private IEnumerator ReactivateController(CharacterController cc)
    {
        yield return null;
        cc.enabled = true;
    }

    private IEnumerator ResetTeleportCooldown()
    {
        yield return new WaitForSeconds(0.5f); 
        canTeleport = true;
    }
}
