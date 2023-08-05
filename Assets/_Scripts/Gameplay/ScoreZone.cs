using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class ScoreZone : MonoBehaviour
{
    #region Variables
    public static event Action<ItemSO> OnItemScored;
    private List<PickableItem> _scoredItems;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _scoredItems = new List<PickableItem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent(out PickableItem item))
            return;

        AddItem(item);
    }
    #endregion

    #region Methods
    /// <summary>
    /// Add an item to the list
    /// </summary>
    private void AddItem(PickableItem item)
    {
        if (_scoredItems.Contains(item))
            return;

        Debug.Log($"New item : {item.ItemInfos.ItemName} : {item.ItemInfos.Score}");

        _scoredItems.Add(item);
        OnItemScored?.Invoke(item.ItemInfos);
        item.BlockItem();
    }
    #endregion
}
