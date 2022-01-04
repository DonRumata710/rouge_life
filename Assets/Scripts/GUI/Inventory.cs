using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace inventory_gui
{


	public class Inventory : MonoBehaviour
	{
		public GameObject itemObject;
		public GameObject defaultItemObject;
		public Sprite[] defaultIcons;

		EquipmentSlot[] equipmentSlots;
		BagSlot[] inventorySlots;

        Text health;
        Text maxHealth;
        Text armor;
        Text damage;

		HumanoidController player;


		public CommonCharacterController.SimpleEvent OnInventoryChanging;


		void Awake ()
		{
			Transform equipment = transform.Find ("Equipment").transform;
			equipmentSlots = new EquipmentSlot[equipment.childCount];

            for (int i = 0; i < equipmentSlots.Length; ++i)
                equipmentSlots[i] = equipment.GetChild(i).gameObject.GetComponent<EquipmentSlot>();

			Transform bag = transform.Find ("Bag").transform;
			inventorySlots = new BagSlot[bag.childCount];

			for (int i = 0; i < inventorySlots.Length; ++i)
				inventorySlots[i] = bag.GetChild (i).gameObject.GetComponent<BagSlot> ();

            Transform stats = transform.Find("Stats").transform;
            health = stats.Find("Health").gameObject.GetComponent<Text>();
            maxHealth = stats.Find("Max health").gameObject.GetComponent<Text>();
            armor = stats.Find("Armor").gameObject.GetComponent<Text>();
            damage = stats.Find("Damage").gameObject.GetComponent<Text>();
        }

		void OnEnable ()
		{
			PauseManager.instance.Pause ();

			player = GameObject.FindGameObjectWithTag ("Player").GetComponent<HumanoidController> ();
			GetEquipment (player.equipment);
			GetInventory (player.inventory.items);
            GetStats ();
		}

		void OnDisable ()
		{
			DestroyEquipment();
			DestroyInventory();

			PauseManager.instance.Play ();
		}

		public void GetEquipment (EquipmentSet equipment)
		{
			for (int i = 0; i < equipmentSlots.Length; ++i)
			{
				if (equipmentSlots[i].transform.childCount != 0)
					Destroy (equipmentSlots[i].transform.GetChild (0).gameObject);
			}

			if (equipment == null)
			{
				for (int i = 0; i < equipmentSlots.Length; ++i)
					CreateDefaultIcon (i);
			}
			else
			{
				CreateIcon (equipment.rightHand, 0);
				CreateIcon (equipment.helmet, 1);
				CreateIcon (equipment.platebody, 2);
				CreateIcon (equipment.pants, 3);
				CreateIcon (equipment.shoes, 4);
				CreateIcon (equipment.leftHand, 5);
				CreateIcon (equipment.necklace, 6);
				CreateIcon (equipment.ring1, 7);
				CreateIcon (equipment.ring2, 8);
				CreateIcon (equipment.quiver, 9);
				CreateIcon (equipment.cloak, 10);
				CreateIcon (equipment.gloves, 11);
				CreateIcon (equipment.belt, 12);
			}
		}

		public void GetInventory (List<global::Inventory.Slot> items)
		{
			if (items != null)
			{
				for (int i = 0; i < items.Count; ++i)
				{
                    inventorySlots[i].slot = items[i];

                    global::Item item = items[i].item;
					if (item != null)
					{
						GameObject itemIcon = Instantiate (itemObject);
						Image image = itemIcon.GetComponent<Image> ();
						image.sprite = item.icon;

                        if ((uint)item.type < (uint)global::Item.Type.OTHER && !CheckCompability((Equipment)item))
                            image.color = Color.red;
                        else
                            image.color = Color.grey;

						itemIcon.transform.position = inventorySlots [i].transform.position;
						itemIcon.transform.SetParent (inventorySlots [i].transform);
						itemIcon.GetComponent<inventory_gui.Item> ().item = item;
                    }
				}
			}
		}

		private void DestroyEquipment()
		{
			foreach (EquipmentSlot slot in equipmentSlots)
			{
				foreach (Transform obj in slot.transform)
				{
					Destroy(obj.gameObject);
				}
			}
		}

		private void DestroyInventory()
        {
			foreach(BagSlot slot in inventorySlots)
			{
				foreach(Transform obj in slot.transform)
				{
					Destroy(obj.gameObject);
				}
			}
        }

        public void GetStats ()
        {
            health.text = player.Parameters.Health.ToString();
            maxHealth.text = player.Parameters.MaxHealth.ToString();
            armor.text = player.Parameters.Armor.ToString();
            damage.text = (player.Parameters.RightHandDamage + player.Parameters.LeftHandDamage).ToString();
        }

        public bool CheckCompability(Equipment equipment)
        {
            if (equipment == null)
                return false;

            return player.CheckCompability(equipment);
        }
        
		void CreateDefaultIcon (int i)
		{
			GameObject item = Instantiate (defaultItemObject);
			item.transform.position = equipmentSlots [i].transform.position;
			item.transform.SetParent (equipmentSlots [i].transform);

			Image image = item.GetComponent<Image> ();
			image.color = Color.grey;
			image.sprite = defaultIcons [i];
        }

        void CreateIcon (global::EquipmentSlot equipment, int i)
		{
			CreateDefaultIcon(i);

			if (equipment == null)
            {
				Debug.LogError("Null pointer for equimpent");
				throw new UnityException("Null pointer for equimpent");
            }

            equipment.OnExchange += GetStats;
            equipmentSlots[i].slot = equipment;

			if (equipment.item == null)
				return;

			GameObject item;
			Image image;

			item = Instantiate(itemObject);
			image = item.GetComponent<Image>();
			image.sprite = equipment.item.icon;

			image.color = Color.grey;
			item.transform.position = equipmentSlots [i].transform.position;
			item.transform.SetParent (equipmentSlots [i].transform);

        }
	}


}
