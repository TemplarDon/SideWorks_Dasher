using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesController : MonoBehaviour
{
    public int m_lives;
    public float m_damageImmunityTime;
    public float m_damageRecoilDist;

    private PlayerController m_player;

    // Start is called before the first frame update
    void Start()
    {
        m_player = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(Transform otherGo)
    {
        m_lives--;
        m_player.SetState(PlayerController.PLAYER_STATE.DAMAGED, m_damageImmunityTime);
        m_player.RecoilPlayer(otherGo, m_damageRecoilDist);
    }

}
