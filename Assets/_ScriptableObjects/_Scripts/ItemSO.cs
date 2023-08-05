using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "SO/Item")]
public class ItemSO : ScriptableObject
{
    #region Variables/Properties
    [SerializeField] private string itemName;
    [SerializeField] private int score;
    [SerializeField] private Sprite icon;
    [SerializeField] private GameObject prefab;

    public string ItemName => itemName;
    public int Score => score;
    public Sprite Icon => icon;
    public GameObject Prefab => prefab;
    #endregion
}
