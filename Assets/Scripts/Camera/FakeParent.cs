using Unity.Netcode;
using UnityEngine;

public class FakeParent : MonoBehaviour
{
    public Transform parentTransform;


    public bool attemptChildScale = false;

    Vector3 startParentPosition;
    Quaternion startParentRotationQ;
    Vector3 startParentScale;

    Vector3 startChildPosition;
    Quaternion startChildRotationQ;

    Matrix4x4 parentMatrix;


    private void OnEnable()
    {
        startParentPosition = parentTransform.position;
        startParentRotationQ = parentTransform.rotation;
        startParentScale = parentTransform.lossyScale;
        startChildPosition = transform.position;
        startChildRotationQ = transform.rotation;
        startChildPosition = DivideVectors(Quaternion.Inverse(parentTransform.rotation) * (startChildPosition - startParentPosition), startParentScale);
    }


    void Update()
    {

        parentMatrix = Matrix4x4.TRS(parentTransform.position, parentTransform.rotation, parentTransform.lossyScale);

        transform.position = parentMatrix.MultiplyPoint3x4(startChildPosition);
        transform.rotation = (parentTransform.rotation * Quaternion.Inverse(startParentRotationQ)) * startChildRotationQ;

    }

    Vector3 DivideVectors(Vector3 num, Vector3 den)
    {

        return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);

    }
}
