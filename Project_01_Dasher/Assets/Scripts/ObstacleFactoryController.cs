using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactoryController : MonoBehaviour
{
    public Vector2 m_dir;

    private bool m_isReadyToSpawn;
    private GameObject m_spawnObject;

    // Start is called before the first frame update
    void Start()
    {
        m_isReadyToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_spawnObject)
            m_isReadyToSpawn = false;
        else
            m_isReadyToSpawn = true;
    }
    
    public void SpawnObstacle(GameObject toSpawn)
    {
        Quaternion spawnRotate = Quaternion.identity;
        // rotata object based on m_dir
        if (m_dir.x == -1)
        {
            spawnRotate = Quaternion.Euler(new Vector3(0, 0, -90));
        }
        else if (m_dir.x == 1)
        {
            spawnRotate = Quaternion.Euler(new Vector3(0, 0, 90));
        }
        else if (m_dir.y == 1)
        {
            spawnRotate = Quaternion.Euler(new Vector3(0, 0, 180));
        }

        m_spawnObject = Instantiate(toSpawn, transform.position, spawnRotate);
        m_spawnObject.GetComponent<ObstacleGroupController>().m_dir = m_dir;

        m_isReadyToSpawn = false;
    }

    public bool CanSpawn()
    {
        return m_isReadyToSpawn;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        GameObject collidedGo = col.gameObject;

        // Collide with obstacle
        if (collidedGo.tag == "Obstacle")
        {
            if (collidedGo.GetComponentInParent<ObstacleGroupController>().m_dir != m_dir)
            {
                Destroy(col.transform.parent);
            }
        }

    }
}
