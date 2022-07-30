using System;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    public event OnLedgeDetectDelegate OnLedgeDetect;
    public delegate void OnLedgeDetectDelegate(Vector3 ledgeForward, Vector3 closestPoint);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ledge"))
        {
            Vector3 ledgeForward = other.transform.forward;
            Vector3 closestPoint = other.ClosestPointOnBounds(other.transform.position);
            OnLedgeDetect?.Invoke(ledgeForward, closestPoint);
        }
    }
}
