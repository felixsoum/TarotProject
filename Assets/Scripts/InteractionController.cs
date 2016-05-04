using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public LayerMask interactableLayer;
    CardController card;
    Vector3 previousMousePos;
    bool isInteracting;

	void Update()
    {
	    if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitHinfo;
            if (Physics.Raycast(ray, out hitHinfo, 5f, interactableLayer))
            {
                isInteracting = true;
                previousMousePos = Input.mousePosition;
                card = hitHinfo.transform.gameObject.GetComponent<CardController>();
                card.StartUse();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isInteracting = false;
            card.FinishUse();
        }
        else if (isInteracting && Input.GetMouseButton(0))
        {
            Vector3 deltaPos = Input.mousePosition - previousMousePos;
            previousMousePos = Input.mousePosition;
            card.MoveCard(deltaPos);
        }
        Cursor.visible = !isInteracting;
	}
}
