using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LivesController : MonoBehaviour
{
    public PlayerLivesController m_playerLivesController;
    public GameObject m_livesUIElement;
    public float m_spacing;

    private List<GameObject> m_livesUIList;

    // Start is called before the first frame update
    void Start()
    {
        if (!m_playerLivesController)
            m_playerLivesController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLivesController>();

        if (m_livesUIElement)
        {
            m_livesUIList = new List<GameObject>();
            m_livesUIList.Add(m_livesUIElement);
            for (int i = 1; i < m_playerLivesController.m_lives; ++i)
            {
                Vector2 offset = new Vector2(m_livesUIElement.transform.GetComponent<RectTransform>().anchoredPosition.x + (i * m_spacing), 
                                             m_livesUIElement.transform.GetComponent<RectTransform>().anchoredPosition.y);

                GameObject newGo = Instantiate(m_livesUIElement, m_livesUIElement.transform.position, Quaternion.identity, transform);
                newGo.transform.GetComponent<RectTransform>().anchoredPosition = offset;

                m_livesUIList.Add(newGo);
            }
        }

        PlayerLivesController.OnDamage += RemoveLife;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RemoveLife()
    {
        Destroy(m_livesUIList[m_playerLivesController.m_lives]);
    }
}
