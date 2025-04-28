using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public Item item;

    public void Initialize(Item item)
    {
        this.item = item;
    }

    public void DestroyItem()
    {
        Destroy(gameObject);
    }
}