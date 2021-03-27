using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObstacleController : MonoBehaviour
{
    public float m_shakeVariable;
    public float m_shakeInterval;

    private ObstacleGroupController m_attachedGroup;
    private Vector3 m_origPos;
    private float m_timer;

    // Start is called before the first frame update
    void Start()
    {
        m_attachedGroup = GetComponentInParent<ObstacleGroupController>();
        m_origPos = transform.localPosition;
        m_timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_timer >= m_shakeInterval)
        {
            Shake();
            m_timer = 0;
        }
        else
            m_timer += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject collidedGo = col.gameObject;

        // Collide with player
        if (collidedGo.tag == "Player")
        {
            if (!(collidedGo.GetComponent<PlayerController>().GetState() == PlayerController.PLAYER_STATE.DASHING))
            {
                collidedGo.GetComponent<PlayerLivesController>().TakeDamage(transform);
            }
            Destroy(m_attachedGroup.gameObject);
        }

    }

    void Shake()
    {
        Vector3 newPos;
        newPos.z = 0;

        newPos.x = Random.Range(m_origPos.x - m_shakeVariable, m_origPos.x + m_shakeVariable);
        newPos.y = Random.Range(m_origPos.y - m_shakeVariable, m_origPos.y + m_shakeVariable);

        transform.localPosition = newPos;
    }
}
