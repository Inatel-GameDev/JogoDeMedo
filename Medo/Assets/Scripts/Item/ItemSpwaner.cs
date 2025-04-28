using System;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance { get; private set; }

    private void Awake() => Instance = this;

    public void SpawnItem(Item item, Vector3 position)
    {
        if(item.worldPrefab != null)
        {
            GameObject newItem = Instantiate(item.worldPrefab, position, Quaternion.identity);
            newItem.GetComponent<WorldItem>().Initialize(item);
        }
    }

    public void DestroyItem(GameObject worldItem)
    {
        Destroy(worldItem);
    }
}