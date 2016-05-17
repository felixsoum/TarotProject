using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public LayerMask interactableLayer;
    InteractableController currentInteractable;
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
                currentInteractable = hitHinfo.transform.gameObject.GetComponent<InteractableController>();
                currentInteractable.StartUse();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isInteracting = false;
            if (currentInteractable != null)
            {
                currentInteractable.FinishUse();
            }
        }
        else if (isInteracting && Input.GetMouseButton(0))
        {
            Vector3 deltaPos = Input.mousePosition - previousMousePos;
            previousMousePos = Input.mousePosition;
            if (currentInteractable != null)
            {
                currentInteractable.UpdatePos(deltaPos);
                if (Input.GetMouseButtonDown(1))
                {
                    currentInteractable.StartAltUse();
                }
            }
        }
        Cursor.visible = !isInteracting;
	}
}
