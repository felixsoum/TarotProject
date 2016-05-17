using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public virtual void StartUse() {}

    public virtual void FinishUse() {}

    public virtual void StartAltUse() { }

    public virtual void UpdatePos(Vector3 deltaPos) {}
}
