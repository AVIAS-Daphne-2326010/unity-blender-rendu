using UnityEngine;

public class TriggerTest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER AVEC : " + other.name);
    }
}
