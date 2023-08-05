using UnityEngine;

[CreateAssetMenu(fileName = "New ItemDataBase", menuName = "SO/Item DataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] private ItemSO[] items;
    public ItemSO[] Items => items;
}
