using UnityEngine;


public class InputToBarScript : MonoBehaviour
{
    public float moveVal;

    Rigidbody rb;

    // The gains are chosen experimentally
    public float turnKp;
    public float turnKi;
    public float turnKd;

    public float turnCur;
    public float turnTgt;
    public float turnTorCur;

    public float prevError;
    public float P;
    public float I;
    public float D;

    private GameMasterScript gm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }
    void Rotate(float value)
    {
        moveVal = value;
        turnTgt = moveVal * 45.0f;
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


