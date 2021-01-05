using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace inventory_gui
{


	public class Item : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
		public static GameObject draggedItem {
			get;
			private set;
		}
		
		public global::Item item {
			get;
			set;
		}

		Vector3 startPosition;
		Transform startParent;

		#region IBeginDragHandler implementation

		public void OnBeginDrag (PointerEventData eventData)
		{
			draggedItem = gameObject;
			startPosition = transform.position;
			startParent = transform.parent;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}

		#endregion

		#region IDragHandler implementation

		public void OnDrag (PointerEventData eventData)
		{
            transform.position = Input.mousePosition + new Vector3(0.0f, 0.0f, -10f);
		}

		#endregion

		#region IEndDragHandler implementation

		public void OnEndDrag (PointerEventData eventData)
		{
            draggedItem = null;
			GetComponent<CanvasGroup> ().blocksRaycasts = true;

            if (transform.parent != startParent)
                startParent.GetComponent<Slot>().PushItem(null);

            transform.position = transform.parent.position;
		}

        #endregion
    }


}
