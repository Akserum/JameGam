using UnityEngine;

public interface IPickable
{
    public void PickUp();
    public void Drop();
    public Transform GetTransform();
}
