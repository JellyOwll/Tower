using Cinemachine;
using Com.JellyOwl.Tower.UI;
using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public delegate void PlayerEventHandler(Player sender);
    protected Rigidbody rb;

    [Header("Camera")]
    [SerializeField]
    protected float mouseSensitivity = 100f;
    [SerializeField]
    protected GameObject cam;
    [SerializeField]
    protected GameObject slowMoEffect;

    protected float xRotation;
    [SerializeField]
    protected float minClampCam = -90f;
    [SerializeField]
    protected float maxClampCam = 90;

    [Header("Player")]
    [SerializeField]
    protected float velocity = 100f;
    [SerializeField]
    protected float jumpHeight = 3f;
    [SerializeField]
    protected float launchForce = 3f;
    [SerializeField]
    protected Transform groundCheck;
    [SerializeField]
    protected float groundDistance;
    [SerializeField]
    protected LayerMask groundMask;
    public Transform weaponHolder;
    public Transform bowHolder;
    protected bool isGrounded;
    [HideInInspector]
    public bool hasWeapon;

    protected Action DoAction;
    [HideInInspector]
    public Transform currentObject;
    protected float objectDistance;

    [SerializeField]
    protected float force;
    [SerializeField]
    private float forceMax = 2f;
    [SerializeField]
    protected float incrementForce;

    static public event PlayerEventHandler OnThrow;
    static public event PlayerEventHandler OnPick;
    static public event PlayerEventHandler ForceChanged;
    static public event PlayerEventHandler OnAttack;

    [Header("HUD")]
    [SerializeField]
    protected GameObject HUD;

    public float Force { 
        get => force;
        set 
        { 
            force = value;
            ForceChanged?.Invoke(this);
        }
    }

    public float ForceMax { get => forceMax;}

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Ladder.EnterLadder += Ladder_EnterLadder;
        Ladder.ExitLadder += Ladder_ExitLadder;
        MenuUI.OnPlayClick += MenuUI_OnPlayClick;
        DoAction = DoActionVoid;
        HUD.SetActive(false);
    }

    private void MenuUI_OnPlayClick()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam.SetActive(true);
        DoAction = DoActionNormal;
        HUD.SetActive(true);
    }

    private void DoActionVoid()
    {

    }

    private void Ladder_ExitLadder(Ladder sender)
    {
        DoAction = DoActionNormal;
        rb.isKinematic = false;
    }

    private void Ladder_EnterLadder(Ladder sender)
    {
        DoAction = DoActionLadder;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        DoAction();
    }

    private void FixedUpdate()
    {
        Time.fixedDeltaTime = .02f * Time.timeScale;
    }

    private void Gravity()
    {
        int layerMask = 1 << 9;
        layerMask = ~layerMask;
        isGrounded = Physics.Raycast(groundCheck.position, -transform.up, 0.5f, layerMask) ;
        Debug.DrawRay(groundCheck.position, -transform.up * 0.5f) ;
    }

    private void Control()
    {
        if (Input.GetButtonDown("Jump")){
            if (isGrounded)
            {
                rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);

            }
        }
        float x = Input.GetAxis("Horizontal") * velocity * Time.fixedDeltaTime;
        float z = Input.GetAxis("Vertical") * velocity * Time.fixedDeltaTime;

        Vector3 move = transform.right * x + transform.forward * z;

        rb.MovePosition(rb.transform.position + move);
        
    }

    private void ControlCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minClampCam, maxClampCam);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate((Vector3.up * mouseX));
    }

    protected void DoActionNormal()
    {
        Control();
        ControlCamera();
        Gravity();
        CheckMovableObject();
        UseObject();
    }

    private void UseObject()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isGrounded)
            {
                Time.timeScale = 0.25f;
                slowMoEffect.SetActive(true);
            }else
            {
                Time.timeScale = 1f;
                slowMoEffect.SetActive(false);
            }
            Force += incrementForce * Time.deltaTime;
            if (Force >= ForceMax)
            {
                Force = ForceMax;
            }
        }
        if(Input.GetButtonDown("Fire1"))
        {
            Tween.StopAll();
            Tween.Value(cam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView, 90, UpdateFOV, 0.5f, 0, Tween.EaseInOut);

        }
        if (Input.GetButtonUp("Fire1"))
        {
            Tween.StopAll();
            Tween.Value(cam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView, 60, UpdateFOV, 0.5f, 0, Tween.EaseInOut);
            OnAttack.Invoke(this);
            Force = 0;
            Time.timeScale = 1f;
            slowMoEffect.SetActive(false);
        }

        if (currentObject != null)
        {
            if (hasWeapon)
            {
                return;
            }
            currentObject.position = cam.transform.position + cam.transform.forward * objectDistance;
            currentObject.rotation = cam.transform.rotation;
        }

       
    }

    private void UpdateFOV(float value)
    {
            cam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView = value;

    }

    private void CheckMovableObject()
    {
        if (Input.GetButtonDown("Use"))
        {
            if (currentObject == null)
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 3))
                {
                    if (hit.collider.GetComponent<MovableObject>())
                    {
                        currentObject = hit.collider.transform;
                        objectDistance = hit.distance;
                        OnPick.Invoke(this);
                        if (hasWeapon)
                        {
                            return;
                        }
                        currentObject.position = cam.transform.position + cam.transform.forward * objectDistance;
                        currentObject.rotation = cam.transform.rotation;
                    }
                }
            } else
            {
                OnThrow.Invoke(this);
                currentObject.GetComponent<Rigidbody>().AddForce(cam.transform.forward * launchForce, ForceMode.Impulse);
                currentObject.GetComponent<Rigidbody>().AddTorque(currentObject.right * launchForce, ForceMode.Impulse);
                currentObject = null;
                objectDistance = 0;
            }
        }
    }

    protected void DoActionLadder()
    {
        ControlLadder();
        ControlCamera();
        UseObject();
    }

    private void ControlLadder()
    {
        if (isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {

                rb.AddForce(transform.forward * jumpHeight, ForceMode.Impulse);
                rb.AddForce(transform.forward * jumpHeight/2, ForceMode.Impulse);
            }
        }
        float x = Input.GetAxis("Horizontal") * velocity * Time.fixedDeltaTime;
        float z = Input.GetAxis("Vertical") * velocity * Time.fixedDeltaTime;

        Vector3 move = transform.right * x + transform.up * z;

        rb.MovePosition(rb.transform.position + move);
    }

    private void OnDestroy()
    {
        Ladder.EnterLadder -= Ladder_EnterLadder;

    }
}
