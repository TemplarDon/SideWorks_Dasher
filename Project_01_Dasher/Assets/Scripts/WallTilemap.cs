using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTilemap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.name + " hit the wall");

        if (collision.gameObject.GetComponent<PlayerController>())
        {
            collision.gameObject.GetComponent<PlayerController>().SetState(PlayerController.PLAYER_STATE.NORMAL);
            collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
