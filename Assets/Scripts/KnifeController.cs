using UnityEngine;

public class KnifeController : InteractableController
{
    public float throwForce = 10f;
    public Rigidbody victimRigidbody;
    const float ScreenToWorldDelta = 0.0025f;
    bool isThrown;
    bool isStabbed;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Update()
    {
        if (isStabbed)
        {
            return;
        }
        base.Update();

        if (transform.position.x > -0.25 && transform.position.x < 0.4 && transform.position.z > 0.22)
        {
            isStabbed = true;
            //myRigidbody.Sleep();
            //myRigidbody.useGravity = true;
            Destroy(gameObject.GetComponent<Rigidbody>());
            //myRigidbody.isKinematic = true;
            //myRigidbody.velocity = Vector3.zero;
            Vector3 stabPos = transform.position;
            stabPos.z += 0.2f;
            transform.position = stabPos;
            transform.parent = victimRigidbody.transform;
            victimRigidbody.isKinematic = false;
            victimRigidbody.AddForce(throwForce * Vector3.forward, ForceMode.Impulse);
            victimRigidbody.velocity = Vector3.zero;
        }

        if (isThrown)
        {
            return;
        }
    }

    public override void StartUse()
    {
        if (isStabbed)
        {
            return;
        }
        myRigidbody.useGravity = false;
        myRigidbody.isKinematic = true;
        Vector3 pos = transform.position;
        pos.y = 1.1f;
        transform.position = pos;
        transform.eulerAngles = new Vector3(270, 270, 0);
    }

    public override void FinishUse()
    {
        if (isStabbed)
        {
            return;
        }
        myRigidbody.useGravity = true;
        myRigidbody.isKinematic = false;
    }

    public override void StartAltUse()
    {
        if (isStabbed)
        {
            return;
        }
        base.StartAltUse();
        isThrown = true;
        FinishUse();
        myRigidbody.AddForce(throwForce * Vector3.forward, ForceMode.Impulse);
    }

    public override void ResetPosition()
    {
        if (isStabbed)
        {
            return;
        }
        base.ResetPosition();
        isThrown = false;
    }

    public override void UpdatePos(Vector3 deltaPos)
    {
        if (isStabbed || isThrown || !myRigidbody.isKinematic)
        {
            return;
        }
        Vector3 pos = transform.position;
        pos.x += deltaPos.x * ScreenToWorldDelta;
        pos.z += deltaPos.y * ScreenToWorldDelta;
        transform.position = pos;
    }
}
