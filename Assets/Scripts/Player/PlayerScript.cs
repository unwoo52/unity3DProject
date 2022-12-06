using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Reflection;
using Definitions;

namespace Player
{
    public class PlayerScript : MonoBehaviour, IBuff, IBattle
    {
        /***********************************************************************
        *                               SingleTone
        ***********************************************************************/
        #region Singletone
        private static PlayerScript playerInstance = null;

        void Awake()
        {
            ChangeState(STATE.START);
            instance = this;
            InitComponents();
            InitSettings();

            if (playerInstance == null)
            {
                //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
                playerInstance = this;

                //씬 전환이 되더라도 파괴되지 않게 한다.
                //gameObject만으로도 이 스크립트가 컴포넌트로서 붙어있는 Hierarchy상의 게임오브젝트라는 뜻이지만, 
                //나는 헷갈림 방지를 위해 this를 붙여주기도 한다.
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                //만약 씬 이동이 되었는데 그 씬에도 Hierarchy에 GameMgr이 존재할 수도 있다.
                //그럴 경우엔 이전 씬에서 사용하던 인스턴스를 계속 사용해주는 경우가 많은 것 같다.
                //그래서 이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
                Destroy(this.gameObject);
            }
        }
        //게임 매니저 인스턴스에 접근할 수 있는 프로퍼티. static이므로 다른 클래스에서 맘껏 호출할 수 있다.
        public static PlayerScript PlayerInstance
        {
            get
            {
                if (null == playerInstance)
                {
                    return null;
                }
                return playerInstance;
            }
        }
        #endregion
        /***********************************************************************
        *                               Interfaces
        ***********************************************************************/
        #region interface IBuff   
        [Space]
        public List<BaseBuff> BuffList = new();
        /// <summary>
        /// 버프스탯에 적용받는 스탯들(이동속도 등) 외의 체력과 같은 스탯에 효과를 적용
        /// </summary>
        /// <param name="s">ex) CurHP</param>
        /// <param name="f">ex) 5.0f</param>
        public void BuffValueApply(string s, float f)
        {
            switch (s)
            {
                case "CurHP":
                    myInfo.CurHP += f;
                    break;
            }
        }
        public void BuffListAdd(BaseBuff baseBuff)
        {
            BuffList.Add(baseBuff);
        }
        public void RemovBuff(BaseBuff baseBuff)
        {
            BuffList.Remove(baseBuff);
        }
        /// <summary>
        /// buffTypenameList에 있는 buffTypename들에 대해 각각의 IBuff.BuffEffectAplly()을 수행
        /// </summary>
        /// <param name="buffTypenameList"></param>
        public void ChooseBuff(List<string> buffTypenameList)//리스트[1]에 0.miningDelay와 1.movespeed를 받아왔다면 miningDelay와 movespeed에 대한 BuffEffectApply를 실행
        {
            foreach (string s in buffTypenameList)
            {
                switch (s)
                {
                    case "MineDelay_Mining":
                        myInfo.MineDelay_Mining_AfterBuff = BuffEffectAplly(s, myInfo.MineDelay_Mining_Origin);
                        break;
                    case "MineDelay_Picking":
                        myInfo.MineDelay_Picking_AfterBuff = BuffEffectAplly(s, myInfo.MineDelay_Picking_Origin);
                        break;
                    case "MoveSpeed":
                        myInfo.MoveSpeed_AfterBuff = BuffEffectAplly(s, myInfo.MoveSpeed_Origin);
                        break;
                }
            }
        }
        /// <summary>
        /// 버프 리스트를 모두 탐색하여 buffTypeName이 일치하는 버프의 값들을 구해 origin에 더하여 반환
        /// </summary>
        /// <param name="buffTypeName">변경해야 하는 버프 타입 이름 ex)MoveSpeed_AfterBuff</param>
        /// <param name="origin">해당 타입의 오리진 ex)MoveSpeed_Origin</param>
        /// <returns>해당 버프 타입의 버프 효과가 모두 더해진 값</returns>
        public float BuffEffectAplly(string buffTypeName, float origin)//플레이어가 가진 BuffList에서 s와 일치하는 모두를 탐색한 후 해당하는 value들을 origin에 더해서 return
        {
            if (BuffList.Count > 0)
            {
                float temp = 0;
                for (int i = 0; i < BuffList.Count; i++)//버프 갯수만큼 반복, 버프 리스트를 훑기
                {
                    for (int j = 0; j < BuffList[i].BuffTypenameList.Count; j++)//리스트의 i번째 버프의 buffTypenameList를 탐색
                    {
                        if (BuffList[i].BuffTypenameList[j].Equals(buffTypeName)) //리스트의 i번째 버프의 buffTypenameList의 j번째 문자열을 s와 비교
                            temp += origin * BuffList[i].BuffValueList[j]; //일치한다면 temp에 버프의 value를 더하기
                    }
                }
                return origin + temp; //origin에 temp들을 모두 더하여 리턴
            }
            else return origin;
        }


        #endregion
        #region IBattle
        public void OnDamage(float dmg)
        {
            myInfo.CurHP -= dmg;
            if (myInfo.CurHP > 0.0f)
            {
                Com.anim.SetTrigger("OnDamage");
            }
            else
            {
                ChangeState(STATE.DEATH);
            }
        }

        public bool IsLive
        {
            get
            {
                if (myInfo.CurHP <= 0.0f)
                {
                    return false;
                }
                return true;
            }
        }
        #endregion
        /***********************************************************************
        *                               Fields, Properties
        ***********************************************************************/
        #region .
        [Space, Header("테스트 리스트. 나중에 UI, 인벤토리, 장비창으로 대체")]
        [SerializeField] private InventoryObject inventory;
        public InventoryObject Inventory { get; }
        public GameObject StartCamera;
        /// <summary> 장비 장착 위치를 숫자로 받아 장비의 스탯 버프를 저장 </summary>
        [SerializeField] private GameObject[] GadgetBuffList = new GameObject[7];
        public Components Com => _components;
        public KeyOption Key => _keyOption;
        public MovementOption MoveOption => _movementOption;
        public CameraOption CamOption => _cameraOption;
        public AnimatorOption AnimOption => _animatorOption;
        public CharacterState State => _state;
        public Gadget Gget => _gadget;
        public PlayerInfoMask plMask => _playerInfoMask;
        public MouseInput MouseInput => _mouseInput;

        public static PlayerScript instance;
        [Space, Header(" # 컴포넌트들")]
        [SerializeField] private Components _components = new();

        [Space, Header(" # 키 설정")]
        [SerializeField] private KeyOption _keyOption = new();

        [Space, Header(" # 이동 옵션")]
        [SerializeField] private MovementOption _movementOption = new();

        [Space, Header(" # 카메라 옵션")]
        [SerializeField] private CameraOption _cameraOption = new();

        private AnimatorOption _animatorOption = new();

        [Space, Header(" # 플레이어 STATEs")]
        [SerializeField] private CharacterState _state = new();

        [Space, Header(" # 장비창")]
        [SerializeField] private Gadget _gadget = new();

        [Space, Header(" # 스탯창")]
        public PlayerStat myInfo;

        [Space, Header(" # 레이어 마스크")]
        [SerializeField] private PlayerInfoMask _playerInfoMask = new();

        [Space, Header(" # 마우스 스크립트")]
        [SerializeField] private MouseInput _mouseInput = new();

        [Space, Header(" # 장비 장착 위치")]
        [SerializeField] private List<GameObject> bodyPointList = new();


        private Vector3 _moveDir;
        private Vector3 _worldMove;
        private Vector2 _rotation;

        /// <summary> TP 카메라 ~ Rig 초기 거리 </summary>
        private float _tpCamZoomInitialDistance;

        /// <summary> TP 카메라 휠 입력 값 </summary>
        private float _tpCameraWheelInput = 0;

        /// <summary> </summary>
        private float _groundCheckRadius;
        float _distFromGround;

        /// <summary> </summary>
        private float _moveX;
        private float _moveZ;

        /// <summary> </summary>
        private float _currentWheel;

        /// <summary> </summary>
        private float _currentJumpCooldown;
        private float _deltaTime;

        /// <summary> Character stat </summary>



        public Arrow myArrow = null;

        #endregion
        /***********************************************************************
        *                               Init Methods
        ***********************************************************************/
        #region .
        private void InitComponents()
        {
            LogNotInitializedComponentError(Com.tpCamera, "TP Camera");
            LogNotInitializedComponentError(Com.fpCamera, "FP Camera");
            TryGetComponent(out Com.rBody);
            Com.anim = GetComponentInChildren<Animator>();

            Com.tpCamObject = Com.tpCamera.gameObject;
            Com.tpRig = Com.tpCamera.transform.parent;
            Com.fpCamObject = Com.fpCamera.gameObject;
            Com.fpRig = Com.fpCamera.transform.parent;
            Com.fpRoot = Com.fpRig.parent;
        }

        private void InitSettings()
        {
            // Rigidbody
            if (Com.rBody)
            {
                // 회전은 트랜스폼을 통해 직접 제어할 것이기 때문에 리지드바디 회전은 제한
                Com.rBody.constraints = RigidbodyConstraints.FreezeRotation;
            }

            // Camera deactivate    
            /*
            var allCams = FindObjectsOfType<Camera>();
            foreach (var cam in allCams)
            {
                cam.gameObject.SetActive(false);
            }*/
            // 설정한 카메라 하나만 활성화// 카메라 depth 설정
            State.isCurrentFp = (CamOption.initialCamera == CameraOption.CameraEnum.FpCamera);
            Com.fpCamera.depth = State.isCurrentFp ? 1 : 0;
            Com.tpCamera.depth = State.isCurrentFp ? 0 : 1;

            TryGetComponent(out CapsuleCollider cCol);
            _groundCheckRadius = cCol ? cCol.radius : 0.1f;

            _tpCamZoomInitialDistance = Vector3.Distance(Com.tpRig.position, Com.tpCamera.transform.position);
        }

        private void Start()
        {
            ChangeState(STATE.ZOOM);
            /*
            myInfo = new PlayerStat(
                hp: FileData.Inst.curData.curHP,
                dmg: 10f,
                moveSpeed_Origin: 10f,
                mineDelay_Picking_Origin: 1f,
                mineDelay_Mining_Origin: 1f,
                mineDelay_Logging_Origin: 1f,
                battleDelay_Melee_Origin: 1f);
            */
            //transform.position = FileData.Inst.curData.curPos;
        }


        #endregion
        /***********************************************************************
        *                               Checker Methods
        ***********************************************************************/
        #region .
        private void LogNotInitializedComponentError<T>(T component, string componentName) where T : Component
        {
            if (component == null)
                Debug.LogError($"{componentName} 컴포넌트를 인스펙터에 넣어주세요");
        }

        #endregion
        /***********************************************************************
        *                               Methods
        ***********************************************************************/
        #region Update
        private void Update()
        {
            _deltaTime = Time.deltaTime;
            StateProcess();
        }
        #endregion
        #region Camera Move Jump Rotate
        private void UpdateAnimationParams()
        {
            float x, z;

            if (State.isCurrentFp)
            {
                x = _moveDir.x;
                z = _moveDir.z;

                if (State.isRunning)
                {
                    x *= 2f;
                    z *= 2f;
                }
            }
            else
            {
                x = 0f;
                z = _moveDir.sqrMagnitude > 0f ? 1f : 0f;

                if (State.isRunning)
                {
                    z *= 2f;
                }
            }

            // 보간
            const float LerpSpeed = 0.05f;
            _moveX = Mathf.Lerp(_moveX, x, LerpSpeed);
            _moveZ = Mathf.Lerp(_moveZ, z, LerpSpeed);

            Com.anim.SetFloat(AnimOption.paramMoveX, _moveX);
            Com.anim.SetFloat(AnimOption.paramMoveZ, _moveZ);
            Com.anim.SetFloat(AnimOption.paramDistY, _distFromGround);
            Com.anim.SetBool(AnimOption.paramGrounded, State.isGrounded);

            if (_state.isJumping && Com.anim.GetBool(AnimOption.paramGrounded)) _state.isJumping = false;
        }
        private void Jump()
        {
            if (!State.isGrounded) return;
            if (_currentJumpCooldown > 0f) return; // 점프 쿨타임

            if (Input.GetKeyDown(Key.jump))
            {
                _state.isJumping = true;
                // 하강 중 점프 시 속도가 합산되지 않도록 속도 초기화
                Com.rBody.velocity = Vector3.zero;

                Com.rBody.AddForce(Vector3.up * MoveOption.jumpForce, ForceMode.VelocityChange);

                // 애니메이션 점프 트리거
                Com.anim.SetTrigger(AnimOption.paramJump);

                // 쿨타임 초기화
                _currentJumpCooldown = MoveOption.jumpCooldown;
            }
        }
        private void UpdateCurrentValues()
        {
            if (_currentJumpCooldown > 0f)
                _currentJumpCooldown -= _deltaTime;
        }

        private void Rotate()
        {
            if (State.isCurrentFp)
            {
                if (!State.isCursorActive)
                    RotateFP();
            }
            else
            {
                if (!State.isCursorActive) RotateTP();
                if (State.isMoving == true) RotateFPRoot();
            }
        }


        private void CheckDistanceFromGround()
        {
            Vector3 ro = transform.position + Vector3.up;
            Vector3 rd = Vector3.down;
            Ray ray = new Ray(ro, rd);

            const float rayDist = 500f;
            const float threshold = 0.01f;

            bool cast =
                Physics.SphereCast(ray, _groundCheckRadius, out var hit, rayDist, MoveOption.groundLayerMask);

            _distFromGround = cast ? (hit.distance - 1f + _groundCheckRadius) : float.MaxValue;
            State.isGrounded = _distFromGround <= _groundCheckRadius + threshold;
        }

        /// <summary> 3인칭 회전 </summary>
        private void RotateTP()
        {
            float deltaCoef = _deltaTime * 50f;

            // 상하 : TP Rig 회전
            float xRotPrev = Com.tpRig.localEulerAngles.x;
            float xRotNext = xRotPrev + _rotation.y
                * CamOption.rotationSpeed * deltaCoef;

            if (xRotNext > 180f)
                xRotNext -= 360f;

            // 좌우 : TP Rig 회전
            float yRotPrev = Com.tpRig.localEulerAngles.y;
            float yRotNext =
                yRotPrev + _rotation.x
                * CamOption.rotationSpeed * deltaCoef;

            // 상하 회전 가능 여부
            bool xRotatable =
                CamOption.lookUpDegree < xRotNext &&
                CamOption.lookDownDegree > xRotNext;

            Vector3 nextRot = new Vector3
            (
                xRotatable ? xRotNext : xRotPrev,
                yRotNext,
                0f
            );

            // TP Rig 회전 적용
            Com.tpRig.localEulerAngles = nextRot;
        }
        /// <summary> 1인칭 회전 </summary>
        private void RotateFP()
        {
            float deltaCoef = _deltaTime * 50f;

            // 상하 : FP Rig 회전
            float xRotPrev = Com.fpRig.localEulerAngles.x;
            float xRotNext = xRotPrev + _rotation.y
                * CamOption.rotationSpeed * deltaCoef;

            if (xRotNext > 180f)
                xRotNext -= 360f;

            // 좌우 : FP Root 회전
            float yRotPrev = Com.fpRoot.localEulerAngles.y;
            float yRotNext =
                yRotPrev + _rotation.x
                * CamOption.rotationSpeed * deltaCoef;

            // 상하 회전 가능 여부
            bool xRotatable =
                CamOption.lookUpDegree < xRotNext &&
                CamOption.lookDownDegree > xRotNext;

            // FP Rig 상하 회전 적용
            Com.fpRig.localEulerAngles = Vector3.right * (xRotatable ? xRotNext : xRotPrev);

            // FP Root 좌우 회전 적용
            Com.fpRoot.localEulerAngles = Vector3.up * yRotNext;
        }

        /// <summary> 3인칭일 경우 FP Root 회전 
        /// FP Root에 
        /// </summary>
        private void RotateFPRoot()
        {
            Vector3 dir = Com.tpRig.TransformDirection(_moveDir); // Direction을 world Vector로 변환해 dir에 저장
            float currentY = Com.fpRoot.localEulerAngles.y;
            float nextY = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;

            //currentY와 nextY 의 연산
            if (nextY - currentY > 180f) nextY -= 360f;
            else if (currentY - nextY > 180f) nextY += 360f;

            Com.fpRoot.eulerAngles = Vector3.up * Mathf.Lerp(currentY, nextY, 0.1f);
        }

        private void Move()
        {
            // 이동하지 않는 경우, 미끄럼 방지
            if (State.isMoving == false)
            {
                Com.rBody.velocity = new Vector3(0f, Com.rBody.velocity.y, 0f);
                return;
            }

            // 실제 이동 벡터 계산
            // 1인칭
            if (State.isCurrentFp)
            {
                _worldMove = Com.fpRoot.TransformDirection(_moveDir);
            }
            // 3인칭
            else
            {
                _worldMove = Com.tpRig.TransformDirection(_moveDir);
            }

            _worldMove *= (myInfo.MoveSpeed_AfterBuff) * (State.isRunning ? MoveOption.runningCoef : 1f);

            // Y축 속도는 유지하면서 XZ평면 이동
            if (!_state.isJumping && !_state.isGrounded) Com.rBody.velocity = new Vector3(_worldMove.x, -_movementOption.rigidGrabity, _worldMove.z);
            else Com.rBody.velocity = new Vector3(_worldMove.x, Com.rBody.velocity.y, _worldMove.z);

        }

        private void CameraViewToggle()
        {
            if (Input.GetKeyDown(Key.switchCamera))
            {
                State.isCurrentFp = !State.isCurrentFp;
                Com.fpCamera.depth = State.isCurrentFp ? 1 : 0;
                Com.tpCamera.depth = State.isCurrentFp ? 0 : 1;

                // TP -> FP
                if (State.isCurrentFp)
                    ApplyTpcamValueToFpcam();
                // FP -> TP
                else
                    ApplyFpcamValueToTpcam();
            }
        }

        public void ApplyTpcamValueToFpcam()
        {
            Vector3 tpEulerAngle = Com.tpRig.localEulerAngles;
            Com.fpRig.localEulerAngles = Vector3.right * tpEulerAngle.x;
            Com.fpRoot.localEulerAngles = Vector3.up * tpEulerAngle.y;
        }
        public void ApplyFpcamValueToTpcam()
        {
            Vector3 newRot = default;
            newRot.x = Com.fpRig.localEulerAngles.x;
            newRot.y = Com.fpRoot.localEulerAngles.y;
            Com.tpRig.localEulerAngles = newRot;
        }


        private void TpCameraZoom()
        {
            if (State.isCurrentFp) return;                // TP 카메라만 가능
            if (Mathf.Abs(_currentWheel) < 0.01f) return; // 휠 입력 있어야 가능

            Transform tpCamTr = Com.tpCamera.transform;
            Transform tpCamRig = Com.tpRig;

            float zoom = _deltaTime * CamOption.zoomSpeed;
            float currentCamToRigDist = Vector3.Distance(tpCamTr.position, tpCamRig.position);
            Vector3 move = Vector3.forward * zoom * _currentWheel * 10f;

            // Zoom In
            if (_currentWheel > 0.01f)
            {
                if (_tpCamZoomInitialDistance - currentCamToRigDist < CamOption.zoomInDistance)
                {
                    tpCamTr.Translate(move, Space.Self);
                }
            }
            // Zoom Out
            else if (_currentWheel < -0.01f)
            {

                if (currentCamToRigDist - _tpCamZoomInitialDistance < CamOption.zoomOutDistance)
                {
                    tpCamTr.Translate(move, Space.Self);
                }
            }
        }
        #endregion
        #region Cusor
        private void ShowCursorToggle()
        {
            if (Input.GetKeyDown(Key.showCursor))
                State.isCursorActive = !State.isCursorActive;

            ShowCursor(State.isCursorActive);
        }

        private void ShowCursor(bool value)
        {
            Cursor.visible = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
        }
        public void ActiveOnCursor()
        {
            State.isCursorActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        #endregion
        #region Gadget
        /// <summary>
        /// 오브젝트와 슬롯을 입력받아 착용을 실행
        /// string으로 받고 있지만 코드를 사용하는 것이 더 좋아보임
        /// </summary>
        /// <param name="gameObject">착용할 오브젝트</param>
        /// <param name="image">아이콘 이미지</param>
        /// <param name="str">착용할 슬롯 위치 이름</param>
        private void GadgetWearing(GameObject gameObject, Sprite image, string str)
        {
            if (gameObject.TryGetComponent(out ItemScript objScript))
            {
                myWeapon = (WeaponType)(objScript.ItemTypeCode); //플레이어 weapon enum 변경
                BuffManagerScript.instance.CreateBuff(
                    objScript.GadgetBuffTypeName,
                    objScript.GadgetBuffvalue,
                    image);//버프 추가
                GameObject PartsPostionObject = typeof(Gadget).GetField(str).GetValue(_gadget) as GameObject;
                Instantiate(
                    gameObject,
                    PartsPostionObject.transform.position,
                    PartsPostionObject.transform.rotation,
                    PartsPostionObject.transform);
            }
            else Debug.LogError("This Object itemscript does not exist!!");
        }
        #endregion

        /***********************************************************************
        *                               Input Methods
        ***********************************************************************/
        #region .
        private void SetValuesByKeyInput()
        {
            float h = 0f, v = 0f;

            if (Input.GetKey(Key.moveForward)) v += 1.0f;
            if (Input.GetKey(Key.moveBackward)) v -= 1.0f;
            if (Input.GetKey(Key.moveLeft)) h -= 1.0f;
            if (Input.GetKey(Key.moveRight)) h += 1.0f;

            Vector3 moveInput = new Vector3(h, 0f, v).normalized;
            _moveDir = Vector3.Lerp(_moveDir, moveInput, MoveOption.acceleration); // 가속, 감속
            _rotation = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

            State.isMoving = _moveDir.sqrMagnitude > 0.01f;
            State.isRunning = Input.GetKey(Key.run);

            _tpCameraWheelInput = Input.GetAxisRaw("Mouse ScrollWheel");
            _currentWheel = Mathf.Lerp(_currentWheel, _tpCameraWheelInput, CamOption.zoomAccel);

        }
        /// <summary>
        /// 캐릭터의 회전을 마우스 방향으로 바라보게 정렬한 후 FrontView Camerad에서 화면 중앙(에임)을 향해 발사하는 Ray를 리턴
        /// </summary>
        /// <returns>캐릭터에서 화면 중앙(에임)으로 뻗는 Ray</returns>
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="hit"></param>

        #endregion
        /***********************************************************************
        *                               STATE Machine
        ***********************************************************************/
        #region .
        public STATE myState = STATE.START;
        public enum STATE
        {
            START, CREAT, ZOOM, NORMAL, DEATH
        }

        public void ChangeState(STATE state)
        {
            if (myState == state) return;
            myState = state;

            switch (myState)
            {
                case STATE.START:
                    break;
                case STATE.CREAT:
                    break;
                case STATE.ZOOM:
                    StartCamera.SetActive(true);
                    break;
                case STATE.NORMAL:
                    CrossHair.CrosshairInstance.ChangeCrossHair(1);
                    //StopAllCoroutines();
                    break;
                case STATE.DEATH:
                    StopAllCoroutines();
                    Com.anim.SetTrigger("Death");
                    Com.anim.enabled = false;
                    break;
            }
        }

        public void StateProcess()
        {
            switch (myState)
            {
                case STATE.START:
                    break;
                case STATE.CREAT:
                    break;
                case STATE.ZOOM:
                    break;
                case STATE.NORMAL:

                    ShowCursorToggle();
                    CameraViewToggle();
                    SetValuesByKeyInput();
                    CheckDistanceFromGround();

                    Rotate();

                    UpdateAnimationParams();
                    TpCameraZoom();
                    UpdateCurrentValues();
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        Debug.Log("Save!");
                        inventory.Save();
                    }
                    if (Input.GetKeyDown(KeyCode.KeypadEnter))
                    {
                        Debug.Log("Load!");
                        inventory.Load();
                    }
                    myInfo.curDelay += _deltaTime;
                    Move();
                    Jump();
                    _mouseInput.MouseInPutProcess();
                    break;
                case STATE.DEATH:
                    break;
            }
        }
        #endregion
        /***********************************************************************
        *                               STATE Machine
        ***********************************************************************/
        #region .
        public List<Transform> myAttackpos = new List<Transform>();


        public WeaponType myWeapon = WeaponType.None;

        public enum WeaponType
        {
            None,
            Axe = 13,
            PickAxe = 15,
            Two_Handed = 14,
            One_Handed = 11,
            Bow = 12
        }

        void ChangeWeapon(WeaponType mw)
        {
            if (myWeapon == mw) return;
            myWeapon = mw;
            switch (myWeapon)
            {
                case WeaponType.None:
                    myInfo.BattleDelay_Melee_AfterBuff = 1.0f;
                    break;
                case WeaponType.Two_Handed:
                    myInfo.BattleDelay_Melee_AfterBuff = 5.0f;
                    break;
                case WeaponType.One_Handed:
                    myInfo.BattleDelay_Melee_AfterBuff = 3.0f;
                    break;
                case WeaponType.Bow:
                    myInfo.BattleDelay_Melee_AfterBuff = 3.0f;
                    break;
            }
        }
        #endregion
        /***********************************************************************
        *                               Animation Event
        ***********************************************************************/
        #region Event
        public void OnAttack()
        {
            if (myInfo.curAtDelay <= Mathf.Epsilon)
            {
                myInfo.curAtDelay = myInfo.BattleDelay_Melee_AfterBuff;
                switch (myWeapon)
                {
                    case WeaponType.None:
                        Com.anim.SetTrigger("RHandAttack");
                        break;
                    case WeaponType.Two_Handed:
                        Com.anim.SetTrigger("TwoHandedAttack");
                        break;
                    case WeaponType.One_Handed:
                        Com.anim.SetTrigger("OneHandedAttack");
                        break;
                }
            }

            StartCoroutine(CoolDown());
        }

        public void OnAttack_Bow()
        {
            myArrow.Shooting();
        }

        public void OnAttack_None()
        {
            Collider[] list = Physics.OverlapSphere(myAttackpos[(int)myWeapon].position, 0.13f, PlayerScript.instance.plMask.MaskEnemy);

            foreach (Collider col in list)
            {
                IBattle ib = col.GetComponent<IBattle>();
                ib.OnDamage(myInfo.myDmg);
            }
        }

        public void OnAttack_TwoHanded()
        {
            Vector3 startPos = myAttackpos[(int)myWeapon].position;
            startPos.y = startPos.y - 0.3f;
            Vector3 endPos = myAttackpos[(int)myWeapon].position;
            endPos.y = endPos.y + 0.35f;

            Collider[] list = Physics.OverlapCapsule(startPos, endPos, 0.08f, PlayerScript.instance.plMask.MaskEnemy);

            foreach (Collider col in list)
            {
                IBattle ib = col.GetComponent<IBattle>();
                ib.OnDamage(myInfo.myDmg);
            }
        }

        public void OnAttack_OneHanded()
        {
            Vector3 startPos = myAttackpos[(int)myWeapon].position;
            startPos.y = startPos.y - 0.2f;
            Vector3 endPos = myAttackpos[(int)myWeapon].position;
            endPos.y = endPos.y + 0.25f;

            Collider[] list = Physics.OverlapCapsule(startPos, endPos, 0.05f, PlayerScript.instance.plMask.MaskEnemy);

            foreach (Collider col in list)
            {
                IBattle ib = col.GetComponent<IBattle>();
                ib.OnDamage(myInfo.myDmg);
            }
        }
        #endregion
        /***********************************************************************
        *                               Inventory System
        ***********************************************************************/
        #region Inventory System
        private void PickObjectItem(GameObject obj)
        {
            if (obj.TryGetComponent(out GroundItem gi))
            {
                inventory.AddItem(new Item(gi.item), 1);
                Destroy(obj);
            }
        }
        private void OnApplicationQuit()   // 실행(게임)이 종료되면 인벤토리 초기화
        {
            inventory.Container.Items = new InventorySlot[9];
        }
        #endregion
        /***********************************************************************
        *                               Coroutine
        ***********************************************************************/
        #region .
        /// <summary> TEST FIELD </summary>
        /*
        public float testZoomSpeed = 0f;
        public float boolTEST = 1;
        IEnumerator StartZoomCamera()
        {
            Vector3 tpCameraPos = Com.tpCamera.transform.localPosition + Vector3.up;
            Vector3 startZoomCameraPos = StartCamera.transform.localPosition;
            Quaternion tpCameraQuat = Com.tpCamera.transform.localRotation;
            Quaternion startZoomCameraQuat = StartCamera.transform.localRotation;

            //y,z,rotate dist

            Vector3 diry = tpCameraPos - startZoomCameraPos;
            diry.Normalize();
            float disty = MathF.Abs(tpCameraPos.y - startZoomCameraPos.y); //이동할 방향과 거리
            float delta;
            float logValue = 1.1f;

            while (disty > 0f)
            {
                Debug.Log(Mathf.Log(logValue, boolTEST));
                delta = testZoomSpeed * Time.deltaTime * Mathf.Log(logValue, boolTEST);
                if (delta > MathF.Abs(disty)) delta = disty;
                disty -= delta;
                StartCamera.transform.localPosition += diry * delta;
                logValue += 0.01f;
                //StartCamera.transform.Translate(dir * delta, Space.Self);
                /*StartCamera.transform.Translate(Vector3.MoveTowards(
                    StartCamera.transform.position,
                    new Vector3(0, Mathf.Lerp(startZoomCameraPos.y, tpCameraPos.y, _deltaTime * testZoomSpeed), 0),
                    float.MaxValue), Space.Self);*/
        /*
                //StartCamera.transform.localPosition = new Vector3(0, 0, Mathf.Lerp(startZoomCameraPos.z, tpCameraPos.z, 0.5f));
                //(new Vector3(0, Mathf.Lerp(startZoomCameraPos.y, tpCameraPos.y, _deltaTime * testZoomSpeed), 0), Space.Self);
                //StartCamera.transform.Translate(new Vector3(0, 0, Mathf.Lerp(startZoomCameraPos.z, tpCameraPos.z, _deltaTime * testZoomSpeed)), Space.Self);
                //StartCamera.transform.eulerAngles = new Vector3(Mathf.Lerp(startZoomCameraQuat.x, tpCameraQuat.x, _deltaTime * testZoomSpeed), 0, 0);
                yield return null;
            }

            StartCamera.SetActive(false);
            ChangeState(STATE.NORMAL);
        }
        */
        IEnumerator CoolDown()
        {
            while (myInfo.curAtDelay >= Mathf.Epsilon)
            {
                myInfo.curAtDelay -= Time.deltaTime;
                yield return null;
            }
        }
        #endregion
        /***********************************************************************
        *                               Collision Event
        ***********************************************************************/
        #region collisionEvent
        #endregion
        #region TriggerEvent
        private void OnTriggerEnter(Collider other)
        {
        }
        #endregion


        /***********************************************************************
        *                               산술 연산
        ***********************************************************************/
        #region 산술연산
        public Vector3 GetMidMonitor()
        {
            if (_state.isCurrentFp)
                return new Vector3(_components.fpCamera.pixelWidth / 2, _components.fpCamera.pixelHeight / 2);
            else
                return new Vector3(_components.tpCamera.pixelWidth / 2, _components.tpCamera.pixelHeight / 2);
        }
        #endregion
        public Vector3 GetPlayerPosition()
        {
            return transform.position;
        }

        public void AnimGetheringItem()
        {
            if (GatherItem.handItem != null)
            {
                PlayerScript.PlayerInstance.inventory.AddItem(new Item(GatherItem.handItem.item), 1);
                Destroy(GatherItem.handItem.gameObject);
            }
        }
        public void AnimDamageTpRock()
        {
            Mining mining = _mouseInput.Mining;
            if (mining.RockScript != null)
            {
                mining.RockScript.DamToRock(mining.RockHitRay, mining.RockHitVector, PlayerScript.PlayerInstance.myInfo.PowerMining_Origin);
            }
        }
        public void AnimDamageToTree()
        {
            Mining mining = _mouseInput.Mining;
            if (mining.TreeScript != null)
            {
                mining.TreeScript.DamtoTree(50f);
            }
        }
    }
}