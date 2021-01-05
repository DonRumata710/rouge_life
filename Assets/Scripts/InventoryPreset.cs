using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryPreset : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        Inventory inventory = GetComponent<Inventory>();

        if (inventory != null)
        {
            inventory.Refill();

            for (int i = 0; i < items.Count && i < inventory.items.Count; ++i)
            {
                inventory.items[i].item = items[i];
            }
        }
    }
}
