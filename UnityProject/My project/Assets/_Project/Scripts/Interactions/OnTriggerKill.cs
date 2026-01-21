using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerKill : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Killable victim = other.GetComponent<Killable>();
        if (victim != null)
        {
            victim.Kill();
        }
    }
}
