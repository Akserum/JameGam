using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectCollisions))]
public class PickableItem : MonoBehaviour, IPickable
{
    #region Variables/Properties
    [SerializeField] private ItemSO itemInfos;

    private Rigidbody _rb;
    private Collider _collider;
    private ObjectCollisions _collisionHandler;

    public bool IsPickable = true;
    public ItemSO ItemInfos => itemInfos;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _collisionHandler = GetComponent<ObjectCollisions>();
    }
    #endregion

    #region Interfaces Implementation
    public void PickUp()
    {
        EnableRbGravity(false);
        EnableCollider(false);
    }

    public void Drop(Vector3 position, Vector3 direction)
    {
        transform.position = position;
        EnableRbGravity(true);
        EnableCollider(true);
        _collisionHandler.AvoidCollision(true);
        _rb.AddForce(direction, ForceMode.Impulse);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Switch between gravity and kinematic mode on the rigidbody
    /// </summary>
    private void EnableRbGravity(bool enabled)
    {
        if (!_rb)
            return;

        _rb.useGravity = enabled;
        _rb.isKinematic = !enabled;
    }

    /// <summary>
    /// Enable or disable object collider
    /// </summary>
    private void EnableCollider(bool enabled)
    {
        if (!_collider)
            return;

        _collider.enabled = enabled;
    }

    /// <summary>
    /// Enable or disable the pick up
    /// </summary>
    public void BlockItem()
    {
        _collisionHandler.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    #endregion
}
