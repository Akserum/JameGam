using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractingPlayer : FPSController
{
    #region Variables
    [Header("Inventory Infos")]
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private int maxCarriedAmount = 10;

    [Header("Interact Properties")]
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private LayerMask rayMask;
    private RaycastHit _hitInfo;
    #endregion

    #region Properties
    public PickableItem CurrentItem { get; private set; }
    public List<PickableItem> ItemList { get; private set; } = new List<PickableItem>();
    #endregion

    #region Builts_In
    private void FixedUpdate()
    {
        if (ItemList.Count >= maxCarriedAmount)
            Debug.Log("Inventory full");
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
        _inputs.currentActionMap.FindAction("Scroll").started += OnScroll;
    }

    protected override void UnsubscribeToInputs()
    {
        base.UnsubscribeToInputs();
        _inputs.currentActionMap.FindAction("PickUp").started -= OnPickUp;
        _inputs.currentActionMap.FindAction("Drop").started -= OnDrop;
        _inputs.currentActionMap.FindAction("Scroll").started -= OnScroll;
    }

    private void OnPickUp(InputAction.CallbackContext ctx) { PickUp(); }
    private void OnDrop(InputAction.CallbackContext ctx) { Drop(); }

    private void OnScroll(InputAction.CallbackContext ctx)
    {
        if (ItemList.Count <= 1)
            return;

        if (ctx.ReadValue<float>() < 0)
            GetPreviousItem();
        else
            GetNextItem();
    }
    #endregion

    #region PickUp/Drop Methods
    /// <summary>
    /// Pick up the target object
    /// </summary>
    private void PickUp()
    {
        if (ItemList.Count >= maxCarriedAmount)
            return;

        //Shoot a raycast to check if there's an object
        Ray ray = new Ray(_camera.position, _camera.forward * interactRange);
        if (!Physics.Raycast(ray.origin, ray.direction, out _hitInfo, interactRange, rayMask, QueryTriggerInteraction.Collide))
            return;

        GameObject hitObject = _hitInfo.collider.gameObject;
        if (!hitObject.TryGetComponent(out PickableItem item))
            return;

        AddObject(item);
    }

    /// <summary>
    /// Throw the object towards the player
    /// </summary>
    private void Drop()
    {
        if (!CurrentItem)
            return;

        Vector3 position = _camera.position + _camera.forward * 0.5f;
        Vector3 direction = _camera.forward;

        CurrentItem.gameObject.SetActive(true);
        CurrentItem.Drop(position, direction);
        ItemList.RemoveAt(ItemList.FindIndex(x => x == CurrentItem));

        //Select a new item if possible
        SelectItemOnDrop();
    }

    /// <summary>
    /// Add an object to the list
    /// </summary>
    private void AddObject(PickableItem item)
    {
        //Reset transform
        Transform itemTransform = item.transform;
        itemTransform.SetParent(inventoryTransform);
        itemTransform.localPosition = Vector3.zero;
        itemTransform.localRotation = Quaternion.identity;
        itemTransform.gameObject.SetActive(false);

        //Add it to the list
        CurrentItem = item;
        ItemList.Add(item);
    }
    #endregion

    #region Inventory Methods
    /// <summary>
    /// Select the item at the given index
    /// </summary>
    private void GetItemAt(int index)
    {
        if (index < 0 || index >= ItemList.Count)
            return;

        CurrentItem = ItemList.ElementAt(index);
    }

    /// <summary>
    /// Select the next object in the list
    /// </summary>
    private void GetNextItem()
    {
        int index = GetCurrentItemIndex() + 1;
        if (index >= ItemList.Count)
            index = 0;

        GetItemAt(index);
    }

    /// <summary>
    /// Get the previous object in the list
    /// </summary>
    private void GetPreviousItem()
    {
        int index = GetCurrentItemIndex() - 1;
        if (index <= -1)
            index = ItemList.Count - 1;

        GetItemAt(index);
    }

    /// <summary>
    /// Select ing a nex item when dropping another
    /// </summary>
    private void SelectItemOnDrop()
    {
        //No more objects
        if (ItemList.Count <= 0)
        {
            CurrentItem = null;
            return;
        }

        //More than 1 item
        if (ItemList.Count > 1)
            GetNextItem();
        //1 item in inventory
        else
            GetItemAt(0);
    }

    /// <summary>
    /// Return current item index in the list
    /// </summary>
    private int GetCurrentItemIndex()
    {
        return ItemList.FindIndex(x => x == CurrentItem);
    }
    #endregion
}