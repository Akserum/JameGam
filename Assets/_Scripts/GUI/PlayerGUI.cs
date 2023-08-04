using UnityEngine;
using System.Linq;
using TMPro;

public class PlayerGUI : MonoBehaviour
{
    #region Variables
    [Header("Slots properties")]
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private Transform slotParent;
    [SerializeField] private ItemSlot slotPrefab;
    [SerializeField] private Color slotColor = Color.black;
    [SerializeField] private Color selectedColor = Color.white;

    private ItemSlot[] _slots;
    private ItemSlot _selectedSlot;
    private InteractingPlayer _player;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _player = FindObjectOfType<InteractingPlayer>();
    }

    private void Start()
    {
        CreateSlots();
        FillSlots();
    }

    private void OnEnable()
    {
        _player.OnInventoryChanged += FillSlots;
        _player.OnSeletectedItemChanged += ShowSelectedItem;
    }

    private void OnDisable()
    {
        _player.OnInventoryChanged -= FillSlots;
        _player.OnSeletectedItemChanged -= ShowSelectedItem;
    }
    #endregion

    #region Methods
    /// <summary>
    /// Instantiates all the slots for the inventory
    /// </summary>
    private void CreateSlots()
    {
        _slots = new ItemSlot[_player.MaxItemAmount];
        for (int i = 0; i < _slots.Length; i++)
        {
            ItemSlot instance = Instantiate(slotPrefab, slotParent);
            SetSlotColor(instance, false);
            _slots[i] = instance;
        }
    }

    /// <summary>
    /// Fill slots with icon sprite
    /// </summary>
    private void FillSlots()
    {
        int itemCount = _player.ItemList.Count;
        for (int i = 0; i < _slots.Length; i++)
        {
            if (i < itemCount)
            {
                ItemSO item = _player.ItemList.ElementAt(i).ItemInfos;
                _slots[i].SetNewSprite(item.Icon);
            }
            else
                _slots[i].EmptySlot();
        }
    }

    /// <summary>
    /// Indicates the name of the selected item
    /// </summary>
    private void ShowSelectedItem()
    {
        string name = _player.SelectedItem ? _player.SelectedItem.ItemInfos.ItemName : "";
        int index = _player.GetCurrentItemIndex();

        //Update text
        nameField.text = name;

        //Select a new slot
        if (_selectedSlot)
            SetSlotColor(_selectedSlot, false);
        
        //Select a new slot
        if (_player.SelectedItem)
        {
            _selectedSlot = _slots[index];
            SetSlotColor(_selectedSlot, true);
        }
    }

    /// <summary>
    /// Modify the color based on the selection boolean
    /// </summary>
    private void SetSlotColor(ItemSlot slot, bool selected)
    {
        Color color = selected ? selectedColor : slotColor;
        slot.SetColor(color);
    }
    #endregion
}
