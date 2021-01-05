using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlot
{
	private Equipment equipment;
    private uint type;
    
    public void SetType(Item.Type _type)
    {
        type = (uint)_type;
    }

    public Equipment item
	{
		get
        {
			return equipment;
		}
        set
        {
            if (value != null && ((uint)value.type & type) == 0)
                return;
            
            equipment = value;
            OnExchange();
        }
    }
    
    public delegate void EquipmentExchange();
    
    public event EquipmentExchange OnExchange;
}
