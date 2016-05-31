using UnityEngine;

public class PhotographController : InteractableController
{
    public Vector3 zoomTargetPosition;
    public Vector3 zoomTargetRotation;
    Vector3 originalPosition;
    Vector3 originalRotation;
    const float ScreenToWorldDelta = 0.0025f;
    bool isZooming;
    const float ZoomSpeed = 1000f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void StartUse()
    {
        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
        Vector3 pos = transform.position;
        pos.y = 1.1f;
        transform.position = pos;
    }

    public override void FinishUse()
    {
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;
    }

    public override void StartAltUse()
    {
        isZooming = true;
        originalPosition = gameObject.transform.position;
        originalRotation = gameObject.transform.eulerAngles;
        gameObject.transform.position = zoomTargetPosition;
        gameObject.transform.eulerAngles = zoomTargetRotation;
    }

    public override void FinishAltUse()
    {
        isZooming = false;
        gameObject.transform.position = originalPosition;
        gameObject.transform.eulerAngles = originalRotation;
    }

    public override void UpdatePos(Vector3 deltaPos)
    {
        if (!myRigidbody.isKinematic)
        {
            return;
        }
        Vector3 pos = transform.position;
        pos.x += deltaPos.x * ScreenToWorldDelta;
        if (!isZooming)
        {
            pos.z += deltaPos.y * ScreenToWorldDelta;
        }
        else
        {
            pos.y += deltaPos.y * ScreenToWorldDelta;
        }
        transform.position = pos;
    }
}
