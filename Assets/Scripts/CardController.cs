using UnityEngine;

public class CardController : MonoBehaviour
{
    Rigidbody myRigidbody;
    bool isActive;
    const float ScreenToWorldDelta = 0.0025f;

    void Awake()
    {
        myRigidbody = GetComponent<Rigidbody>();
    }

    public void StartUse()
    {
        isActive = true;
        myRigidbody.useGravity = false;
        Vector3 pos = transform.position;
        pos.y = 1.1f;
        transform.position = pos;
    }

    public void FinishUse()
    {
        isActive = false;
        myRigidbody.useGravity = true;
    }

    public void MoveCard(Vector3 deltaPos)
    {
        Vector3 pos = transform.position;
        pos.x += deltaPos.x * ScreenToWorldDelta;
        pos.z += deltaPos.y * ScreenToWorldDelta;
        transform.position = pos;
    }
}
