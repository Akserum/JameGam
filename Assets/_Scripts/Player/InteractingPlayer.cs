using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractingPlayer : FPSController
{
    #region Variables
    [Header("Inventory Infos")]
    [SerializeField] private Transform inventoryTransform;
    [SerializeField] private int maxItemAmount = 10;

    [Header("Interact Properties")]
    [SerializeField] private float interactRange = 5f;
    [SerializeField] private float throwDistance = 0.5f;
    [SerializeField] private float throwForce = 2f;
    [SerializeField] private LayerMask rayMask;
    private RaycastHit _hitInfo;

    public event Action OnInventoryChanged;
    public event Action OnSeletectedItemChanged;
    #endregion

    #region Properties
    public PickableItem SelectedItem { get; private set; }
    public PickableItem ReachableItem { get; private set; }
    public List<PickableItem> ItemList { get; private set; } = new List<PickableItem>();
    public int MaxItemAmount => maxItemAmount;
    #endregion

    #region Builts_In
    private void FixedUpdate()
    {
        GetReachableItems();
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
        if (ItemList.Count >= maxItemAmount)
            return;

        if (!ReachableItem)
            return;

        AddObject(ReachableItem);
    }

    /// <summary>
    /// Throw the object towards the player
    /// </summary>
    private void Drop()
    {
        if (!SelectedItem)
            return;

        Vector3 position = _camera.position + _camera.forward * 0.5f;
        Vector3 direction = _camera.forward * throwDistance * throwForce;

        SelectedItem.gameObject.SetActive(true);
        SelectedItem.transform.SetParent(null);
        SelectedItem.Drop(position, direction);
        ItemList.RemoveAt(ItemList.FindIndex(x => x == SelectedItem));

        //Select a new item if possible
        SelectItemOnDrop();
        RaiseModifiedInventory();
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
        SelectedItem = item;
        ItemList.Add(item);

        //Raise an event
        RaiseModifiedInventory();
        RaiseItemSelectionEvent();
    }

    /// <summary>
    /// Shoot a raycast and indicates if an item is reachable
    /// </summary>
    private void GetReachableItems()
    {
        Ray ray = new Ray(_camera.position, _camera.forward * interactRange);
        //Shoot a raycast and no return
        if (!Physics.Raycast(ray.origin, ray.direction, out _hitInfo, interactRange, rayMask, QueryTriggerInteraction.Collide))
            ReachableItem = null;

        if (!_hitInfo.collider)
            return;

        //Atleast one object hit
        GameObject hitObject = _hitInfo.collider.gameObject;
        if (!hitObject.TryGetComponent(out PickableItem item))
            ReachableItem = null;
        else
            ReachableItem = item;
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

        SelectedItem = ItemList.ElementAt(index);
        RaiseItemSelectionEvent();
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
            SelectedItem = null;
            RaiseItemSelectionEvent();
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
    public int GetCurrentItemIndex()
    {
        return ItemList.FindIndex(x => x == SelectedItem);
    }
    #endregion

    #region Events Methods
    /// <summary>
    /// Raise an event that indicates that the selected item has changed
    /// </summary>
    private void RaiseItemSelectionEvent()
    {
        OnSeletectedItemChanged?.Invoke();
    }

    /// <summary>
    /// Raise an event that indicates player inventory has changed
    /// </summary>
    private void RaiseModifiedInventory()
    {
        OnInventoryChanged?.Invoke();
    }
    #endregion
}