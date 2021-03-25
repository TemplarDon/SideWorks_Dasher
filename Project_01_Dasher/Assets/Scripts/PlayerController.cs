using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_normalSpeed;
    public float m_dashSpeed;
    public float m_maxSpeed;
    public float m_dashTime;

    public float m_directionObjectOffset;

    public List<KeyCode> m_keys;
    public Transform m_directionObject;

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

    // Start is called before the first frame update
    void Start()
    {
        m_currState = PLAYER_STATE.NORMAL;
        m_timer = 0.0f;

        m_rigidbody = GetComponent<Rigidbody2D>();

       if (!m_directionObject) m_directionObject = transform.Find("DirectionObject");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer > 0) TimerUpdate(); 

        switch (m_currState)
        {
            case PLAYER_STATE.NORMAL:
                SpeedCheck();
                DirectionUpdate();
                MovementUpdate();
                break;
            case PLAYER_STATE.DASHING: break;
            case PLAYER_STATE.DAMAGED: break;
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
                        m_currState = PLAYER_STATE.DASHING;
                        m_timer = m_dashTime;
                        m_rigidbody.velocity = Vector2.zero;

                        Vector3 dirVec;
                        dirVec = (m_directionObject.position - transform.position);
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
                    m_currState = PLAYER_STATE.NORMAL;
                    break;
                case PLAYER_STATE.DAMAGED: break;
                case PLAYER_STATE.DEAD: break;
            }
            m_timer = 0;
        }
    }

    // Updates direction object to face mouse at an offset
    void DirectionUpdate()
    {
        // get mouse pos
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get angle with atan2
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;

        // use angle and trigo to get new pos
        Vector3 newPos;
        newPos.z = -1;
        newPos.y = m_directionObjectOffset * Mathf.Sin(angle * Mathf.Deg2Rad);
        newPos.x = m_directionObjectOffset * Mathf.Cos(angle * Mathf.Deg2Rad);

        m_directionObject.position = newPos + transform.position;
    }

    public void SetState(PLAYER_STATE ps, float timer = 0)
    {
        //Debug.Log("Player State set -> " + ps.ToString() + " Timer set -> " + timer);

        m_timer = timer;
        m_currState = ps;
    }
}
