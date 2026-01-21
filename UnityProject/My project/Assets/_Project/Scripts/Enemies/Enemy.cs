using UnityEngine;

public class Enemy : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Killable killable = other.GetComponent<Killable>();
            if (killable)
            {
                killable.Kill();
                GameManager.instance.PlayerDied();
            }
        }
    }
}
