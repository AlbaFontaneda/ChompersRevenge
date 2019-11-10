using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeDirection : MonoBehaviour
{
    public Transform m_nextPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Chomper"))
        {
            //other.SendMessage("StartRotate");
            other.SendMessage("Look", m_nextPoint.position)
;        }
    }
}
