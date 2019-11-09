using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperController : MonoBehaviour
{
    public bool run;
    public float speed;
    public float runSpeed;
    public float angle;
    public float rotationTime;
    public float timeToAccel;

    private Rigidbody chomperRigidBody;
    private ChomperAnimation chomperAnimation;
    private Animator _animator;
    private BoxCollider _boxCollider;
    private bool rotate;
    private float runTime;


    private GameObject m_target;
    public List<Vector3> m_waypointList;
    private int m_currentWaypoint;
    private float m_distanceToChangeWaypoint = 2.0f;
    private enum TState { MOVE, ATTACK, DEATH, INIT_SEARCH, FOLLOW_PATH };
    private TState m_state = TState.MOVE;
    private Vector3 m_direction = Vector3.zero;
    public float m_stopDistance = 6f;

    // Start is called before the first frame update
    void Start()
    {
        //El rigidbody se puede utilizar para rigidbody
        chomperRigidBody = GetComponent<Rigidbody>();
        chomperAnimation = GetComponent<ChomperAnimation>();
        _boxCollider = GetComponent<BoxCollider>();
        _animator = GetComponent<Animator>();
        rotate = false;

        chomperRigidBody.isKinematic = false;
        _animator.applyRootMotion = false;

        m_waypointList = new List<Vector3>();
        m_target = GameObject.FindGameObjectWithTag("Player") as GameObject;

    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawRay(transform.position, transform.forward * 5f, Color.red);

     
        if (m_state == TState.MOVE)
        {
            //Look(m_target.transform.position);
            Move();
        }

        //CAMBIAMOS DE DIRECCIÓN CON UNA PROBABILIDAD DEL 5%
        //if (Random.Range(0f, 1f) >= 0.95f && !rotate)
        //StartCoroutine(Rotate(Random.Range(1, 10) % 2 == 0));
        //float velocity = CalculateVelocity(Time.deltaTime);

        //chomperAnimation.Updatefordward(velocity / runSpeed);

        //#TODO: Habra que comprobar colisiones.
        //if (!Physics.BoxCast(transform.position, _boxCollider.size, transform.forward, transform.rotation, 0.1f))
        //{
        //transform.position += transform.forward * velocity * Time.deltaTime;
        //}
    }


    protected void Look(Vector3 newPosition)
    {
        m_direction = newPosition;
        m_direction.y = transform.position.y;
        this.transform.LookAt(m_direction);
    }

    protected void Common()
    {
        RaycastHit hit;
        Vector3 direction = m_target.transform.position - this.transform.position;

        Vector3 center = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        Debug.DrawRay(center, direction * 5f, Color.green);
        bool targetVisible = false;
        if (Physics.Raycast(center, direction, out hit))
        {
            //Si te veo
            if (hit.collider.gameObject == m_target)
            {
                targetVisible = true;
                m_direction = direction;
                //Si no te tengo a tiro
                if (m_direction.sqrMagnitude > m_stopDistance * m_stopDistance)
                {
                    m_state = TState.MOVE;
                }
                else
                {
                    //Si te tengo a tiro...
                    m_state = TState.ATTACK;
                    //m_attackTime = m_timeBetweenAttack;
                    //if (HasAttackAnim)
                        //m_animationcomponent.IsAttacking = true;
                }
            }
        }

        if (!targetVisible)
        {
            //Si estaba buscando directamente y le pierdo de vista, usa A*
            if (m_state == TState.MOVE || m_state == TState.ATTACK)
                m_state = TState.INIT_SEARCH;
        }
    }

    protected void Move()
    {
       
            float velocity = CalculateVelocity(Time.deltaTime);

                chomperAnimation.Updatefordward(velocity / runSpeed);
        

        if (!Physics.BoxCast(transform.position, _boxCollider.size, transform.forward, transform.rotation, 0.1f))
            {
            transform.position += transform.forward * velocity * Time.deltaTime;
        }
 

    }


    private float CalculateVelocity(float time)
    {
        if (run)
        {
            runTime += time;
            if (runTime > timeToAccel)
                runTime = timeToAccel;
            return Mathf.Lerp(speed, runSpeed, runTime / timeToAccel);
        }
        else
        {
            runTime -= time;
            if (runTime < 0)
                runTime = 0f;
            return Mathf.Lerp(speed, runSpeed, runTime / timeToAccel);

        }
    }

    IEnumerator Rotate(bool inverse)
    {
        float time = 0;
        rotate = true;
        float realAngle = inverse ? -angle : angle;
        //Podemos rotar a izquierda o a derecha.
        Quaternion newRotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.up * realAngle);
        Quaternion originalRotation = transform.rotation;
        while (time < rotationTime)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(originalRotation, newRotation, time / rotationTime);
            yield return new WaitForEndOfFrame();
        }
        rotate = false;
    }

    private void StartRotate()
    {
        StartCoroutine(Rotate(true));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            chomperRigidBody.isKinematic = true;
            _animator.applyRootMotion = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            chomperRigidBody.isKinematic = true;
            _animator.applyRootMotion = true;
        }
            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            chomperRigidBody.isKinematic = false;
            _animator.applyRootMotion = false;
        }
            
            
    }


}
