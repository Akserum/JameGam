using UnityEngine;

public class ObjectCollisions : MonoBehaviour
{
    #region Variables
    [Header("Layer options")]
    [SerializeField] private string baseLayer = "Item";
    [SerializeField] private string insidePlayer = "InsidePlayer";

    [Header("Player Detection")]
    [SerializeField] private bool showHelpers = true;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField] private Vector3 size = Vector3.one;
    #endregion

    #region Buitls_In
    private void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer(baseLayer);
    }

    private void FixedUpdate()
    {
        HandleCollisionLayer();
    }

    private void OnDrawGizmosSelected()
    {
        if (!showHelpers)
            return;

        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(Vector3.zero + offset, size);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Modify the layer if the object is inside the player
    /// </summary>
    private void HandleCollisionLayer()
    {
        //The player is already outside the object box
        if (gameObject.layer == GetLayer(baseLayer))
            return;

        Vector3 center = transform.position + offset;
        Vector3 halfExtends = size / 2;
        int layerMask = (1 << LayerMask.NameToLayer("Player"));

        //Detects player inside the object
        if (Physics.CheckBox(center, halfExtends, transform.rotation, layerMask))
        {
            if (gameObject.layer.ToString() != insidePlayer)
                AvoidCollision(true);

            return;
        }

        //Player is no longer inside the object
        AvoidCollision(false);
    }

    /// <summary>
    /// Modify this object layer based on the boolean parameter
    /// </summary>
    /// <param name="isInside"> Indicates if its inside the player or not </param>
    public void AvoidCollision(bool isInside)
    {
        string layer = isInside ? insidePlayer : baseLayer;
        gameObject.layer = GetLayer(layer);
    }

    //Return the int value of the target layer
    private int GetLayer(string layer)
    {
        return LayerMask.NameToLayer(layer);
    }
    #endregion
}