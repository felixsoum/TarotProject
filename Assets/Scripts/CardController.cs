using UnityEngine;

public class CardController : InteractableController
{
    Rigidbody myRigidbody;
    const float ScreenToWorldDelta = 0.0025f;
    bool isFacingDown = true;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public override void StartUse()
    {
        myRigidbody.useGravity = false;
        Vector3 pos = transform.position;
        pos.y = 1.1f;
        transform.position = pos;
    }

    public override void FinishUse()
    {
        myRigidbody.useGravity = true;
    }

    public override void StartAltUse()
    {
        gameObject.transform.up = isFacingDown ? Vector3.down : Vector3.up;
        isFacingDown = !isFacingDown;
    }

    public override void UpdatePos(Vector3 deltaPos)
    {
        Vector3 pos = transform.position;
        pos.x += deltaPos.x * ScreenToWorldDelta;
        pos.z += deltaPos.y * ScreenToWorldDelta;
        transform.position = pos;
    }
}
