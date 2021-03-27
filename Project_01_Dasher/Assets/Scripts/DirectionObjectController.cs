using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionObjectController : MonoBehaviour
{
    public Color m_readyColour;
    public Color m_cooldownColour;

    private SpriteRenderer m_spriteRenderer;
    private SpriteRenderer m_outlineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_spriteRenderer.color = m_readyColour;

        m_outlineRenderer = transform.Find("Outline").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoColourTransition(float timeLeft, float initialTime)
    {
        // Linearly interpolates between colors a and b by t.
        // t is clamped between 0 and 1.When t is 0 returns a. When t is 1 returns b.
        // Lerp(a, b, t);
        // t is interpolation value;

        float interpolate = timeLeft / initialTime;

        m_spriteRenderer.color = Color.Lerp(m_readyColour, m_cooldownColour, interpolate);
        m_outlineRenderer.enabled = (timeLeft < 0.01f);
    }

    public void ResetColour()
    {
        m_spriteRenderer.color = m_readyColour;
        m_outlineRenderer.enabled = true;
    }

    public void MoveToPosition(Vector3 playerPos, float offset)
    {
        // get mouse pos
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // get angle with atan2
        float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
        //Debug.Log((int)angle);

        // rotate pointer to face mouse
        transform.rotation = Quaternion.Euler(0, 0, angle - 90);

        // use angle and trigo to get new pos
        Vector3 newPos;
        newPos.z = -1;
        newPos.y = offset * Mathf.Sin(angle * Mathf.Deg2Rad);
        newPos.x = offset * Mathf.Cos(angle * Mathf.Deg2Rad);

        transform.position = newPos + playerPos;
    }
}
