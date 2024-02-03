using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float crouchSpeed = 1f;
    [SerializeField] private bool characterCrouch = false;
    public Vector3 move;

    [Header("Ground Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundDistance = 0.4f;
    public bool isGrounded;
    private Vector3 velocity;
    //private bool isCrouch;

    [Header("Character Speed Timer")]
    [SerializeField] private float minTime = 10f;
    [SerializeField] private float maxTime = 0f;
    private float currentTime;
    private bool characterTired = false;
    public HeadbobSystem headbobControl;

    [Header("Weapon")]
    public Weapon currentActiveWeapon;
    public Weapon[] allWeapons;

    private void Start()
    {
        ChangeWeapon(0);

        currentTime = maxTime;
    }

    private void ChangeWeapon(int weaponID)
    {


        for (int i = 0; i < allWeapons.Length; i++)
        {
            allWeapons[i].gameObject.SetActive(false);
        }

        allWeapons[weaponID].gameObject.SetActive(true);
        currentActiveWeapon = allWeapons[weaponID];
    }


    // Update is called once per frame
    void Update()
    {
        CharacterMovement();
        CheckWeaponInput();
        CheckChangeWeaponInput();
    }

    #region MOVEMENT

    void CharacterMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        move = transform.right * x + transform.forward * y;
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        CharacterFast();
        CharacterCrouch();
    }

    void CharacterFast()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !characterTired && !characterCrouch)
        {
            playerSpeed = 12;
            headbobControl.bobbingAmount = 0.05f;
            headbobControl.walkingBobbingSpeed = 14f;
            currentTime = Mathf.Max(currentTime - 1f * Time.deltaTime, minTime);
            if (currentTime <= maxTime / 3f)
            {
                characterTired = true;
            }
        }
        else
        {
            playerSpeed = 6;
            currentTime = Mathf.Min(currentTime + 1f * Time.deltaTime, maxTime);
            headbobControl.bobbingAmount = 0.03f;
            headbobControl.walkingBobbingSpeed = 10f;
            if (currentTime >= maxTime / 2f)
            {
                characterTired = false;
            }
        }
    }


    void CharacterCrouch()
    {
        if (Input.GetKey(KeyCode.C))
        {
            crouchHeight = 1.2f;
            controller.height = Mathf.Lerp(controller.height, crouchHeight, crouchSpeed * Time.deltaTime);
            isGrounded = true;
            characterCrouch = true;
            headbobControl.bobbingAmount = 0.01f;
            headbobControl.walkingBobbingSpeed = 7f;        }
        else
        {
            controller.height = Mathf.Lerp(controller.height, 1.8f, crouchSpeed * Time.deltaTime);
            characterCrouch = false;
        }
    }

    #endregion

    #region WEAPON

    private void CheckChangeWeaponInput()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1)) ChangeWeapon(0);
        if (Input.GetKeyUp(KeyCode.Alpha2)) ChangeWeapon(1);
        if (Input.GetKeyUp(KeyCode.Alpha3)) ChangeWeapon(2);
    }

    void CheckWeaponInput()
    {
        if (currentActiveWeapon == null) return;

        switch (currentActiveWeapon.weaponProperties._mode)
        {
            case GunType.ShootingMode.single:
                if (Input.GetButtonDown("Fire1")) FireRequest();
                break;
            case GunType.ShootingMode.serial:
                if (Input.GetButton("Fire1")) FireRequest();
                break;
        }
    }


    void FireRequest()
    {
        if (currentActiveWeapon == null) return;

        currentActiveWeapon.FireRequest();
    }

    


    #endregion


}
