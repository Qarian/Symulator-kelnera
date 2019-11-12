using UnityEngine;

public class PlayerMovementWallRun : MonoBehaviour
{
	[SerializeField] Transform groundChecker;
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

	[Space]
	[SerializeField] float speed = 1f;
	[SerializeField] float maxSpeed = 5f;
	[SerializeField] float jumpForce = 10f;
    [SerializeField] float jumpOfWallForce = 4f;
    [SerializeField] float wallJumpTime = 0.5f;

    Rigidbody rb;
    Transform playerTransform;

    Vector3 moveAxis = Vector3.zero;
    Vector3 position;
    Vector3 right;

    bool onGround = false;
	bool canJump = true;

    enum Wall {No, Left, Right}
    Wall status = Wall.No;
    Vector3 wallDir;
    Vector3 wallCheckDir;
    float leftWallTime = 0f;

    const float wallRayLength = 0.8f;
    RaycastHit[] raycastHits = new RaycastHit[2];

	void Start()
	{
		rb = GetComponent<Rigidbody>();
        playerTransform = transform;
    }

	void Update()
	{
        position = playerTransform.position;
        right = playerTransform.right;
        // Get movement input
        moveAxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

		CheckGround();
        WallRunning();
        Jumping();
    }

    private void WallRunning()
    {
        if (Input.GetKeyUp(jumpKey) && status != Wall.No)
        {
            leftWallTime = Time.time;
            StopWallRunning();
            return;
        }
        
        if (status == Wall.No)
        {
            if (!onGround && (Time.time - leftWallTime) >= wallJumpTime)
            CheckWall();
        }
        else
        {
            int checkWallCount = Physics.RaycastNonAlloc(position, wallCheckDir, raycastHits, wallRayLength);
            if (checkWallCount == 0)
                StopWallRunning();
        }
    }

    private void StopWallRunning()
    {
        rb.useGravity = true;
        status = Wall.No;
        canJump = true;
    }

    private void CheckWall()
    {
        float leftDistance = wallRayLength;
        int rayCount = Physics.RaycastNonAlloc(position, -1 * right, raycastHits, wallRayLength);
        if (rayCount > 0)
        {
            if (Vector3.Dot(raycastHits[0].normal, Vector3.up) < 0.001f)
                SetWallRunning(raycastHits[0], Wall.Left);
        }

        rayCount = Physics.RaycastNonAlloc(position, right, raycastHits, wallRayLength);
        if (rayCount > 0)
        {
            if (Vector3.Dot(raycastHits[0].normal, Vector3.up) < 0.001f)
            {
                float rightDistance = Vector3.Distance(raycastHits[0].point, position);
                if (rightDistance < leftDistance)
                    SetWallRunning(raycastHits[0], Wall.Right);
            }
        }
    }

    private void SetWallRunning(RaycastHit hit, Wall side)
    {
        status = side;
        wallCheckDir = -raycastHits[0].normal;
        if (side == Wall.Left)
            wallDir = Vector3.Cross(raycastHits[0].normal, Vector3.up);
        else
            wallDir = -1 * Vector3.Cross(raycastHits[0].normal, Vector3.up);
        rb.useGravity = false;
        canJump = false;
    }

    private void Jumping()
    {
        if (canJump && Input.GetKeyDown(jumpKey))
        {
            Jump(false);
            if ((Time.time - leftWallTime) < wallJumpTime)
                Jump(true);
        }
    }

    private void Jump(bool fromWall)
    {
        if (!onGround)
            canJump = false;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        if (fromWall)
            rb.AddForce(wallCheckDir * jumpOfWallForce * -1f, ForceMode.VelocityChange);
    }

	private void CheckGround()
	{
		int hitCounts = Physics.RaycastNonAlloc(groundChecker.position, Vector3.down, raycastHits, 0.2f);
		if (hitCounts > 0)
		{
			onGround = true;
			canJump = true;
		}
		else
			onGround = false;
	}

	private void FixedUpdate()
	{
        if (status == Wall.No)
        {
            Vector3 accelaration = Vector3.zero;
            accelaration += moveAxis.z * playerTransform.forward;
            accelaration += moveAxis.x * playerTransform.right;
            accelaration = accelaration.normalized * speed * Time.fixedDeltaTime;
            rb.velocity = Vector3.ClampMagnitude(rb.velocity + accelaration, maxSpeed);
        }
        else
            rb.velocity = Vector3.ClampMagnitude(rb.velocity + (wallDir * speed * Time.fixedDeltaTime), maxSpeed);
    }
}
