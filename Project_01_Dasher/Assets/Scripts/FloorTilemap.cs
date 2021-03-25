using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorTilemap : MonoBehaviour
{
    public List<Color> m_colors;
    private int m_iter;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.OnClicked += ColourSwap;
        m_iter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ColourSwap()
    {
        if (m_iter >= m_colors.Count) m_iter = 0;
        GetComponent<Tilemap>().color = m_colors[m_iter++];
    }
}
