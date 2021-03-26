using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; // for [Serializable]

[Serializable] public struct PlayerStateSprite
{
    public PlayerController.PLAYER_STATE state;
    public Sprite sprite;
}


public class PlayerSpriteController : MonoBehaviour
{
    public List<PlayerStateSprite> m_playerSpriteList;

    private Dictionary<PlayerController.PLAYER_STATE, Sprite> m_playerSpriteMap;
    private SpriteRenderer m_spriteRenderer;
    private Animation m_animation;

    // Start is called before the first frame update
    void Start()
    {
        m_playerSpriteMap = new Dictionary<PlayerController.PLAYER_STATE, Sprite>();
        foreach (PlayerStateSprite pss in m_playerSpriteList)
        {
            m_playerSpriteMap.Add(pss.state, pss.sprite);
        }

        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_animation = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeSprite(PlayerController.PLAYER_STATE newState)
    {
        Sprite newSprite;
        if (m_playerSpriteMap.TryGetValue(newState, out newSprite))
            m_spriteRenderer.sprite = newSprite;
    }

    public void PlayAnim()
    {
        if (!m_animation.isPlaying)
            m_animation.Play("PlayerDamage");
    }
}
