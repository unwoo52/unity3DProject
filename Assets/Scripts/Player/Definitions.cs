using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Definitions
{

    [Serializable]
    public class Components
    {
        public Camera tpCamera;
        public Camera fpCamera;

        [HideInInspector] public Transform tpRig;
        [HideInInspector] public Transform fpRoot;
        [HideInInspector] public Transform fpRig;

        [HideInInspector] public GameObject tpCamObject;
        [HideInInspector] public GameObject fpCamObject;

        [HideInInspector] public Rigidbody rBody;
        [HideInInspector] public Animator anim;
    }
    [Serializable]
    public class Gadget
    {
        [Tooltip("장비창 - 무기")]
        public GameObject RightHand;
        [Tooltip("장비창 - 모자")]
        public GameObject Helmet;
        [Tooltip("장비창 - 상의")]
        public GameObject Jacket;
        [Tooltip("장비창 - 하의")]
        public GameObject Pants;
        [Tooltip("장비창 - 신발")]
        public GameObject Shoes;
        [Tooltip("장비창 - 장갑")]
        public GameObject Gloves;
        [Tooltip("장비창 - 망토")]
        public GameObject Cloak;
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
        public KeyCode switchCamera = KeyCode.V;
        public KeyCode showCursor = KeyCode.C;
        public KeyCode LeftClick = KeyCode.Mouse0;
        public KeyCode RightClick = KeyCode.Mouse1;
        public KeyCode RollingBody = KeyCode.Q;
        public KeyCode Esc = KeyCode.Escape;

        /*
        public KeyCode moveForward = KeyManager.Inst.KeyPairs[KeyAction.Forward];
        public KeyCode moveBackward = KeyManager.Inst.KeyPairs[KeyAction.Backward];
        public KeyCode moveLeft = KeyManager.Inst.KeyPairs[KeyAction.Left];
        public KeyCode moveRight = KeyManager.Inst.KeyPairs[KeyAction.Right];
        public KeyCode run = KeyManager.Inst.KeyPairs[KeyAction.Run];
        public KeyCode jump = KeyManager.Inst.KeyPairs[KeyAction.Jump];
        public KeyCode switchCamera = KeyManager.Inst.KeyPairs[KeyAction.SwitchCamera];
        public KeyCode showCursor = KeyManager.Inst.KeyPairs[KeyAction.ShowCursor];
        public KeyCode LeftClick = KeyManager.Inst.KeyPairs[KeyAction.LeftClick];
        public KeyCode RightClick = KeyManager.Inst.KeyPairs[KeyAction.RightClick];
        public KeyCode RollingBody = KeyManager.Inst.KeyPairs[KeyAction.RollingBody];
        public KeyCode Esc = KeyManager.Inst.KeyPairs[KeyAction.Esc];
        */
    }

    [Serializable]
    public class MovementOption
    {
        [Range(1f, 3f), Tooltip("달리기 이동속도 증가 계수")]
        public float runningCoef = 1.5f;
        [Range(0f, 5f), Tooltip("점프 외의 추락 할 때 적용 받는 중력")]
        public float rigidGrabity = 3.5f;
        [Range(1f, 10f), Tooltip("점프 강도")]
        public float jumpForce = 5.5f;
        [Range(1f, 5f), Tooltip("이동키 입력시 가속력")]
        public float acceleration = 1.0f;
        [Tooltip("지면으로 체크할 레이어 설정")]
        public LayerMask groundLayerMask = -1;
        [Range(0.0f, 2.0f), Tooltip("점프 쿨타임")]
        public float jumpCooldown = 0.3f;
    }
    [Serializable]
    public class CameraOption
    {
        public enum CameraEnum { FpCamera, TpCamera };
        [Tooltip("게임 시작 시 카메라")]
        public CameraEnum initialCamera;
        [Range(1f, 10f), Tooltip("카메라 상하좌우 회전 속도")]
        public float rotationSpeed = 3f;
        [Range(-90f, 0f), Tooltip("올려다보기 제한 각도")]
        public float lookUpDegree = -60f;
        [Range(0f, 75f), Tooltip("내려다보기 제한 각도")]
        public float lookDownDegree = 75f;
        [Range(0f, 3.5f), Space, Tooltip("줌 확대 최대 거리")]
        public float zoomInDistance = 3f;
        [Range(0f, 5f), Tooltip("줌 축소 최대 거리")]
        public float zoomOutDistance = 3f;
        [Range(1f, 20f), Tooltip("줌 속도")]
        public float zoomSpeed = 10f;
        [Range(0.01f, 0.5f), Tooltip("줌 가속")]
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
        public bool isJumping;
    }

    [Serializable]
    public class PlayerInfoMask
    {
        public LayerMask MaskMouseTrigger;
        public LayerMask MaskTerrain;
        public LayerMask MaskItem;
        public LayerMask MaskObjectItem;
        public LayerMask MaskResource;
        public LayerMask MaskEnemy;
        public LayerMask currentTerrainLayer;
    }   
}