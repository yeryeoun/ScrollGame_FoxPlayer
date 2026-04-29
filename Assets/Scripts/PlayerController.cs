using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;
    public float jumpDuration = 0.5f;

    [Header("Animation Controller")]
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController jumpController;
    public RuntimeAnimatorController runController;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    private bool isJumping = false;
    private float jumpTimer = 0f;
    private Vector3 startPosition;
    private bool isMovingRight = false;

    void Start()
    {
        // 컴포넌트 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // 초기 애니메이션 설정
        if (idleController != null)
        {
            animator.runtimeAnimatorController = idleController;
        }
    }

    void Update()
    {
        // 1. 이동 로직
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x -= 1f;
            spriteRenderer.flipX = true; // 왼쪽을 볼 때 이미지 반전 (필요 시)
        }
        
        if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x += 1f;
            isMovingRight = true;
            spriteRenderer.flipX = false; // 오른쪽을 볼 때 정방향
        }

        moveDirection = moveDirection.normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 2. 점프 입력 확인
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        // 3. 점프 상태 업데이트
        if (isJumping)
        {
            UpdateJump();
        }

        // 이동 방향 값 초기화 (프레임 끝에서)
        isMovingRight = false;
    }

    void StartJump()
    {
        isJumping = true;
        jumpTimer = 0f;
        startPosition = transform.position;

        // 점프 애니메이션으로 교체
        if (jumpController != null)
        {
            animator.runtimeAnimatorController = jumpController;
        }
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;
        float progress = jumpTimer / jumpDuration;

        if (progress >= 1f)
        {
            // 점프 종료: 위치 복구 및 상태 리셋
            transform.position = new Vector3(transform.position.x, startPosition.y, transform.position.z);
            isJumping = false;

            // 다시 아이들(또는 런) 애니메이션으로 교체
            if (idleController != null)
            {
                animator.runtimeAnimatorController = idleController;
            }
        }
        else
        {
            // Sine 곡선을 이용한 부드러운 점프 구현
            // 높이 계산: h = sin(π * progress) * jumpHeight
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;
            transform.position = new Vector3(transform.position.x, startPosition.y + height, transform.position.z);
        }
    }
}