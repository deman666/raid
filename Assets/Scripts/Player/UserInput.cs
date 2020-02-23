using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    CharacterMovement characterMove;

    WeaponHandler weaponHandler;

    [System.Serializable]
    public class InputSettings
    {
        public string verticalAxis = "Vertical";
        public string horizontalAxis = "Horizontal";
        public string jumpButton = "Jump";
        public string reloadButton = "Reload";
        public string aimButton = "Fire2";
        public string fireButton = "Fire1";
        public string dropWeaponButton = "DropWeapon";
        public string swtitchWeaponButton = "SwitchWeapon";
       

    }
    [SerializeField]
    InputSettings input;

    [System.Serializable]
    public class OtherSettings
    {
        public float lookSpeed = 5.0f;
        public float lookDistance = 10.0f;
        public bool requireInputForTurn = true;
        public LayerMask aimDetectionLayers;
    }
    [SerializeField]
    public OtherSettings other;

    public bool debugAim;
    public Transform spine;
    bool aiming;

    public Camera TPSCamera;

    private void Start()
    {
        characterMove = GetComponent<CharacterMovement>();
        
        weaponHandler = GetComponent<WeaponHandler>();
    }

    private void Update()
    {

        CharacterLogic();
        CameraLookLogic();
        WeaponLogic();
        
    }
    private void LateUpdate()
    {
        if (weaponHandler)
        {
            if (weaponHandler.currentWeapon)
            {
                if (aiming)
                {
                    PositioningSpine();
                }
            }
        }
    }

    void CharacterLogic()
    {
        if (!characterMove)
            return;
        if (characterMove)
        {
            characterMove.Animate(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal"));
            if (Input.GetButtonDown("Jump"))
            {
                characterMove.Jump();
            }

        }
    }
    void CameraLookLogic()
    {
        if (!TPSCamera)
            return;

            if (other.requireInputForTurn)
            {
                if ((Input.GetAxis(input.horizontalAxis) != 0 || Input.GetAxis(input.verticalAxis) != 0))
                {
                    CharacterLook();
                }
            }
            else
            {
                CharacterLook();
            }

        
    }
    void WeaponLogic()
    {
        if (!weaponHandler)
            return;

        aiming = Input.GetButton(input.aimButton);

        if (weaponHandler.currentWeapon)
        {
            weaponHandler.Aim(aiming);

            other.requireInputForTurn = !aiming;

            weaponHandler.FingerOnTrigger(Input.GetButton(input.fireButton));

            if (Input.GetButtonDown(input.reloadButton))
                weaponHandler.Reload();
            // #Adrian temp comment-out
            //if (Input.GetButtonDown(input.dropWeaponButton))
            //    weaponHandler.DropCurrentWeapon();
            if (Input.GetButtonDown(input.swtitchWeaponButton))
                weaponHandler.SwtichWeapons();

        }
    }
    void PositioningSpine()
    {
        if (!spine || !weaponHandler.currentWeapon || !TPSCamera)
            return;

        Transform mainCamT = TPSCamera.transform;
        Vector3 mainCamPos = mainCamT.position;
        Vector3 dir = mainCamT.forward;
        Ray ray = new Ray(mainCamPos, dir);

        spine.LookAt(ray.GetPoint(50));

        Vector3 eulerAngleOffset = weaponHandler.currentWeapon.userSettings.spineRotation;
        spine.Rotate(eulerAngleOffset);
    }

        void CharacterLook()
        {
            Transform mainCamT = TPSCamera.transform;
            Transform pivotT = mainCamT.parent;
            Vector3 pivotPos = pivotT.position;
            Vector3 lookTarget = pivotPos + (pivotT.forward * other.lookDistance);
            Vector3 thisPos = transform.position;
            Vector3 lookDir = lookTarget - thisPos;
            Quaternion lookRot = Quaternion.LookRotation(lookDir);
            lookRot.x = 0;
            lookRot.z = 0;

            Quaternion newRotation = Quaternion.Lerp(transform.rotation, lookRot, Time.deltaTime * other.lookSpeed);
            transform.rotation = newRotation;
        }


    

}
