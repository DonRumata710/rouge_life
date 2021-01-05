using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace inventory_gui
{


	public class InventoryController : MonoBehaviour
	{
		GameObject inventoryObject;
		Inventory invectoryComponent;

		void Start()
		{
			inventoryObject = transform.Find ("Inventory").gameObject;
			invectoryComponent = inventoryObject.GetComponent<Inventory> ();
		}

		void Update ()
		{
			if (Input.GetButtonDown ("Inventory"))
			{
				Toggle ();
			}
		}

		public void CloseInventory ()
		{
			inventoryObject.SetActive (false);
		}

		public void Toggle ()
		{
			inventoryObject.SetActive (!inventoryObject.activeSelf);
		}
	}


}
