using UnityEngine;

public class InteractableController : MonoBehaviour
{
    public GameEventHandler gameEventHandler = null;
    Vector3[] positions;
    Vector3 reliablePosition;
    Quaternion rot;
    protected Rigidbody myRigidbody;

    public virtual void Awake()
    {
        rot = gameObject.transform.rotation;
        positions = new Vector3[30];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = gameObject.transform.position;
        }
        myRigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        TrackPositions();
        if (gameObject.transform.position.y < 0.7)
        {
            ResetPosition();
        }
    }

    public virtual void ResetPosition()
    {
        gameObject.transform.position = reliablePosition;
        gameObject.transform.rotation = rot;
        if (myRigidbody != null)
        {
            myRigidbody.velocity = Vector3.zero;
        }
        FinishUse();
        FinishAltUse();
    }

    public void TrackPositions()
    {
        bool isReliable = true;
        for (int i = positions.Length - 1; i > 0; i--)
        {
            positions[i] = positions[i - 1];
            if (Vector3.Distance(positions[i], gameObject.transform.position) > 0.1)
            {
                isReliable = false;
            }
        }
        if (positions.Length > 0)
        {
            positions[0] = gameObject.transform.position;
        }
        if (isReliable)
        {
            reliablePosition = gameObject.transform.position;
        }
    }

    public virtual void StartUse() {}

    public virtual void FinishUse() {}

    public virtual void StartAltUse()
    {
        if( gameEventHandler != null )
        {
            gameEventHandler.OnInteractionAltUse(this);
        }
    }

    public virtual void FinishAltUse() {}

    public virtual void UpdatePos(Vector3 deltaPos) {}
}
