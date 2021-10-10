using UnityEngine;


public class InputToDeckScript : MonoBehaviour
{
    public float moveVal;
    public float moveSpeed;
    public float jumpForce;
    public float moveForceCur;
    public float stepSpeed;

    private int jumpCount = 0;

    Rigidbody rb;

    // The gains are chosen experimentally
    public Vector3 rpyKp = new Vector3();
    public Vector3 rpyKi = new Vector3();
    public Vector3 rpyKd = new Vector3();

    public Vector3 rpyCur = new Vector3();
    public Vector3 rpyTgt = new Vector3();
    public Vector3 rpyTorCur = new Vector3();

    public Vector3 prevError = new Vector3();
    public Vector3 P = new Vector3();
    public Vector3 I = new Vector3();
    public Vector3 D = new Vector3();

    private GameMasterScript gm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("GameMaster").GetComponent<GameMasterScript>();
    }
    void Move(float value)
    {
        moveVal = value;
    }
    void BunnyHop()
    {
        ++jumpCount;
        if (gm.IsScootOnGround())
        {
            rb.AddRelativeForce(0.0f, jumpForce * Time.deltaTime, 0.0f);
        }
    }
    void FixedUpdate()
    {
        rpyCur = transform.localEulerAngles;
        for(int i = 0; i < 3; ++i) {
            if (rpyCur[i] > 180.0f)
            {
               rpyCur[i] -= 360.0f;
            }
            if (rpyCur[i] < -180.0f)
            {
               rpyCur[i] += 360.0f;
            } 
        }
        rpyTorCur = GetOutput(rpyTgt - rpyCur, Time.deltaTime);
        rb.AddRelativeTorque(rpyTorCur);

        moveForceCur = moveVal * Mathf.Clamp(Mathf.Sin(Time.time * stepSpeed), 0.0f, 1.0f);
        if (gm.IsScootOnGround())
        {
            rb.AddRelativeForce(0.0f, 0.0f, -moveForceCur * moveSpeed * Time.deltaTime);
        }
    }
    public Vector3 GetOutput(Vector3 currentError, float dt)
    {
        P = currentError;
        I += P * dt;
        D = (P - prevError) / dt;
        prevError = currentError;

        return Vector3.Scale(P, rpyKp) + Vector3.Scale(I, rpyKi) + Vector3.Scale(D, rpyKd);
    }
}


