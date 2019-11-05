using UnityEngine;
using System.Collections;

public class StickyFloor : MonoBehaviour {

	private Vector3 m_EnterScale = Vector3.one;
    public Transform m_globalParent = null; //Por defecto el padre global será la raiz de la escena pero podría ser que no fuera así.
    public Transform m_transformToAttach;
    public Quaternion rotation;

    void Start()
    {
        if (m_transformToAttach == null)
            m_transformToAttach = transform;
    }

    void OnTriggerEnter(Collider other)
    {
        //TODO 1: Cuando el objeto que caiga sea attachable, atachamos el objeto. Ojo, la scala puede cambiar!!!
        Attachable atachable = other.GetComponent<Attachable>();
        if (atachable && atachable.IsAttachable)
        {
            m_EnterScale = other.transform.localScale;
            rotation = other.transform.localRotation;
            other.transform.parent = m_transformToAttach;
            atachable.IsAttached = true;
        }

    }

    void OnTriggerExit(Collider other)
    {
        //TODO 2: Cuando el objeto que caiga sea attachable, como estamos saliendo, desatachamos el objeto. Ojo, la scala puede cambiar!!!
        Attachable atachable = other.GetComponent<Attachable>();
        DesAttached(atachable, other);
    }

    public void DesAttached(Attachable atachable, Collider other)
    {
        if (atachable && atachable.IsAttached)
        {
            other.transform.parent = m_globalParent;
            other.transform.localScale = m_EnterScale;
            atachable.IsAttached = false;
            other.transform.localRotation = rotation;
        }
    }
}
