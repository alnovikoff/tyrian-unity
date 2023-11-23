using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{
    public float maxSpeed = 8;
    public float maxAccel = 25;
    public Vector3 velocity = Vector3.zero;

    private BoxCollider collider;
    public Transform targetTransform;

    public float minReactionDelay = 0.1f;
    public float maxReactionDelay = 0.2f;
    private float reactionDelay = 0.0f;

    private bool gameZoneEntered = false;

    public int NumLongToCooldown = 14;
    private int num_LongShots;

    public float firePointShiftZ = 15f;
    private Vector3 enemyPosition = Vector3.zero;

    public float ReloadSeconds = 0.3f;
    public float ReloadSecondsShort = 0.5f;
    private float reload = 0.0f;
    private float reload_short = 0.0f;
    public float CooldownSeconds = 10.0f;
    private float cooldown;
    [SerializeField] private Transform gun;
    public GameObject pref;

    public Projectile Projectile;
    bool superFlag = false;

    [SerializeField] public int currentHealth;
    [SerializeField] public int maxHealth;

    public Image helthBar;
    public TMP_Text healthTxt;

    float retreal_current_culldown = 0.0f;
    float retreal_culldown = 5.0f;

    enum State
    {
        ENTER_GAME_ZONE,
        LONG_ATTACK,
        SHORT_ATTACK,
        SUPER_ATTACK,
        RETREAT
    }
    private State activeState = State.ENTER_GAME_ZONE;

    void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = FindAnyObjectByType<PlayerBase>().transform;
        gun = transform.Find("Gun");
        cooldown = CooldownSeconds;

        currentHealth = maxHealth;
        helthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        healthTxt.text = currentHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        reactionDelay -= Time.deltaTime;
        if (reactionDelay <= 0.0f)
        {
            reactionDelay = Random.Range(minReactionDelay, maxReactionDelay);
            Perception();
            SelectState();
        }

        ProcessGunTimers();
        switch (activeState)
        {
            case State.ENTER_GAME_ZONE:
                Process_ENTER_GAME_ZONE();
                break;
            case State.SHORT_ATTACK:
                Process_SHORT_ATTACK();
                break;
            case State.LONG_ATTACK:
                Process_LONG_ATTACK();
                break;
            case State.SUPER_ATTACK:
                Process_SUPER_ATTACK();
                break;
            case State.RETREAT:
                Process_RETREAT();
                break;
            default: Debug.Assert(false); break;
        }

        helthBar.fillAmount = (float)currentHealth / (float)maxHealth;
        healthTxt.text = currentHealth.ToString();
    }

    private void Perception()
    {
        enemyPosition = targetTransform.position;
    }
    private void SelectState()
    {
        switch (activeState)
        {
            case State.ENTER_GAME_ZONE:
                if (gameZoneEntered)
                    activeState = State.LONG_ATTACK;
                break;
            case State.LONG_ATTACK:
                if (num_LongShots >= NumLongToCooldown)
                    activeState = State.SHORT_ATTACK;
                else if (SuperAttackProbability())
                    activeState = State.SUPER_ATTACK;
                break;
            case State.SHORT_ATTACK:
                if (num_LongShots < NumLongToCooldown && !SuperAttackProbability())
                    activeState = State.LONG_ATTACK;
                else if (SuperAttackProbability())
                {
                    superFlag = true;
                    activeState = State.SUPER_ATTACK;
                }
                break;
            case State.SUPER_ATTACK:
                    activeState = State.RETREAT;
                break;
            case State.RETREAT:
                if (retreal_current_culldown <= 0)
                    activeState = State.SHORT_ATTACK;
                break;
            default: Debug.Assert(false); break;
        }
    }
    private void Process_ENTER_GAME_ZONE()
    {
        EnvironmentProps env = EnvironmentProps.Instance;
        Vector3 target = new Vector3(
        0.5f * (env.minX + env.maxX),
        0.0f,
        env.minZ + 0.75f * (env.maxZ - env.minZ)
        );
        velocity = GameUtils.Instance.ComputeSeekVelocity(
        transform.position, velocity,
        maxSpeed, maxAccel,
        target, Time.deltaTime);
        transform.position = GameUtils.Instance.ComputeEulerStep(
        transform.position, velocity, Time.deltaTime);
        if ((target - transform.position).magnitude < 1.0f)
            gameZoneEntered = true;
    }
    private void Process_SHORT_ATTACK()
    {
        velocity = GameUtils.Instance.ComputeSeekVelocity(
            transform.position, velocity,
            maxSpeed, maxAccel,
            enemyPosition + new Vector3(0, 0, firePointShiftZ),
            Time.deltaTime
        );
        Vector3 pos = GameUtils.Instance.ComputeEulerStep(
        transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.IntoArea(
        pos, 0.5f * collider.size.x, 0.5f * collider.size.z);
        Shoot();
    }

    private void Process_LONG_ATTACK()
    {
        velocity = GameUtils.Instance.ComputeSeekVelocity(
            transform.position, velocity,
            maxSpeed, maxAccel,
            enemyPosition + new Vector3(0, 0, firePointShiftZ),
            Time.deltaTime
        );
        Vector3 pos = GameUtils.Instance.ComputeEulerStep(
        transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.IntoArea(
        pos, 0.5f * collider.size.x, 0.5f * collider.size.z);
        LongShoot();
    }

    private void Process_SUPER_ATTACK()
    {
        retreal_current_culldown = retreal_culldown;
        velocity = GameUtils.Instance.ComputeSeekVelocity(
            transform.position, velocity,
            maxSpeed, maxAccel,
            enemyPosition + new Vector3(0, 0, firePointShiftZ),
            Time.deltaTime
        );
        Vector3 pos = GameUtils.Instance.ComputeEulerStep(
        transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.IntoArea(
        pos, 0.5f * collider.size.x, 0.5f * collider.size.z);
        SuperShot();
    }
    private void Process_RETREAT()
    {
        EnvironmentProps env = EnvironmentProps.Instance;
        Vector3 target = new Vector3(
        env.minX +
        (enemyPosition.x < transform.position.x ? 0.8f : 0.2f) *
        (env.maxX - env.minX),
        0,
        env.minZ + 0.8f * (env.maxZ - env.minZ)
        );
        velocity = GameUtils.Instance.ComputeSeekVelocity(
        transform.position, velocity,
        maxSpeed, maxAccel,
        target, Time.deltaTime);
        Vector3 pos = GameUtils.Instance.ComputeEulerStep(
        transform.position, velocity, Time.deltaTime);
        transform.position = EnvironmentProps.Instance.IntoArea(
        pos, 0.5f * collider.size.x, 0.5f * collider.size.z);
    }

    private bool SuperAttackProbability()
    {
        float rnd = Random.Range(0, 101);
        Debug.Log(rnd);
        if (rnd < 5)
            return true;
        else
            return false;
    }
    
    private void ProcessGunTimers()
    {
        if (num_LongShots == NumLongToCooldown)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0.0f)
            {
                cooldown = CooldownSeconds;
                reload = 0.0f;
                num_LongShots = 0;
            }
        }
        else if (reload > 0.0f)
        reload -= Time.deltaTime;
        reload_short -= Time.deltaTime;
        retreal_current_culldown -= Time.deltaTime;
    }
    private void Shoot()
    {
        if (reload_short <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            Projectile.Instantiate(
                gun.position,
                horizontalVelocity,
                Matrix4x4.Rotate(gun.rotation).MultiplyVector(new Vector3(0, 0, 1)));
            reload_short = ReloadSecondsShort;
        }
    }

    private void LongShoot()
    {
        if (reload <= 0.0f)
        {
            Vector3 horizontalVelocity =
            Vector3.Dot(velocity, Vector3.right) * Vector3.right;
            Projectile.Instantiate(
                gun.position,
                horizontalVelocity,
                Matrix4x4.Rotate(gun.rotation).MultiplyVector(new Vector3(0, 0, 1)));
            ++num_LongShots;
            reload = ReloadSeconds;
            superFlag = true;
        }
    }

    private void SuperShot()
    {
        if (superFlag)
        {
            for (int i = 0; i < 8; i++)
            {
                float angle = i * Mathf.PI * 2 / 8;
                float x = Mathf.Cos(angle) * 3;
                float z = Mathf.Sin(angle) * 3;

                Vector3 position = transform.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Deg2Rad;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                Instantiate(pref, position, rot);
            }
            for (int i = 0; i < 6; i++)
            {
                float angle = i * Mathf.PI * 2 / 6;
                float x = Mathf.Cos(angle) * 4;
                float z = Mathf.Sin(angle) * 4;

                Vector3 position = transform.position + new Vector3(x, 0, z);
                float angleDegrees = -angle * Mathf.Deg2Rad;
                Quaternion rot = Quaternion.Euler(0, angleDegrees, 0);
                Instantiate(pref, position, rot);
            }
            superFlag = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (currentHealth > 0)
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion");
            currentHealth -= UIManager.Instance.playerBase.playerBulletDamage;
            Currencies.Instance.scoreHitsInt++;
            Currencies.Instance.credHitsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
            UIManager.Instance.RefreshCurrncies();

            helthBar.fillAmount = (float)currentHealth / (float)maxHealth;
            healthTxt.text = currentHealth.ToString();
            if (currentHealth <= 0)
            {
                GameManager.Instance.gameGloasCounter++;
                FindObjectOfType<AudioManager>().PlaySound("explosion1");
                Destroy(gameObject);
                Currencies.Instance.scoreKillsInt++;
                Currencies.Instance.credKillsInt += UIManager.Instance.playerBase.currentHealth * UIManager.Instance.playerBase.playerBulletDamage;
                UIManager.Instance.RefreshCurrncies();
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().PlaySound("explosion1");
            Destroy(gameObject);
            Time.timeScale = 0;
            UIManager.Instance.WinUI();
            GameManager.Instance.gameGloasCounter++;
        }
    }
}
