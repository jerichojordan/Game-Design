using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class Player : MonoBehaviour, IHittable
{
    [Header("Stats")]
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _weaponRange = 10f;
    [SerializeField] public float HitPoint = 100f;
    [SerializeField] private float maxAmmo = 30f;
    [SerializeField] private float reloadTime = 1.5f;
    [SerializeField] private float shootInterval = 0.2f;
    [Header("Audio")]
    [SerializeField] private AudioClip _finishReloadSound;
    [SerializeField] private AudioClip scream;
    [SerializeField] private AudioClip dead;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private AudioClip _gunShot;
    [Header("Used Asset")]
    [SerializeField] private GameObject _hitBlood;
    [SerializeField] private GameObject Flashlight;
    [SerializeField] private GameObject _CrosshairSprite;
    [SerializeField] private GameObject _bulletTrail;
    [SerializeField] private Transform _gunPoint;


    public Animator animator;
    public float location;
    public float currentAmmo;

    private float Damage = 35f;
    private float nextFireTime;
    private Rigidbody2D _rigidbody;
    private bool isReloading;
    private LevelFinish levelFinish;
    private bool isSprinting;
    private bool onFlashlight;
    private float currentspeed;
    GameObject _Crosshair;


    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        var crossPos = _gunPoint.position;
        _Crosshair = Instantiate(
               _CrosshairSprite,
               crossPos,
               transform.rotation
               );
        isReloading = false;
        _hitBlood.gameObject.SetActive(false);
        currentAmmo = maxAmmo;
        onFlashlight = true;
        levelFinish = GameObject.FindGameObjectWithTag("Finish").GetComponent<LevelFinish>();
        Flashlight = this.transform.Find("Light 2D").gameObject;
    }

    
    void Update()
    {
        CrosshairPositioning(_Crosshair);
        LookAtMouse();
        Move();
        if (currentAmmo > 0 && !isReloading)
        {
            Shoot();
        }else if (currentAmmo <= 0 && !isReloading) {
            Reload();
        }
        if(Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo) {
            Reload();
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
        if(Input.GetKeyDown(KeyCode.F))
        {
            flashlight();
        }
    }

    //Function too make the player looking at the mouse cursor
    private void LookAtMouse()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        transform.right =  mousePos - new Vector2(transform.position.x, transform.position.y);
    }

    private void Move()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (isSprinting) currentspeed = (5+ _speed);
        else currentspeed = _speed;
        _rigidbody.velocity = input.normalized * currentspeed;
        animator.SetFloat("speed", Mathf.Abs(_rigidbody.velocity[0]) + Mathf.Abs(_rigidbody.velocity[1]));
    }

    private void Shoot()
    {
        
        if(Input.GetMouseButton(0)&&isSprinting==false&&Time.time >= nextFireTime)
        {
            
            
            AudioSource.PlayClipAtPoint(_gunShot, transform.position);
            animator.SetTrigger("Shoot");
            var hit = Physics2D.Raycast(
                _gunPoint.position,
                transform.right,
                _weaponRange
                );

            var trail = Instantiate(
                _bulletTrail,
                _gunPoint.position,
                transform.rotation
                );

            var trailScript = trail.GetComponent<BulletTrail>();
            if(hit.collider != null)
            {
                trailScript.setTargetPosition(hit.point);
                var hittable = hit.collider.GetComponent<IHittable>();
                if( hittable != null )
                {
                    hittable.RecieveHit(hit, Damage);
                }
            }
            else
            {
                var endPosition = _gunPoint.position + transform.right * _weaponRange;
                trailScript.setTargetPosition(endPosition);
            }
            currentAmmo--;
            nextFireTime = Time.time + shootInterval;
        }
    }
    private void CrosshairPositioning(GameObject Crosshair)
    {
        var hit = Physics2D.Raycast(
               _gunPoint.position,
               transform.right,
               10f
               );
        if (hit.collider != null)
        {
            Crosshair.transform.position = hit.point;
        }
        else
        {
            var pos = _gunPoint.position + transform.right*15f;
            Crosshair.transform.position = pos;
        }
    }

    private void GetHit(RaycastHit2D hit, float damage)
    {
        HitPoint -= damage;
        if (HitPoint <= 0)
        {
            AudioSource.PlayClipAtPoint(dead, this.gameObject.transform.position);
            levelFinish.GameOver();
            gameObject.SetActive(false);
        }
        else
        {
            AudioSource.PlayClipAtPoint(scream, this.gameObject.transform.position);
            _hitBlood.gameObject.SetActive(true);
            Invoke("deactivateBlood", 0.2f);
        }
    }
    public void RecieveHit(RaycastHit2D hit, float damage)
    {
        GetHit(hit, damage);
    }
    private void Reload()
    {
        if (isReloading) return; // Ignore if already reloading
        AudioSource.PlayClipAtPoint(_reloadSound, transform.position);
        isReloading = true;
        Debug.Log("Reloading...");

        // Perform the reload logic
        // For example, you can play a reload animation or wait for a certain amount of time

        // After the reload time has passed
        Invoke("FinishReload", reloadTime);
    }

    private void FinishReload()
    {
        currentAmmo = maxAmmo;
        isReloading = false;
        AudioSource.PlayClipAtPoint(_finishReloadSound, transform.position);
        Debug.Log("Reload complete!");
    }

    private void flashlight()
    {
        if (onFlashlight)
        {
            Flashlight.SetActive(false);
            onFlashlight = false;
        }
        else
        {
            Flashlight.SetActive(true);
            onFlashlight = true;
        }
    }
    private void deactivateBlood()
    {
        _hitBlood.gameObject.SetActive(false);
    }
}

