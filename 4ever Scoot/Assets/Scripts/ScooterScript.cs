using UnityEngine;
using UnityEngine.InputSystem;

public class ScooterScript : MonoBehaviour
{
    public float moveSpeedForward;
    public float moveSpeedBackward;
    public float moveSpeedMax;
    public float jumpForce;
    public float jumpTorque;
    public float stepSpeed;

    private float moveCmdForward;
    private int jumpCount = 0;

    // The gains are chosen experimentally
    public Vector3 rpyKp = new Vector3();
    public Vector3 rpyKi = new Vector3();
    public Vector3 rpyKd = new Vector3();

    public bool variablesBelowAreComputed = true;

    public Vector2 userStickInput;
    public Vector2Int wheelOnGround = new Vector2Int(1, 1);

    public Vector3 rpyCur = new Vector3();
    public Vector3 rpyTgt = new Vector3();
    public Vector3 rpyTorCur = new Vector3();

    public Vector3 prevError = new Vector3();
    public Vector3 P = new Vector3();
    public Vector3 I = new Vector3();
    public Vector3 D = new Vector3();

    public float moveUvecForward;

    private Rigidbody rb;
    private InputToBarScript bar;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        bar = GameObject.Find("BarParent").GetComponent<InputToBarScript>();
    }

    public Vector3 GetOrientation()
    {
        return rb.transform.eulerAngles;
    }
    void OnMove(InputValue value)
    {
        userStickInput = value.Get<Vector2>();
        moveCmdForward = userStickInput.y;
        bar.SendMessage("Rotate", userStickInput.x);
    }
    void OnBunnyHop()
    {
        BunnyHop();
    }
    void WheelTouchedGround(int wheelNum)
    {
        wheelOnGround[wheelNum] = 1;
    }
    void WheelLeftGround(int wheelNum)
    {
        wheelOnGround[wheelNum] = 0;
    }
    public bool IsWheelOnGround(int wheelNum)
    {
        return wheelOnGround[wheelNum] == 1;
    }
    public bool IsScootOnGround()
    {
        if(rb.velocity.magnitude < 0.001)
        {
            // Assume scooter is on ground
            wheelOnGround[0] = 1;
            wheelOnGround[1] = 1;
        }
        return wheelOnGround[0] == 1 || wheelOnGround[1] == 1;
    }

    void BunnyHop()
    {
        ++jumpCount;
        if (IsScootOnGround())
        {
            rb.AddForce(transform.up * jumpForce);
            rb.AddTorque(transform.right * -jumpTorque);
        }
    }
    void FixedUpdate()
    {
        rpyCur = GetOrientation();
        for (int i = 0; i < 3; ++i)
        {
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

        if (!IsScootOnGround()) { 
            rb.AddTorque(transform.right * rpyTorCur[0]);
        }
        rb.AddTorque(transform.up * rpyTorCur[1]);
        rb.AddTorque(transform.forward * rpyTorCur[2]);

        moveUvecForward = moveCmdForward; // moveCmdForward * Mathf.Clamp(Mathf.Sin(Time.time * stepSpeed), 0.0f, 1.0f);

        if (IsScootOnGround() && rb.velocity.magnitude < moveSpeedMax)
        {
            if(moveUvecForward > 0)
            {
                rb.AddForce(bar.GetForwardVector() * moveUvecForward * moveSpeedForward * 0.5f);
                rb.AddForce(transform.forward * moveUvecForward * moveSpeedForward * 0.5f);
            }
            else
            {
                rb.AddForce(bar.GetForwardVector() * moveUvecForward * moveSpeedBackward * 0.5f);
                rb.AddForce(transform.forward * moveUvecForward * moveSpeedBackward * 0.5f);
            }
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
