using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killable : MonoBehaviour
{

    // Update is called once per frame
    public void Kill()
    {
        Debug.Log(name + " est mort");
    }
}
