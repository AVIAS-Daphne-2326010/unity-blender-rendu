using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationSpeed = new Vector3(0, 50f, 0);

    void Update()
    {
        // Rotation autour de chaque axe selon rotationSpeed
        transform.Rotate(rotationSpeed * Time.deltaTime);
    }
}