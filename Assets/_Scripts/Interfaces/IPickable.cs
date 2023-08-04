using UnityEngine;

public interface IPickable
{
    public void PickUp();
    public void Drop(Vector3 position, Vector3 direction);
}
