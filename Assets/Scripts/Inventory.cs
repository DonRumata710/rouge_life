using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Inventory : MonoBehaviour
{
    public class Slot
    {
        public Item item;
    }

	public List<Slot> items = new List<Slot> ();


    public void Refill()
    {
        items.Clear();

        for (int i = 0; i < 20; ++i)
        {
            items.Add(new Slot());
        }
    }


    public int PutItem(Item item)
	{
		for(int i = 0; i < items.Count; ++i)
		{
			if (items[i].item == null)
			{
				items[i].item = item;
				return i;
			}
		}
		return -1;
	}

	public bool PutItem(Item item, int pos)
	{
		if (items[pos].item == null)
			return false;

		items [pos].item = item;
		return true;
	}

	public Item GetItem(int pos)
	{
		Item item = items[pos].item;
		items[pos].item = null;
		return item;
	}

	public Item LookItem(int pos)
	{
		return items[pos].item;
	}
}
