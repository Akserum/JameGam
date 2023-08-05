using UnityEngine;
using TMPro;

public class ItemListGUI : MonoBehaviour
{
    #region Variables
    [Header("ItemList GUI")]
    [SerializeField] private GameObject listElementPrefab;
    [SerializeField] private Transform contentParent;

    private GameManager _gameManager;
    private TextMeshProUGUI[] _list;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _gameManager = GameManager.Instance;
    }

    private void Start()
    {
        CreatListGUI();
    }

    private void OnEnable()
    {
        ScoreZone.OnItemScored += CheckElementInList;
    }

    private void OnDisable()
    {
        ScoreZone.OnItemScored -= CheckElementInList;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Create composed by required items to collect
    /// </summary>
    private void CreatListGUI()
    {
        _list = new TextMeshProUGUI[_gameManager.RequiredItems.Length];
        for (int i = 0; i < _list.Length; i++)
        {
            ItemSO item = _gameManager.RequiredItems[i];
            GameObject instance = Instantiate(listElementPrefab, contentParent);
            TextMeshProUGUI textMesh = instance.GetComponent<TextMeshProUGUI>();

            textMesh.text = item.ItemName;
            _list[i] = textMesh;
        }
    }

    /// <summary>
    /// Check if an element is in the list
    /// </summary>
    private void CheckElementInList(ItemSO item)
    {
        Transform element = GetElementFromList(item.ItemName);

        if (!element)
            return;

        element.GetChild(0).gameObject.SetActive(true);
    }

    /// <summary>
    /// Get a textmesh from the list
    /// </summary>
    private Transform GetElementFromList(string text)
    {
        foreach (TextMeshProUGUI element in _list)
        {
            if (element.text == text)
                return element.transform;
            else 
                continue;
        }

        return null;
    }
    #endregion
}
