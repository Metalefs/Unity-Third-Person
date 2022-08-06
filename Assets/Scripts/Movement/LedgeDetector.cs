using System;
using UnityEngine;
namespace Movement
{
    public class LedgeDetector : MonoBehaviour
    {
        public event OnLedgeDetectDelegate OnLedgeDetect;
        public delegate void OnLedgeDetectDelegate(Vector3 ledgeForward, Vector3 closestPoint);

        private void OnTriggerEnter(Collider other)
        {
            OnLedgeDetect?.Invoke(other.transform.forward, other.ClosestPointOnBounds(transform.position));
        }
    }
}