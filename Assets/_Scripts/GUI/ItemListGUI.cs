using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class ItemListGUI : MonoBehaviour
{
    #region Variables
    [Header("Inputs options")]
    [SerializeField] private InputActionAsset input;

    [Header("ItemList GUI")]
    [SerializeField] private GameObject listElementPrefab;
    [SerializeField] private Transform contentParent;
    private GameObject _listParent;

    private GameManager _gameManager;
    private TextMeshProUGUI[] _listELements;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _gameManager = GameManager.Instance;
        _listParent = contentParent.transform.parent.gameObject;
    }

    private void Start()
    {
        CreatListGUI();
    }

    private void OnEnable()
    {
        ScoreZone.OnItemScored += CheckElementInList;
        input.FindAction("ShowList").performed += ShowList;
    }

    private void OnDisable()
    {
        ScoreZone.OnItemScored -= CheckElementInList;
        input.FindAction("ShowList").performed -= ShowList;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Enable or disable the list GUI
    /// </summary>
    private void ShowList(InputAction.CallbackContext ctx)
    {
        _listParent.SetActive(!_listParent.activeSelf);
    }

    /// <summary>
    /// Create composed by required items to collect
    /// </summary>
    private void CreatListGUI()
    {
        _listELements = new TextMeshProUGUI[_gameManager.RequiredItems.Length];
        for (int i = 0; i < _listELements.Length; i++)
        {
            ItemSO item = _gameManager.RequiredItems[i];
            GameObject instance = Instantiate(listElementPrefab, contentParent);
            TextMeshProUGUI textMesh = instance.GetComponent<TextMeshProUGUI>();

            textMesh.text = item.ItemName;
            _listELements[i] = textMesh;
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
        foreach (TextMeshProUGUI element in _listELements)
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
