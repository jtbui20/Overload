using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    [Header("Player Properties")]
    public float speed;
    public float jumpStrength;
    public Ability PrimaryAction;
    public Ability SecondaryAction;

    [Header("ColliderCheck")]
    public float wallDistance;
    public float groundDistance;
    public float checkRadius;
    public LayerMask checkLayer;

    [Header("HitCheck")]
    public float hitDistance;
    public float dashDistance;
    public LayerMask hitLayers;

    [Header("Ownership")]
    public GameObject projectile;
    public float shootOffset;
    public float shootStrength;

    [Header("Debug Bools")]
    public bool isJump = false;
    public bool isGrounded = false;
    public bool isPrimary = false;
    public bool isSecondary = false;
    public bool isRandom = false;
    public bool isLock = false;
    private Rigidbody2D _rb;

    public float HorizontalVelocity;
    public Vector3 mousePos;
    public Vector2 motion;
    Camera _camera;
    GameController _gm;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        GravityAndJump();
        CooldownTick();
        WallAndGroundCheck();
        Move();
        Shoot();
    }

    void HandleInput()
    {
        if (!isLock)
        {
            isJump = Input.GetButtonDown("Jump");
            HorizontalVelocity = Input.GetAxis("Horizontal") * speed;
            motion = new Vector2(HorizontalVelocity * Time.deltaTime, 0);
        } else
        {
            motion = new Vector2(0, 0);
        }
        
        isPrimary = Input.GetButton("Fire1");
        isSecondary = Input.GetButton("Fire2");
        isRandom = Input.GetKeyDown(KeyCode.LeftControl);

        Vector3 position = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos = new Vector2(position.x, position.y);
    }

    void GravityAndJump()
    {
        if (isJump && isGrounded)
        {
            _rb.AddForce(new Vector3(0, jumpStrength, 0), ForceMode2D.Impulse);
        }
    }

    void WallAndGroundCheck()
    {
        Vector3 _motion3 = new Vector3(motion.x, motion.y, 0).normalized * wallDistance;
        Collider2D[] wallHit = Physics2D.OverlapCircleAll(transform.position + _motion3, checkRadius, checkLayer);
        if (wallHit.Length > 0) motion.x = 0;

        Vector3 _ground = new Vector3(0, -groundDistance, 0);
        Collider2D[] groundHit = Physics2D.OverlapCircleAll(transform.position + _ground, checkRadius, checkLayer);
        isGrounded = groundHit.Length > 0;
    }

    void CooldownTick()
    {
        if (PrimaryAction != null)
        {
            PrimaryAction.Tick();
        }
        if (SecondaryAction != null)
        {
            SecondaryAction.Tick();
        }
    }

    public void ToggleInputLock()
    {
        isLock = !isLock;
    }

    void Move()
    {
        transform.position += new Vector3(motion.x, motion.y, 0);
    }

    void Shoot()
    {
        if (isPrimary && (PrimaryAction != null))
        {
            PrimaryAction.Do();
        }
        else if (isSecondary && (SecondaryAction != null))
        {
            SecondaryAction.Do();
        }
    }

    public void RemoveAbility(Type ability)
    {
        if (PrimaryAction != null)
        {
            if (PrimaryAction.GetType() == ability)
            {
                Destroy(PrimaryAction);
                PrimaryAction = null;
            }
        }
            
        if (SecondaryAction != null)
        {
            if (SecondaryAction.GetType() == ability)
            {
                Destroy(SecondaryAction);
                SecondaryAction = null;
            }
        }  

        if (ability.Name == "FBomb")
        {
            Debug.Log("hi?");
            Destroy(GetComponent<FBomb>());
        }
    }

    public void FBomb()
    {
        _gm.MemBar.DeleteAllMemoryByName("Virus");
        _gm.MemBar.DeleteAllMemoryByName("Bloat");
    }
}
