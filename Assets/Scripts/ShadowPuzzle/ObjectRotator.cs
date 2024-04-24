using UnityEngine;
using Unity.Netcode;

public class ObjectRotator : NetworkBehaviour
{
    private Vector3 targetRotation = new Vector3(0, 205, 0);
    [SerializeField]
    private float rotationSpeed = 30f;
    public bool isOnPuzzle = false;
    public bool isDragging = false;

    private Vector2 MaxRotX = new Vector2(-60, 70);
    private Vector2 MaxRotY = new Vector2(100, 290);
    private Vector2 MaxRotZ = new Vector2(0, 360);

    private Vector3[] positions;

    private int howManyTries;

    private void Awake()
    {
        howManyTries = Random.Range(2, 6);
        positions = new Vector3[howManyTries + 1];
        positions[0] = targetRotation;
        for (int i = 1; i < positions.Length; i++)
            positions[i] = new Vector3(Random.Range(MaxRotX.x, MaxRotX.y), Random.Range(MaxRotY.x, MaxRotY.y), Random.Range(MaxRotZ.x, MaxRotZ.y));

        rotationServerRPC(Quaternion.Euler(positions[Random.Range(1, positions.Length)]));
    }


    public bool updateRotation()
    {
        int index = Random.Range(0, positions.Length);

        rotationServerRPC(Quaternion.Euler(positions[index]));
        return index == 0;
    }

    [ServerRpc(RequireOwnership = false)]
    private void rotationServerRPC(Quaternion rotateDir)
    {
        transform.rotation = rotateDir;
    }


}
