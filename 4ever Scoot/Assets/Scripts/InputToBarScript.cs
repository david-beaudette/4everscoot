using UnityEngine;


public class InputToBarScript : MonoBehaviour
{
    public float turnKp;
    public float turnKi;
    public float turnKd;

    public float turnCur;
    public float turnTgt;
    public float turnTorCur;

    public float turnCmd;
    public float prevError;
    public float P;
    public float I;
    public float D;

    private ScooterScript scooter;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scooter = GameObject.Find("Scooter").GetComponent<ScooterScript>();
    }
    void Rotate(float value)
    {
        turnCmd = value;
        turnTgt = turnCmd * 45.0f;
    }
    public Vector3 GetForwardVector()
    {
        return transform.forward;
    }
    void FixedUpdate()
    {
        turnCur = transform.localEulerAngles.y;
        if (turnCur > 180.0f)
        {
            turnCur -= 360.0f;
        }
        if (turnCur < -180.0f)
        {
            turnCur += 360.0f;
        }

        turnTorCur = GetOutput(turnTgt - turnCur, Time.deltaTime);
        rb.AddRelativeTorque(Vector3.up * turnTorCur);
    }
    public float GetOutput(float currentError, float dt)
    {
        P = currentError;
        I += P * dt;
        D = (P - prevError) / dt;
        prevError = currentError;

        return P * turnKp + I * turnKi + D * turnKd;
    }
}


