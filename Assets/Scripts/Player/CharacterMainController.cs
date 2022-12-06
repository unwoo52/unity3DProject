using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��¥ : 2021-02-12 PM 7:45:19
// �ۼ��� : Rito

public interface IMovement3D
{
    /// <summary> ���� �̵� ������ ���� </summary>
    bool IsMoving();
    /// <summary> ���鿡 ��� �ִ��� ���� </summary>
    bool IsGrounded();
    /// <summary> �������κ����� �Ÿ� </summary>
    float GetDistanceFromGround();

    /// <summary> ���� �̵����� �ʱ�ȭ(�̵� ���) </summary>
    void SetMovement(in Vector3 worldMoveDirection, bool isRunning);
    /// <summary> ���� ��� - ���� ���� ���� ���� </summary>
    bool SetJump();
    /// <summary> �̵� ���� </summary>
    void StopMoving();

    /// <summary> ���ĳ��� </summary>
    void KnockBack(in Vector3 force, float time);
}

namespace Rito.CharacterControl
{
    public class CharacterMainController : MonoBehaviour
    {
        /*

            [���̶�Ű ����]

            Character Root [ CharacterMainController, Movement3D, Rigidbody, Capsule Collider ]
              ��TP Root    [ Pos(0,   1,   0) , Rot(0,  0, 0) ]
                ��TP Rig   [ Pos(0,   0,   0) , Rot(0, 45, 0) ]
                  ��TP Cam [ Pos(0,   0,  -4) , Rot(0,  0, 0), Camera Component ]
              ��Walker     [ Pos(0,   1,   0) , Rot(0,  0, 0) ]
                ��FP Rig   [ Pos(0, 1.3, 0.2) , Rot(0,  0, 0) ]
                  ��FP Cam [ Pos(0, 1.3, 0.2) , Rot(0,  0, 0), Camera Component ]
                ��Model Root

        */

        /***********************************************************************
        *                               Definitions
        ***********************************************************************/
        #region .
        public enum CameraType { FpCamera, TpCamera };

        [Serializable]
        public class Components
        {
            public Camera tpCamera;
            public Camera fpCamera;

            [HideInInspector] public Transform tpRoot;
            [HideInInspector] public Transform tpRig;
            [HideInInspector] public Transform walker;
            [HideInInspector] public Transform fpRig;

            [HideInInspector] public GameObject tpCamObject;
            [HideInInspector] public GameObject fpCamObject;

            [HideInInspector] public Animator anim;
            [HideInInspector] public IMovement3D movement3D;
        }
        [Serializable]
        public class KeyOption
        {
            public KeyCode moveForward = KeyCode.W;
            public KeyCode moveBackward = KeyCode.S;
            public KeyCode moveLeft = KeyCode.A;
            public KeyCode moveRight = KeyCode.D;
            public KeyCode run = KeyCode.LeftShift;
            public KeyCode jump = KeyCode.Space;
            public KeyCode switchCamera = KeyCode.Tab;
            public KeyCode showCursor = KeyCode.LeftAlt;
        }
        [Serializable]
        public class CameraOption
        {
            [Tooltip("���� ���� �� ī�޶�")]
            public CameraType initialCamera;

            [Range(1f, 10f), Tooltip("ī�޶� �����¿� ȸ�� �ӵ�")]
            public float rotationSpeed = 2f;

            [Range(-90f, 0f), Tooltip("�÷��ٺ��� ���� ����")]
            public float lookUpDegree = -60f;

            [Range(0f, 75f), Tooltip("�����ٺ��� ���� ����")]
            public float lookDownDegree = 75f;

            [Range(0f, 3.5f), Space, Tooltip("�� Ȯ�� �ִ� �Ÿ�")]
            public float zoomInDistance = 3f;

            [Range(0f, 5f), Tooltip("�� ��� �ִ� �Ÿ�")]
            public float zoomOutDistance = 3f;

            [Range(1f, 30f), Tooltip("�� �ӵ�")]
            public float zoomSpeed = 20f;

            [Range(0.01f, 0.5f), Tooltip("�� ����")]
            public float zoomAccel = 0.1f;
        }
        [Serializable]
        public class AnimatorOption
        {
            public string paramMoveX = "Move X";
            public string paramMoveZ = "Move Z";
            public string paramDistY = "Dist Y";
            public string paramGrounded = "Grounded";
            public string paramJump = "Jump";
        }
        [Serializable]
        public class CharacterState
        {
            public bool isCurrentFp;
            public bool isMoving;
            public bool isRunning;
            public bool isGrounded;
            public bool isCursorActive;
        }

        #endregion
        /***********************************************************************
        *                               Fields, Properties
        ***********************************************************************/
        #region .
        public Components Com => _components;
        public KeyOption Key => _keyOption;
        public CameraOption CamOption => _cameraOption;
        public AnimatorOption AnimOption => _animatorOption;
        public CharacterState State => _state;

        [SerializeField] private Components _components = new Components();
        [Space, SerializeField] private KeyOption _keyOption = new KeyOption();
        [Space, SerializeField] private CameraOption _cameraOption = new CameraOption();
        [Space, SerializeField] private AnimatorOption _animatorOption = new AnimatorOption();
        [Space, SerializeField] private CharacterState _state = new CharacterState();

        /// <summary> Time.deltaTime �׻� ���� </summary>
        private float _deltaTime;

        /// <summary> ���콺 �������� ���� ��� ȸ�� �� </summary>
        private Vector2 _rotation;


        [SerializeField]
        private float _distFromGround;

        // Animation Params
        private float _moveX;
        private float _moveZ;


        /// <summary> TP ī�޶� ~ Rig �ʱ� �Ÿ� </summary>
        private float _tpCamZoomInitialDistance;

        /// <summary> TP ī�޶� �� �Է� �� </summary>
        private float _tpCameraWheelInput = 0;

        /// <summary> ���������� ���� �� �Է� �� </summary>
        private float _currentWheel;



        // Current Movement Variables

        /// <summary> Ű���� WASD �Է����� ��� ���� �̵� ���� </summary>
        [SerializeField]
        private Vector3 _moveDir;

        /// <summary> ���� �̵� ���� </summary>
        [SerializeField]
        private Vector3 _worldMoveDir;

        #endregion

        /***********************************************************************
        *                               Unity Events
        ***********************************************************************/
        #region .
        private void Start()
        {
            InitComponents();
            InitSettings();
        }

        private void Update()
        {
            _deltaTime = Time.deltaTime;

            // 1. Check, Key Input
            ShowCursorToggle();
            CameraViewToggle();
            SetValuesByKeyInput();

            // 2. Behaviors, Camera Actions
            Rotate();
            TpCameraZoom();

            // 3. Updates
            CheckGroundDistance();
            UpdateAnimationParams();
        }

        #endregion
        /***********************************************************************
        *                               Init Methods
        ***********************************************************************/
        #region .
        private void InitComponents()
        {
            LogNotInitializedComponentError(Com.tpCamera, "TP Camera");
            LogNotInitializedComponentError(Com.fpCamera, "FP Camera");

            Com.anim = GetComponentInChildren<Animator>();

            Com.tpCamObject = Com.tpCamera.gameObject;
            Com.tpRig = Com.tpCamera.transform.parent;
            Com.tpRoot = Com.tpRig.parent;

            Com.fpCamObject = Com.fpCamera.gameObject;
            Com.fpRig = Com.fpCamera.transform.parent;
            Com.walker = Com.fpRig.parent;

            TryGetComponent(out Com.movement3D);
            //if(Com.movement3D == null)
            //    Com.movement3D = gameObject.AddComponent<PhysicsBasedMovement>();
            //Com.pbMove = gameObject.AddComponent<Test.PBMove2>();
        }

        private void InitSettings()
        {
            // ��� ī�޶� ���ӿ�����Ʈ ��Ȱ��ȭ
            var allCams = FindObjectsOfType<Camera>();
            foreach (var cam in allCams)
            {
                cam.gameObject.SetActive(false);
            }

            // ������ ī�޶� �ϳ��� Ȱ��ȭ
            State.isCurrentFp = (CamOption.initialCamera == CameraType.FpCamera);
            Com.fpCamObject.SetActive(State.isCurrentFp);
            Com.tpCamObject.SetActive(!State.isCurrentFp);

            // Zoom
            _tpCamZoomInitialDistance = Vector3.Distance(Com.tpRig.position, Com.tpCamera.transform.position);
        }

        #endregion
        /***********************************************************************
        *                               Check Methods
        ***********************************************************************/
        #region .
        private void LogNotInitializedComponentError<T>(T component, string componentName) where T : Component
        {
            if (component == null)
                Debug.LogError($"{componentName} ������Ʈ�� �ν����Ϳ� �־��ּ���");
        }

        #endregion
        /***********************************************************************
        *                               Methods
        ***********************************************************************/
        #region .
        /// <summary> Ű���� �Է��� ���� �ʵ� �ʱ�ȭ </summary>
        private void SetValuesByKeyInput()
        {
            float h = 0f, v = 0f;

            if (Input.GetKey(Key.moveForward)) v += 1.0f;
            if (Input.GetKey(Key.moveBackward)) v -= 1.0f;
            if (Input.GetKey(Key.moveLeft)) h -= 1.0f;
            if (Input.GetKey(Key.moveRight)) h += 1.0f;

            // Move, Rotate
            SendMoveInfo(h, v);
            _rotation = new Vector2(Input.GetAxisRaw("Mouse X"), -Input.GetAxisRaw("Mouse Y"));

            State.isMoving = h != 0 || v != 0;
            State.isRunning = Input.GetKey(Key.run);

            // Jump
            if (Input.GetKeyDown(Key.jump))
            {
                Jump();
            }

            // Wheel
            _tpCameraWheelInput = Input.GetAxisRaw("Mouse ScrollWheel");
            _currentWheel = Mathf.Lerp(_currentWheel, _tpCameraWheelInput, CamOption.zoomAccel);
        }

        private void Rotate()
        {
            Transform root, rig;

            // 1��Ī
            if (State.isCurrentFp)
            {
                root = Com.walker;
                rig = Com.fpRig;
            }
            // 3��Ī
            else
            {
                root = Com.tpRoot;
                rig = Com.tpRig;
                RotateWalker(); // 3��Ī�� ��� Walker�� �̵��������� ȸ��
            }

            if (State.isCursorActive) return;

            // ȸ�� ==========================================================
            float deltaCoef = _deltaTime * 50f;

            // ���� : Rig ȸ��
            float xRotPrev = rig.localEulerAngles.x;
            float xRotNext = xRotPrev + _rotation.y
                * CamOption.rotationSpeed * deltaCoef;

            if (xRotNext > 180f)
                xRotNext -= 360f;

            // �¿� : Root ȸ��
            float yRotPrev = root.localEulerAngles.y;
            float yRotNext =
                yRotPrev + _rotation.x
                * CamOption.rotationSpeed * deltaCoef;

            // ���� ȸ�� ���� ����
            bool xRotatable =
                CamOption.lookUpDegree < xRotNext &&
                CamOption.lookDownDegree > xRotNext;

            // Rig ���� ȸ�� ����
            rig.localEulerAngles = Vector3.right * (xRotatable ? xRotNext : xRotPrev);

            // Root �¿� ȸ�� ����
            root.localEulerAngles = Vector3.up * yRotNext;
        }

        /// <summary> 3��Ī�� ��� Walker ȸ�� </summary>
        private void RotateWalker()
        {
            if (State.isMoving == false) return;

            Vector3 dir = Com.tpRig.TransformDirection(_moveDir);
            float currentY = Com.walker.localEulerAngles.y;
            float nextY = Quaternion.LookRotation(dir, Vector3.up).eulerAngles.y;

            if (nextY - currentY > 180f) nextY -= 360f;
            else if (currentY - nextY > 180f) nextY += 360f;

            Com.walker.eulerAngles = Vector3.up * Mathf.Lerp(currentY, nextY, 0.1f);
        }

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

        private void CameraViewToggle()
        {
            if (Input.GetKeyDown(Key.switchCamera))
            {
                State.isCurrentFp = !State.isCurrentFp;
                Com.fpCamObject.SetActive(State.isCurrentFp);
                Com.tpCamObject.SetActive(!State.isCurrentFp);

                // TP -> FP
                if (State.isCurrentFp)
                {
                    Com.walker.localEulerAngles = Vector3.up * Com.tpRoot.localEulerAngles.y;
                    Com.fpRig.localEulerAngles = Vector3.right * Com.tpRig.localEulerAngles.x;
                }
                // FP -> TP
                else
                {
                    Com.tpRoot.localEulerAngles = Vector3.up * Com.walker.localEulerAngles.y;
                    Com.tpRig.localEulerAngles = Vector3.right * Com.fpRig.localEulerAngles.x;
                }
            }
        }

        private void TpCameraZoom()
        {
            if (State.isCurrentFp) return;                // TP ī�޶� ����
            if (Mathf.Abs(_currentWheel) < 0.01f) return; // �� �Է� �־�� ����

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

            // ����
            const float LerpSpeed = 0.05f;
            _moveX = Mathf.Lerp(_moveX, x, LerpSpeed);
            _moveZ = Mathf.Lerp(_moveZ, z, LerpSpeed);

            Com.anim.SetFloat(AnimOption.paramMoveX, _moveX);
            Com.anim.SetFloat(AnimOption.paramMoveZ, _moveZ);
            Com.anim.SetFloat(AnimOption.paramDistY, _distFromGround);
            Com.anim.SetBool(AnimOption.paramGrounded, State.isGrounded);
        }

        #endregion
        /***********************************************************************
        *                               Movement Methods
        ***********************************************************************/
        #region .
        /// <summary> �����κ����� �Ÿ� üũ - �ִϸ����� ���޿� </summary>
        private void CheckGroundDistance()
        {
            _distFromGround = Com.movement3D.GetDistanceFromGround();
            State.isGrounded = Com.movement3D.IsGrounded();
        }

        private void SendMoveInfo(float horizontal, float vertical)
        {
            _moveDir = new Vector3(horizontal, 0f, vertical).normalized;

            if (State.isCurrentFp)
            {
                _worldMoveDir = Com.walker.TransformDirection(_moveDir);
            }
            else
            {
                _worldMoveDir = Com.tpRoot.TransformDirection(_moveDir);
            }

            Com.movement3D.SetMovement(_worldMoveDir, State.isRunning);
        }

        private void Jump()
        {
            bool jumpSucceeded = Com.movement3D.SetJump();

            if (jumpSucceeded)
            {
                // �ִϸ��̼� ���� Ʈ����
                Com.anim.SetTrigger(AnimOption.paramJump);

                Debug.Log("JUMP");
            }
        }
        #endregion
        /***********************************************************************
        *                               Public Methods
        ***********************************************************************/
        #region .
        public void KnockBack(in Vector3 force, float time)
        {
            Com.movement3D.KnockBack(force, time);
        }

        #endregion
    }
}