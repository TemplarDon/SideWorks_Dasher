using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGroupController : MonoBehaviour
{
    public Vector2 m_dir;
    public float m_speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move obstacle in dir
        transform.Translate(m_dir.normalized * m_speed * Time.deltaTime, Space.World);
    }
}
