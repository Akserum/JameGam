using UnityEngine;
using UnityEngine.InputSystem;

public class InteractingPlayer : FPSController
{
    #region Variables
    [Header("Interact Properties")]
    [SerializeField] private Transform handTransform;
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask rayMask;
    private RaycastHit _hitInfo;
    #endregion

    #region Properties
    public Transform CarriedItem { get; private set; }
    #endregion

    #region Builts_In
    private void FixedUpdate()
    {
        if (!CanPickUp())
            return;

        Debug.Log("Press E to pick up object");
    }

    private void OnDrawGizmos()
    {
        if (_camera)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_camera.position, _camera.forward * interactRange);
        }

    }
    #endregion

    #region Inputs Methods
    protected override void SubscribeToInputs()
    {
        base.SubscribeToInputs();
        _inputs.currentActionMap.FindAction("PickUp").started += OnPickUp;
        _inputs.currentActionMap.FindAction("Drop").started += OnDrop;
    }

    protected override void UnsubscribeToInputs()
    {
        base.UnsubscribeToInputs();
        _inputs.currentActionMap.FindAction("PickUp").started -= OnPickUp;
        _inputs.currentActionMap.FindAction("Drop").started -= OnDrop;
    }

    private void OnPickUp(InputAction.CallbackContext ctx) { PickUp(); }
    private void OnDrop(InputAction.CallbackContext ctx) { Drop(); }    
    #endregion

    #region PickUp/Drop Methods
    /// <summary>
    /// Pick up the target object
    /// </summary>
    private void PickUp()
    {
        if (!CanPickUp())
            return;

        Transform newItem = _hitInfo.collider.gameObject.transform;

        //If already carrying an item, drop it
        if (CarriedItem)
            Drop();

        //Set the new carried item
        CarriedItem = newItem;
        CarriedItem.SetParent(handTransform);
        CarriedItem.localPosition = Vector3.zero;
        GetPickableInterface().PickUp();
    }

    /// <summary>
    /// Drop the last object picked up
    /// </summary>
    private void Drop()
    {
        CarriedItem.SetParent(null);
        CarriedItem.position = _camera.position + _camera.forward;
        GetPickableInterface().Drop();
        CarriedItem = null;
    }

    /// <summary>
    /// Indicates if the player can pick up an object
    /// </summary>
    private bool CanPickUp()
    {
        //Shoot a raycast to check if there's an object
        Ray ray = new Ray(_camera.position, _camera.forward * interactRange);
        if (Physics.Raycast(ray.origin, ray.direction, out _hitInfo, interactRange, rayMask, QueryTriggerInteraction.Collide))
        {
            GameObject hitObject = _hitInfo.collider.gameObject;
            return hitObject.TryGetComponent(out IPickable item);
        }

        return false;
    }

    /// <summary>
    /// Return the pickable interface of the picked up object
    /// </summary>
    private IPickable GetPickableInterface()
    {
        return CarriedItem.GetComponent<IPickable>();
    }
    #endregion
}