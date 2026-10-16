using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private Vector3 minDirection = new Vector3(-0.1f, 0, -0.2f);
    [SerializeField] private Vector3 maxDirection = new Vector3(0.1f, 0, 0.2f);

    [SerializeField] private float minTorque = 5f;
    [SerializeField] private float maxTorque = 15f;
    
    [SerializeField] private Vector3 offMapPosition = new Vector3(0f, -10f, 0f);

    private void Awake()
    {rb = GetComponent<Rigidbody>();
    }

    public void Roll(Vector3 spawnPosition)
    {
        gameObject.SetActive(true);

        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = spawnPosition;
        transform.rotation = Random.rotation;

        Vector3 direction = new Vector3(
            Random.Range(minDirection.x, maxDirection.x),
            0,
            Random.Range(minDirection.z, maxDirection.z)
        );
        
        Vector3 torque = new Vector3(
            Random.Range(minTorque, maxTorque), 0, 0);

        rb.AddForce(direction, ForceMode.Impulse);
        rb.AddTorque(torque, ForceMode.Impulse);
    }

    public void HideOffMap()
    {
        transform.position = offMapPosition;
        gameObject.SetActive(false);
    }

    public bool IsStopped(float sqrThreshold)
    {
        return rb.IsSleeping() ||
               (rb.linearVelocity.sqrMagnitude <= sqrThreshold && rb.angularVelocity.sqrMagnitude <= sqrThreshold);
    }
}