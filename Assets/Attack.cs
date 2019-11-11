using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject m_Enemy = null;
    public BoxCollider m_Player = null;
    private GameObject m_GameManager = null;

    private BoxCollider m_chomperCollider = null;
    private bool isRespawn = false;

    void Start()
    {

        // TODO 1 - Buscamos un GameObject cuyo tag sea "GameManager"
        m_GameManager = GameObject.FindGameObjectWithTag("GameManager");
        m_chomperCollider = m_Enemy.GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == m_Enemy)
        {
          if (m_Player.bounds.center.y > m_chomperCollider.bounds.center.y && !isRespawn)
            {
                m_Enemy.SendMessage("Death");
                Debug.Log(m_Player.bounds.min);
                Debug.Log(m_Enemy.GetComponent<BoxCollider>().bounds.max);
            }

            else if (!isRespawn)
            {
                isRespawn = true;
                m_GameManager.SendMessage("RespawnPlayer");
            }

            else if (isRespawn)
                isRespawn = !isRespawn;
        }
    }
}
