using UnityEngine;

public class CardController : InteractableController
{
    Rigidbody myRigidbody;
    const float ScreenToWorldDelta = 0.0025f;
    bool isFacingDown = true;
    bool isFlipping;
    const float FlipSpeed = 1000f;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!isFlipping)
        {
            return;
        }
        Vector3 targetUp = isFacingDown ? Vector3.up : Vector3.down;
        float distance = Vector3.Distance(gameObject.transform.up, targetUp);
        if (distance > 0.25f)
        {
            gameObject.transform.Rotate(0, 0, FlipSpeed*Time.deltaTime);
        }
        else
        {
            Vector3 rot = gameObject.transform.eulerAngles;
            rot.z = isFacingDown ? 0 : 180;
            gameObject.transform.eulerAngles = Vector3.Lerp(gameObject.transform.eulerAngles, rot, 100f*Time.deltaTime);
        }
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
        isFlipping = false;
    }

    public override void StartAltUse()
    {
        isFacingDown = !isFacingDown;
        isFlipping = true;
    }

    public override void UpdatePos(Vector3 deltaPos)
    {
        Vector3 pos = transform.position;
        pos.x += deltaPos.x * ScreenToWorldDelta;
        pos.z += deltaPos.y * ScreenToWorldDelta;
        transform.position = pos;
    }
}
