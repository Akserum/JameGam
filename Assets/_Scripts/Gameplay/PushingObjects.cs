using UnityEngine;

public class PushingObjects : MonoBehaviour
{
    #region Variables
    [SerializeField] private float forceMagnitude = 1f;
    [SerializeField] private LayerMask pushable;
    #endregion

    #region Builts_In
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;

        //No rb or not pushable object
        if (!rb || pushable != (pushable | (1 << hit.gameObject.layer)) )
            return;

        Vector3 forceDirection = hit.gameObject.transform.position - transform.position;
        forceDirection.y = 0;
        forceDirection.Normalize();

        rb.AddForceAtPosition(forceDirection * forceMagnitude, transform.position, ForceMode.Impulse);
    }
    #endregion
}
