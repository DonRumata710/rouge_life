using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace inventory_gui
{


	public abstract class Slot : MonoBehaviour, IDropHandler
	{
		public GameObject item {
			get {
				if (transform.childCount > 0)
					return transform.GetChild (0).gameObject;
				return null;
			}
		}

		void Awake ()
		{
			inventory = transform.parent.parent.GetComponent<Inventory> ();
		}

		protected Inventory inventory;

		#region IDropHandler inplementation

		public void OnDrop (PointerEventData eventData)
		{
			PushItem (Item.draggedItem.GetComponent<Item> ());
		}

		#endregion

		public abstract void PushItem (Item newItem);
	}


}
