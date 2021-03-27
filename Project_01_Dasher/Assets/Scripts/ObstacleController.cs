using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    private ObstacleGroupController m_attachedGroup;

    // Start is called before the first frame update
    void Start()
    {
        m_attachedGroup = GetComponentInParent<ObstacleGroupController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidedGo = collision.gameObject;

        // Collide with player
        if (collidedGo.tag == "Player")
        {
            collidedGo.GetComponent<PlayerLivesController>().TakeDamage(transform);
            Destroy(m_attachedGroup.gameObject);
        }
    }
}
