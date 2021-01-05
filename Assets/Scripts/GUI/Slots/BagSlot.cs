using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace inventory_gui
{


	public class BagSlot : Slot
	{
        public global::Inventory.Slot slot;
		int pos = -1;

		public override void PushItem (Item newItem)
		{
            if (newItem == null)
            {
                slot.item = null;
            }
			else if (slot.item == null)
			{
				newItem.transform.SetParent (transform);
                slot.item = newItem.item;
			}
		}

		void Awake ()
		{
			int num = 0;

			foreach (char ch in transform.name)
			{
				if (!char.IsDigit (ch))
					++num;
				else
					break;
			}

			pos = int.Parse (transform.name.Substring(num));
		}
	}


}
