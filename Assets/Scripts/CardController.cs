using UnityEngine;

public class CardController : InteractableController
{
    Rigidbody myRigidbody;
    const float ScreenToWorldDelta = 0.0025f;
    bool isFacingDown = true;
    bool isInUse;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isInUse)
        {
            return;
        }
        Vector3 targetUp = isFacingDown ? Vector3.up : Vector3.down;
        if (Vector3.Distance(gameObject.transform.up, targetUp) < 1f)
        {
            //Rotate code
            //gameObject.transform.up = isFacingDown ? Vector3.down : Vector3.up;

        }
    }

    public override void StartUse()
    {
        isInUse = true;
        myRigidbody.useGravity = false;
        Vector3 pos = transform.position;
        pos.y = 1.1f;
        transform.position = pos;
    }

    public override void FinishUse()
    {
        isInUse = false;
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
