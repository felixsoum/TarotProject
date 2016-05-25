using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public GameEventHandler gameEventHandler = null;

    public virtual void StartUse() {}

    public virtual void FinishUse() {}

    public virtual void StartAltUse()
    {
        if( gameEventHandler != null )
        {
            gameEventHandler.OnInteractionAltUse(this);
        }
    }

    public virtual void UpdatePos(Vector3 deltaPos) {}
}
