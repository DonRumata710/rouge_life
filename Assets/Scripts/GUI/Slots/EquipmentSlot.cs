using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace inventory_gui
{


	public class EquipmentSlot : Slot
	{
		public override void PushItem (Item newItem)
		{
            if (newItem != null)
            {
                if (transform.parent.parent.GetComponent<Inventory>().CheckCompability(newItem.item as Equipment))
                {
                    if (slot.item != null)
                    {
                        Slot foreignSlot = newItem.transform.parent.GetComponent<Slot>();
                        foreignSlot.PushItem(null);
                        foreignSlot.PushItem(item.GetComponent<Item>());
                        EquipmentSlot equipmentForeignSlot = foreignSlot as EquipmentSlot;
                        if (equipmentForeignSlot.slot.item == null)
                        {
                            foreignSlot.PushItem(newItem);
                            return;
                        }
                    }

                    Equipment equipment = newItem.item as Equipment;
                    if (equipment != null)
                    {
                        slot.item = equipment;

                        if (slot.item != null)
                        {
                            newItem.transform.SetParent(transform);
                            transform.GetChild(0).gameObject.SetActive(false);
                        }
                    }
                }
            }
            else
            {
                slot.item = null;
                transform.GetChild(0).gameObject.SetActive(true);
            }
		}

		public global::EquipmentSlot slot;
	}


}
