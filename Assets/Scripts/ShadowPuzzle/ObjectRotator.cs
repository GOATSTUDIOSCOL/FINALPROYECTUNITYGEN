using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public bool isOnPuzzle = false;
    public bool puzzleSolved = false;
    private float minXAngle = -10f;
    private float maxXAngle = 10f;
    private float minYAngle = 190f;
    private float maxYAngle = 210f;
    public Door door;
    public LightsOn lighting;

    void Update()
    {
        if (isOnPuzzle && !puzzleSolved)
        {
            if (Input.GetMouseButton(0))
            {
                float mouseX = Input.GetAxis("Mouse X");
                float mouseY = Input.GetAxis("Mouse Y");
                float rotationX = mouseY * rotationSpeed * Time.deltaTime;
                float rotationY = -mouseX * rotationSpeed * Time.deltaTime;

                transform.Rotate(Vector3.right, rotationX, Space.World);
                transform.Rotate(Vector3.up, rotationY, Space.World);

            }
            float currentXAngle = transform.rotation.eulerAngles.x;
            float currentYAngle = transform.rotation.eulerAngles.y;

            bool isXInAngleRange = currentXAngle >= minXAngle && currentXAngle <= maxXAngle;
            bool isYInAngleRange = currentYAngle >= minYAngle && currentYAngle <= maxYAngle;

            if (isXInAngleRange && isYInAngleRange)
            {
                Debug.Log("puzzle resuelto");
                puzzleSolved = true;
                isOnPuzzle = false;
                door.OpenDoorRpc();
                lighting.ExitPuzzle();
            }
        }
    }
}
