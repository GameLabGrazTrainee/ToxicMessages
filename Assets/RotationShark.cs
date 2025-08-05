using UnityEngine;

public class SharkSwim : MonoBehaviour
{
    [Header("Extreme Oscillation Settings")]
    public float pitchAmplitude = 50f;  // X-axis (up/down tilt)
    public float pitchSpeed = 7f;

    public float yawAmplitude = 90f;    // Y-axis (left/right turn)
    public float yawSpeed = 6f;

    public float rollAmplitude = 45f;   // Z-axis (side banking)
    public float rollSpeed = 8f;

    private float baseYaw;

    void Start()
    {
        baseYaw = transform.eulerAngles.y;

        // Optional randomness for variation
        pitchSpeed += Random.Range(-0.3f, 0.3f);
        yawSpeed += Random.Range(-0.3f, 0.3f);
        rollSpeed += Random.Range(-0.3f, 0.3f);
    }

    void Update()
    {
        float pitch = Mathf.Sin(Time.time * pitchSpeed) * pitchAmplitude;
        float yaw = baseYaw + Mathf.Sin(Time.time * yawSpeed) * yawAmplitude;
        float roll = Mathf.Sin(Time.time * rollSpeed) * rollAmplitude;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }
}
