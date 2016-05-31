using UnityEngine;

public class KnifeController : InteractableController
{
    public float throwForce = 10f;
    const float ScreenToWorldDelta = 0.0025f;
    bool isThrown;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        base.Update();
        if (isThrown)
        {
            return;
        }
    }

    public override void StartUse()
    {
        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
        Vector3 pos = transform.position;
        pos.y = 1.1f;
        transform.position = pos;
        transform.eulerAngles = new Vector3(270, 270, 0);
    }

    public override void FinishUse()
    {
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;
    }

    public override void StartAltUse()
    {
        base.StartAltUse();
        isThrown = true;
        FinishUse();
        myRigidbody.AddForce(throwForce * Vector3.forward, ForceMode.Impulse);
    }

    public override void ResetPosition()
    {
        base.ResetPosition();
        isThrown = false;
    }

    public override void UpdatePos(Vector3 deltaPos)
    {
        if (isThrown || !myRigidbody.isKinematic)
        {
            return;
        }
        Vector3 pos = transform.position;
        pos.x += deltaPos.x * ScreenToWorldDelta;
        pos.z += deltaPos.y * ScreenToWorldDelta;
        transform.position = pos;
    }
}
