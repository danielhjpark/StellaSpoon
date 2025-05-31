using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using static SoundManager;



#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 5.0f; // 캐릭터 스피드

        private CharacterController _characterController;

        [Tooltip("How fast the character turns to face movement direction")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("Acceleration and deceleration")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        //public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("The height the player can jump")]
        public float JumpHeight = 1.2f; // 캐릭터 점프력

        [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
        public float Gravity = -15.0f; // 중력

        [Space(10)]
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
        public float JumpTimeout = 0.50f;

        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
        public bool Grounded = true; // 땅에 있는가 ?

        [Tooltip("Useful for rough ground")]
        public float GroundedOffset = -0.14f;

        [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
        public float GroundedRadius = 0.28f;

        [Tooltip("What layers the character uses as ground")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("How far in degrees can you move the camera up")]
        public float TopClamp = 70.0f;

        [Tooltip("How far in degrees can you move the camera down")]
        public float BottomClamp = -30.0f;

        [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("For locking the camera position on all axis")]
        public bool LockCameraPosition = false;

        // cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // player 플레이어
        private float _speed;
        private float _animationBlend;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // timeout deltatime
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // animation IDs
        private int _animIDSpeed;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;
        private int _animIDMotionSpeed;
        private int _animDDodge;

        // Dodge
        public bool isDodge = false;
        private bool dodgeCooldownActive;

        // 캐릭터 기본 스테이터스
        [Header("Player Status")]
        public float MaxHP = 100f;
        public float curHP;
        public float Def = 20;

        // Hit, Die
        public bool isHit = false; // 피격 상태를 나타내는 플래그
        private bool isInvincible = false; // 무적 상태를 나타내는 플래그
        public bool isDie = false; // 죽음 상태를 나타내는 플래그

        // Respawn
        private PlayerRespawn playerRespawn;

        // HPBar
        public Slider _hpBar;

        // DamageText
        [SerializeField]
        private GameObject damageTextPrefab; // 데미지 텍스트 프리팹 (Inspector에서 할당)
        public Transform uiCanvas; // UI 캔버스 (Inspector에서 할당)

        private bool isSliding;
        private float slopeLimit = 60f;
        private float slideSpeed = 10f;

        [Header("Dodge Distance")]
        [SerializeField]
        float dodgeDistance = 7f; // 닷지 이동 거리
        [SerializeField]
        float minDodgeDistance = 7f; // 최소 이동 거리

        private PlayerManager playerManager;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        public bool isAiming = false;
        public bool isReload = false;

        private void Awake()
        {
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            curHP = MaxHP;
            SetMaxHealth(curHP);
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
            _characterController = GetComponent<CharacterController>();
            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

            AssignAnimationIDs();

            // reset our timeouts on start
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;

            playerRespawn = GetComponent<PlayerRespawn>();
            playerManager = GetComponent<PlayerManager>();
        }

        private void Update()
        {
            CheckSlope();
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                playerRespawn = FindObjectOfType<PlayerRespawn>();
                _characterController.enabled = false;
                transform.position = playerRespawn.ReSpawnPoint.position;
                _characterController.enabled = true;
                Debug.Log("3535435535335");
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) // 5 체력 치트
            {
                SetMaxHealth(MaxHP);
            }

            if (isSliding)
            {
                Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, GetGroundNormal()).normalized;
                _controller.Move(slideDirection * slideSpeed * Time.deltaTime);
            }

            if(playerManager.isRestaurant)
            {
                _hpBar.gameObject.SetActive(false);
            }
            else
            {
                _hpBar.gameObject.SetActive(true);
            }

            if (!DeviceManager.isDeactived || TreasureChest.openingChest || !WeaponChanger.isDeactived || StoreNPCManager.openingStoreUI||Inventory.inventoryActivated || InteractUIManger.isUseInteractObject)
            {
                if (_hasAnimator)
                {
                    _animator.SetFloat(_animIDSpeed, 0f);
                    _animator.SetFloat(_animIDMotionSpeed, 0f);
                }
                return;
            }
            if (isDie) return;

            _hasAnimator = TryGetComponent(out _animator);

            Move();

            if (InventoryManager.instance.totalWeight >= 500) return;
            JumpAndGravity();
            GroundedCheck();
            Dodge();
        }

        private void LateUpdate()
        {
            if (!DeviceManager.isDeactived || TreasureChest.openingChest || StoreNPCManager.openingStoreUI ||Inventory.inventoryActivated || InteractUIManger.isUseInteractObject) return; // 비활성화 상태면 카메라 회전 막기 , 보물상자 오픈시 카메라 회전 막기
            CameraRotation();
        }
        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SetMaxHealth(MaxHP); // currentHealth는 따로 저장해둬야 함
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
            _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
            _animDDodge = Animator.StringToHash("Dodge");
        }
        private void Dodge()
        {
            // 가만히 있거나, 이미 닷지 중일 때는 닷지가 실행되지 않도록 함
            if (_input.move == Vector2.zero || isDodge || dodgeCooldownActive || isAiming || isHit || playerManager.isRestaurant) return;

            if (Grounded && _input.dodge)
            {
                if(isReload)
                {
                    CancelReload();
                }

                StartCoroutine(DodgeCoroutine());
            }
        }
        private IEnumerator DodgeCoroutine()
        {
            isDodge = true;
            dodgeCooldownActive = true;
            _input.dodge = false;
            LockCameraPosition = true;
            _animator.SetTrigger(_animDDodge);

            Vector3 dodgeDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
            dodgeDirection = Quaternion.Euler(0.0f, _mainCamera.transform.eulerAngles.y, 0.0f) * dodgeDirection;

            float dodgeDuration = 0.5f;
            float elapsedTime = 0f;
            float verticalVelocity = 0f;

            SoundManager.instance.PlayPlayerSound(SoundManager.Player.roll);

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + dodgeDirection * dodgeDistance;

            // 경사면 보정 (Raycast 거리 증가 및 중심보다 위에서 감지)
            if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit slopeHit, 2f, GroundLayers))
            {
                Vector3 slopeNormal = slopeHit.normal;
                dodgeDirection = Vector3.ProjectOnPlane(dodgeDirection, slopeNormal).normalized;
            }

            // 충돌 체크 (최소 이동 거리 보장)
            if (Physics.Raycast(transform.position, dodgeDirection, out RaycastHit hit, dodgeDistance, GroundLayers))
            {
                dodgeDistance = Mathf.Max(hit.distance, minDodgeDistance);
                targetPosition = startPosition + dodgeDirection * dodgeDistance;
            }

            // CharacterController 슬로프 보정
            _characterController.slopeLimit = 60f;

            while (elapsedTime < dodgeDuration)
            {
                float step = (dodgeDistance / dodgeDuration) * Time.deltaTime;
                Vector3 moveStep = dodgeDirection * step;

                // 중력 적용
                if (!Physics.Raycast(transform.position, Vector3.down, 1.1f, GroundLayers)) // 공중에 있는 경우
                {
                    verticalVelocity += Gravity * Time.deltaTime; // 중력 가속도 증가
                }
                else
                {
                    verticalVelocity = 0f; // 지면에 있으면 중력 리셋
                }

                moveStep.y += verticalVelocity * Time.deltaTime; // y 방향 중력 추가
                _characterController.Move(moveStep);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            LockCameraPosition = false;
            _animator.SetFloat(_animIDSpeed, 0f);
            _animator.SetFloat(_animIDMotionSpeed, 0f);
            isDodge = false;

            yield return new WaitForSeconds(3f);
            dodgeCooldownActive = false;
        }


        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
                transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
                QueryTriggerInteraction.Ignore);

            // update animator if using character
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        private void CameraRotation()
        {
            // if there is an input and camera position is not fixed
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                //Don't multiply mouse input by Time.deltaTime;
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // clamp our rotations so our values are limited 360 degrees
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine will follow this target
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            if (_mainCamera == null)
            {
                GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
                if (camObj != null)
                    _mainCamera = camObj;
                else
                    return;
            }

            if (isDodge || isHit) return;

            float targetSpeed = MoveSpeed;

            if (isAiming)
            {
                targetSpeed = MoveSpeed * 0.5f;
            }

            if (InventoryManager.instance.totalWeight >= 500)
            {
                targetSpeed *= 0.2f;
                Slot.isFull = true; // Set isFull to true
            }
            else
            {
                if (InventoryManager.instance.totalWeight >= 400 && InventoryManager.instance.totalWeight < 500)
                {
                    targetSpeed *= 0.6f;
                }
                Slot.isFull = false; // Set isFull to false
            }

            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
            if (_animationBlend < 0.01f) _animationBlend = 0f;

            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                if (!isAiming)
                {
                    transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
                }
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

            if (_hasAnimator)
            {
                _animator.SetFloat(_animIDSpeed, _animationBlend);
                _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
            }
        }

        private void JumpAndGravity()
        {

            if (isDodge) return; // 닷지 중일 때는 점프 불가

            if (Grounded)
            {
                _fallTimeoutDelta = FallTimeout;

                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    if (playerManager.isRestaurant) return;

                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    //AudioSource.PlayClipAtPoint(Jump_SFX, transform.position + Vector3.up * 0.1f, FootstepAudioVolume);
                    SoundManager.instance.PlayPlayerSound(SoundManager.Player.jump);

                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                _jumpTimeoutDelta = JumpTimeout;

                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                _input.jump = false;
            }

            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (playerManager.isRestaurant)
                {
                    // 식당에서는 식당 발소리만 재생
                    SoundManager.instance.PlayPlayerSound(SoundManager.Player.restfoot);
                }
                else
                {
                    // 일반 행성 발소리
                    SoundManager.instance.PlayPlayerSound(SoundManager.Player.foot);

                    // 70% 확률로 유리 소리 재생 (SoundManager에 추가했다면 여기도 해당 enum 사용)
                    if (Random.value <= 0.7f)
                    {
                        SoundManager.instance.PlayPlayerSFX(EPlayerSfx.GlassFootstep);
                    }
                }
            }
        }


        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.1f)
            {
                //AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
                SoundManager.instance.PlayPlayerSound(SoundManager.Player.land);
            }
        }

        public void TakeDamage(float _damage, Vector3 attackSourcePosition)
        {
            // 플레이어가 죽은 상태라면 데미지 로직 실행 안 함
            if (isInvincible || isDie) return;

            if(isReload)
            {
                CancelReload();
            }

            float monsterDam = _damage - (Def / 2f);

            if (monsterDam <= 0)
            {
                monsterDam = 1;
            }

            curHP -= monsterDam;
            curHP = Mathf.Max(curHP, 0); // HP가 음수가 되지 않도록 설정

            if (curHP <= 0)
            {
                Die();
            }

            isHit = true; // 피격 상태 활성화
            isInvincible = true; // 무적 상태 활성화
            _animator.SetTrigger("Hit");

            SoundManager.instance.PlayPlayerSound(SoundManager.Player.hit);

            Debug.Log("Player HP: " + curHP);
            _hpBar.value = curHP;

            //  데미지 텍스트 표시
            ShowDamageText(monsterDam);

            // 공격 방향 계산
            Vector3 knockbackDirection = (transform.position - attackSourcePosition).normalized; // 공격을 받은 방향의 반대 방향
            float knockbackDistance = 1f; // 넉백 거리
            float knockbackDuration = 0.2f; // 넉백 시간
            StartCoroutine(KnockbackCoroutine(knockbackDirection, knockbackDistance, knockbackDuration));

            // 일정 시간 후 무적 상태 해제
            StartCoroutine(InvincibilityCooldown(1.7f)); // 1.7초 동안 무적
        }

        private IEnumerator KnockbackCoroutine(Vector3 direction, float distance, float duration)
        {
            float elapsedTime = 0f;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = startPosition + direction.normalized * distance;

            while (elapsedTime < duration)
            {
                float step = (distance / duration) * Time.deltaTime;
                Vector3 moveStep = direction.normalized * step;
                _characterController.Move(moveStep);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 피격 애니메이션 종료 후 이동 가능하도록 플래그 해제
            yield return new WaitForSeconds(1.2f); // 애니메이션 재생 시간
            isHit = false;
        }

        private IEnumerator InvincibilityCooldown(float duration)
        {
            yield return new WaitForSeconds(duration);
            isInvincible = false; // 무적 상태 해제
        }

        private void Die()
        {
            if (isDie) return;

            isDie = true;
            Debug.Log("Die() 호출됨");

            // 🔹 모든 애니메이션 트리거 초기화
            _animator.ResetTrigger("Hit");
            _animator.ResetTrigger("Reload");
            _animator.ResetTrigger("Shot");
            _animator.ResetTrigger("Dodge");

            // 🔹 모든 레이어의 애니메이션 가중치 초기화 (총기 애니메이션 비활성화)
            for (int i = 1; i < _animator.layerCount; i++)
            {
                _animator.SetLayerWeight(i, 0);
            }

            // 🔹 Die 애니메이션 실행 (Base Layer에서 강제 실행)
            _animator.Play("Die", 0);

            Debug.Log("Die 애니메이션 강제 실행");

            // 인벤토리 정리 및 리스폰 처리
            InventoryManager.instance.ClearAllSlots();
            StartCoroutine(RespawnWithFade());
        }
        private IEnumerator RespawnWithFade()
        {
            yield return new WaitForSeconds(1f);

            // 페이드 아웃 (0 -> 1)
            if (FadeEffect.Instance != null)
                yield return StartCoroutine(FadeEffect.Instance.Fade(0, 1));

            // 리스폰 진행
            yield return StartCoroutine(Respawn());

            // 페이드 인 (1 -> 0)
            if (FadeEffect.Instance != null)
                yield return StartCoroutine(FadeEffect.Instance.Fade(1, 0));
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(1.5f); // 1.5초 대기

            // playerRespawn이 null이면 찾아서 할당
            if (playerRespawn == null)
            {
                playerRespawn = FindObjectOfType<PlayerRespawn>();
            }

            // playerRespawn이 여전히 null이면 리스폰 중단
            if (playerRespawn == null || playerRespawn.ReSpawnPoint == null)
            {
                Debug.LogError("리스폰 실패! playerRespawn 또는 ReSpawnPoint가 설정되지 않았습니다.");
                yield break;
            }

            curHP = MaxHP; // HP 초기화
            SetMaxHealth(MaxHP); // HPBar 초기화
            isDie = false; // 죽음 상태 해제
            isHit = false; // 피격 상태 해제
            isInvincible = false; // 무적 상태 해제
            _animator.SetTrigger("ReSpawn");
            _characterController.enabled = false;
            transform.position = playerRespawn.ReSpawnPoint.position; // 이제 Null 오류 발생 안 함!
            _characterController.enabled = true;

            Debug.Log("Player Respawned at: " + playerRespawn.ReSpawnPoint.position);
        }


        private void SetMaxHealth(float health)
        {
            _hpBar.maxValue = health;
            _hpBar.value = health;
        }
        void ShowDamageText(float damage)
        {
            if (damageTextPrefab != null && uiCanvas != null)
            {
                GameObject damageText = Instantiate(damageTextPrefab, uiCanvas);
                TextMeshProUGUI textMesh = damageText.GetComponent<TextMeshProUGUI>();

                if (textMesh != null)
                {
                    textMesh.text = damage.ToString();

                    //  플레이어 옆으로 랜덤 위치 설정
                    float randomOffsetX = Random.Range(-1.5f, 1.5f);
                    Vector3 worldPosition = transform.position + Vector3.right * randomOffsetX + Vector3.up * 1f;
                    Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

                    damageText.transform.position = screenPosition;

                    //  페이드 아웃 & 이동 코루틴 실행
                    StartCoroutine(FadeAndMoveDamageText(damageText, textMesh));
                }
            }
        }

        IEnumerator FadeAndMoveDamageText(GameObject textObj, TextMeshProUGUI textMesh)
        {
            float duration = 1f; // 1초 동안 페이드 아웃
            float elapsedTime = 0f;
            Color originalColor = textMesh.color; // 원래 색상 저장
            Vector3 startPos = textObj.transform.position;
            Vector3 targetPos = startPos + new Vector3(0, 50f, 0); // 50px 위로 이동

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                // 🔹 위로 이동
                textObj.transform.position = Vector3.Lerp(startPos, targetPos, t);

                // 🔹 알파값 조절 (점점 투명하게)
                textMesh.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);

                yield return null;
            }

            Destroy(textObj); // 끝나면 삭제
        }

        public void CancelReload()
        {
            _animator.ResetTrigger("Reload");
            _animator.SetLayerWeight(2, 0);
            _animator.CrossFade("Empty", 0.1f, 2);
            isReload = false;
        }
        void CheckSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _controller.height / 2 + 0.1f))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);

                // 경사가 너무 높으면 점프 불가
                if (angle > slopeLimit)
                {
                    isSliding = true;
                    _input.jump = false;
                }
                else
                {
                    isSliding = false;
                }
            }
        }
        Vector3 GetGroundNormal()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, _controller.height / 2 + 0.1f))
            {
                return hit.normal;
            }
            return Vector3.up;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Water"))
            {
                StartCoroutine(DieAfterDelay(0.5f));
            }
        }
        private IEnumerator DieAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            _hpBar.value = 0;
            Die();
        }
    }
}
