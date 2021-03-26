using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform m_objectToFollow;
    public float m_followSpeed;
    public float m_followDist;
    public float m_cameraShakeAmount;
    public float m_cameraShakeSpeed;

    private bool m_isCameraShaking;
    private float m_cameraShakeEnd;
    private float m_origCameraSize;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_objectToFollow) m_objectToFollow = transform.Find("Player");

        m_isCameraShaking = false;
        m_origCameraSize = GetComponent<Camera>().orthographicSize;

        EventManager.OnClicked += CameraShake;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, m_objectToFollow.position) < m_followDist)
        {
            Vector3 dirVec = (m_objectToFollow.position - transform.position).normalized;
            dirVec.z = 0;
            transform.Translate(dirVec * m_followSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 newPos = m_objectToFollow.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        // Camera Shake
        if (m_isCameraShaking)
        {
            if (GetComponent<Camera>().orthographicSize > m_cameraShakeEnd)
            {
                GetComponent<Camera>().orthographicSize -= m_cameraShakeSpeed * Time.deltaTime;
            }
            else
            {
                m_cameraShakeEnd = m_origCameraSize; // set this to prevent it from re-entering the code above and make infinite loop
                GetComponent<Camera>().orthographicSize += m_cameraShakeSpeed * Time.deltaTime;
                if (GetComponent<Camera>().orthographicSize >= m_origCameraSize)
                {
                    m_isCameraShaking = false;
                    GetComponent<Camera>().orthographicSize = m_origCameraSize;

                    //Debug.Log("Camera Shake ends");
                }
            }  
        }
    }

    void CameraShake()
    {
        // if (m_isCameraShaking) return;

        m_isCameraShaking = true;
        m_cameraShakeEnd = m_origCameraSize - m_cameraShakeAmount;
    }
}
