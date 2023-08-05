using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    #region Variables
    [SerializeField] private Image slotImage;
    [SerializeField] private Image iconImage;
    #endregion

    #region Methods
    /// <summary>
    /// Set a new sprite on the image
    /// </summary>
    public void SetNewSprite(Sprite sprite)
    {
        if (!iconImage || !sprite)
            return;

        iconImage.enabled = true;
        iconImage.sprite = sprite;
    }

    /// <summary>
    /// Remove the sprite from the image
    /// </summary>
    public void EmptySlot()
    {
        iconImage.enabled = false;
    }

    /// <summary>
    /// Set the color of the slot image
    /// </summary>
    public void SetColor(Color color)
    {
        if (!slotImage)
            return;

        slotImage.color = color;
    }
    #endregion
}