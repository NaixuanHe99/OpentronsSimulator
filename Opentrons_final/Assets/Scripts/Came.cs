using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Came : MonoBehaviour
{
    // around the rotating target

    public Transform target;
    public Transform[] targets;

    // set rotation angle

    public float x = 0f, y = 0f, z = 0f;

    public bool xFlag = false, yFlag = false;

    // rotation speed

    public float xSpeed = 10f, ySpeed = 10f, mSpeed = 5f;

    // y axis limit

    public float yMinLimit = -50, yMaxLimit = 80;

    // x axis limit

    public float leftMax = -365, rightMax = 365;

    // distance limit

    public float distance = 3f, minDistance = 0.5f, maxDistance = 6f;

    // damping

    public bool needDamping = true;

    public float damping = 3f;

    public float initX;

    public float initY;

    // change the target

    public void SetTarget(GameObject go)

    {

        target = go.transform;

    }

    void Start()

    {
        Vector3 angles = transform.eulerAngles;

        x = angles.y;

        y = angles.x;



        //pers();

    }

    void LateUpdate()

    {
        if (target)

        {

            if (Time.timeScale != 0)
            {
                if (Input.GetMouseButton(1))

                {

                    // judging whether flipping is needed

                    if ((y > 90f && y < 270f) || (y < -90 && y > -270f))

                    {

                        x -= Input.GetAxis("Mouse X") * xSpeed * 0.02f;

                    }

                    else

                    {

                        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;

                    }

                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;



                    x = ClampAngle(x, leftMax, rightMax);

                    y = ClampAngle(y, yMinLimit, yMaxLimit);

                }



                if (!EventSystem.current.IsPointerOverGameObject())
                    distance -= Input.GetAxis("Mouse ScrollWheel") * mSpeed;

                distance = Mathf.Clamp(distance, minDistance, maxDistance);

            }


            Quaternion rotation = Quaternion.Euler(y, x, z);

            Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);

            Vector3 position = rotation * disVector + target.position;



            // damping

            if (needDamping)

            {

                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);

                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);

            }

            else

            {

                transform.rotation = rotation;

                transform.position = position;

            }



        }

    }

    // adjusting the value

    static float ClampAngle(float angle, float min, float max)

    {

        if (angle < -360)

            angle += 360;

        if (angle > 360)

            angle -= 360;

        return Mathf.Clamp(angle, min, max);

    }

    // init

    public void pers()

    {

        this.x = initX;

        this.y = initY;

    }


    public void front()

    {

        this.x = 0f;

        this.y = 0f;

    }


    public void back()

    {

        this.x = 180f;

        this.y = 0f;

    }


    public void left()

    {

        this.x = 90f;

        this.y = 0f;

    }


    public void right()

    {

        this.x = 270f;

        this.y = 0f;

    }


    public void top()

    {

        this.x = 0f;

        this.y = 90f;

    }


    public void bottom()

    {

        this.x = 0f;

        this.y = -90f;

    }
}
