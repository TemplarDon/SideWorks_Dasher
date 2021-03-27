using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_normalSpeed;
    public float m_dashSpeed;
    public float m_maxSpeed;
    public float m_dashCooldown;
    public float m_dashStopSpeed;

    public float m_directionObjectOffset;

    public List<KeyCode> m_keys;
    public DirectionObjectController m_directionObject;

    public enum PLAYER_STATE
    {
        NORMAL,
        DASHING,
        DAMAGED,
        DEAD,
    }
    [SerializeField] private PLAYER_STATE m_currState; // player state
    private float m_timer; // general use timer

    private Rigidbody2D m_rigidbody;
    private PlayerLivesController m_healthController;
    private PlayerSpriteController m_spriteController;

    // Start is called before the first frame update
    void Start()
    {
        m_currState = PLAYER_STATE.NORMAL;
        m_timer = 0.0f;

        m_rigidbody = GetComponent<Rigidbody2D>();

       if (!m_directionObject) m_directionObject = transform.Find("DirectionObject").GetComponent<DirectionObjectController>();

        m_healthController = GetComponent<PlayerLivesController>();
        m_spriteController = GetComponent<PlayerSpriteController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer > 0) TimerUpdate(); 

        switch (m_currState)
        {
            case PLAYER_STATE.NORMAL:
                SpeedCheck();
                MovementUpdate();

                m_directionObject.MoveToPosition(transform.position, m_directionObjectOffset);
                if (m_timer > 0) m_directionObject.DoColourTransition(m_timer, m_dashCooldown);
                break;
            case PLAYER_STATE.DASHING:
                if (m_timer > 0) m_directionObject.DoColourTransition(m_timer, m_dashCooldown);

                // Check if player can start moving while dash is still on cooldown
                if (m_rigidbody.velocity.sqrMagnitude <= m_dashStopSpeed * m_dashStopSpeed)
                    SetState(PLAYER_STATE.NORMAL); 
                break;
            case PLAYER_STATE.DAMAGED:
                // get sprite controller to play animation
                m_spriteController.PlayAnim();
                break;
            case PLAYER_STATE.DEAD: break;
            default: break;
        }
    }

    void MovementUpdate()
    {
        foreach (KeyCode kc in m_keys)
        {
            // For held down keys
            if (Input.GetKey(kc))
            {
                switch (kc)
                {
                    case KeyCode.W: 
                        m_rigidbody.AddForce(new Vector2(0, m_normalSpeed), ForceMode2D.Force); 
                        break;
                    case KeyCode.A: 
                        m_rigidbody.AddForce(new Vector2(-m_normalSpeed, 0), ForceMode2D.Force); 
                        break; 
                    case KeyCode.S: 
                        m_rigidbody.AddForce(new Vector2(0, -m_normalSpeed), ForceMode2D.Force); 
                        break; 
                    case KeyCode.D: 
                        m_rigidbody.AddForce(new Vector2(m_normalSpeed, 0), ForceMode2D.Force); 
                        break; 

                    default: break;
                }
            }

            // For one-time press keys
            if (Input.GetKeyDown(kc))
            {
                switch (kc)
                {
                    case KeyCode.Space:
                        // Stop the player from dashing if the timer is still counting down
                        if (m_timer > 0) break;

                        SetState(PLAYER_STATE.DASHING);
                        m_timer = m_dashCooldown;
                        m_rigidbody.velocity = Vector2.zero;

                        Vector3 dirVec;
                        dirVec = (m_directionObject.gameObject.transform.position - transform.position);
                        dirVec.z = 0;
                        m_rigidbody.AddForce(dirVec * m_dashSpeed, ForceMode2D.Impulse);
                        break;

                    default: break;
                }
            }

            // Key released
            if (Input.GetKeyUp(kc))
            {
                if ((kc == KeyCode.W ||
                    kc == KeyCode.A ||
                    kc == KeyCode.S ||
                    kc == KeyCode.D ) && m_currState == PLAYER_STATE.NORMAL) 
                {
                    m_rigidbody.velocity = Vector2.zero;
                    //m_rigidbody.AddForce(-m_rigidbody.velocity);
                }
            }
        }

    }

    void SpeedCheck()
    {
        if (m_rigidbody.velocity.sqrMagnitude > m_maxSpeed * m_maxSpeed)
        {
            m_rigidbody.velocity = m_rigidbody.velocity.normalized * m_maxSpeed;
        }
    }

    // Updates timer and changes state based on current state
    void TimerUpdate()
    {
        m_timer -= Time.deltaTime;

        if (m_timer <= 0)
        {
            switch (m_currState)
            {
                case PLAYER_STATE.NORMAL: break;
                case PLAYER_STATE.DASHING:
                    break;
                case PLAYER_STATE.DAMAGED:
                    SetState(PLAYER_STATE.NORMAL);
                    break;
                case PLAYER_STATE.DEAD: break;
            }
            m_timer = 0;
        }
    }

    public void SetState(PLAYER_STATE ps, float timer = 0)
    {
        //Debug.Log("Player State set -> " + ps.ToString() + " Timer set -> " + timer);

        if (timer != 0) m_timer = timer;
        m_currState = ps;

        m_spriteController.ChangeSprite(ps);

        // Set the direction to ready colour
        if (timer != 0) m_directionObject.ResetColour();
    }

    public void RecoilPlayer(Transform otherGo, float recoilStrength = 1)
    {
        Vector3 dirVec = (transform.position - otherGo.position).normalized;
        m_rigidbody.velocity = dirVec * recoilStrength;
    }
}
