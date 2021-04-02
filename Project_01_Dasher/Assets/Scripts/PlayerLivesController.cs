using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLivesController : MonoBehaviour
{
    public int m_lives;
    public float m_damageImmunityTime;
    public float m_damageRecoilDist;

    private PlayerController m_player;

    public delegate void DamageTaken();
    public static event DamageTaken OnDamage;

    public delegate void PlayerDied();
    public static event PlayerDied OnPlayerDeath;

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
        --m_lives;
        if (m_lives <= 0)
        {
            OnPlayerDeath();
            return;
        }

        OnDamage();
        m_player.SetState(PlayerController.PLAYER_STATE.DAMAGED, m_damageImmunityTime);
        m_player.RecoilPlayer(otherGo, m_damageRecoilDist);
    }

}
