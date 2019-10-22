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

    private ChomperAnimation chomperAnimation;
    private BoxCollider _boxCollider;
    private bool rotate;
    private float runTime;


    // Start is called before the first frame update
    void Start()
    {
        //El rigidbody se puede utilizar para rigidbody
        _boxCollider = GetComponent<BoxCollider>();
        chomperAnimation = GetComponent<ChomperAnimation>();
        rotate = false;
    }

    // Update is called once per frame
    void Update()
    {
        //CAMBIAMOS DE DIRECCIÓN CON UNA PROBABILIDAD DEL 5%
        if (Random.Range(0f, 1f) >= 0.95f && !rotate)
            StartCoroutine(Rotate(Random.Range(1, 10) % 2 == 0));
        float velocity = CalculateVelocity(Time.deltaTime);
        Debug.Log("Velocity " + velocity);
        chomperAnimation.Updatefordward(velocity / runSpeed);
        //La velocidad depende de si está corriendo o no
        //#TODO: Habra que meter gravedad

        //#TODO: Habra que comprobar colisiones.
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
}
