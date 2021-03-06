using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public List<Cube> cubes;

    public float multiplier = 0.5f;
    public float platformHeight;
    public float radius;
    public float initialHeight;

    int currentTarget = -1;
    // Start is called before the first frame update
    void Start()
    {
        SetupPlanets();
    }

    void SetupPlanets()
    {
        var separationAngle = 180 / (cubes.Count - 1);
        var n = 0;
        foreach (Cube cube in cubes)
        {
            Vector3 p = transform.position;
            p.x += 0.5f * multiplier;
            if (n % 2 == 0)
            {
                p.y += 0.1f * multiplier;
            }
            else
            {
                p.y += 0.2f * multiplier;
            }


            cube.transform.position = p;
            cube.transform.RotateAround(transform.position, Vector3.up, -separationAngle * n);
            n += 1;
            cube.blurredPosition = cube.transform.position;
            cube.currentPosition = cube.transform.position;
            cube.blurredScale = cube.transform.localScale;
            cube.currentScale = cube.transform.localScale;
        }
    }
    public void focus(int index)
    {
        if (index == currentTarget || cubes[index].resizing)
        {
            return;
        }
        else if (currentTarget >= 0)
        {
            cubes[currentTarget].currentPosition = cubes[currentTarget].blurredPosition;
            cubes[currentTarget].currentScale = cubes[currentTarget].blurredScale;
        }

        if (index >= cubes.Count)
        {
            return;
        }
        currentTarget = index;

        Vector3 center = transform.position;
        center.y += 0.5f * multiplier;
        cubes[currentTarget].currentPosition = center;
        cubes[currentTarget].currentScale = cubes[currentTarget].transform.localScale * 3.5f;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            focus(1 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            focus(2 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            focus(3 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            focus(4 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            focus(5 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            focus(6 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            focus(7 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            focus(8 - 1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            focus(9 - 1);
        } else if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
        {
            focus(-1);
        }
    }
}
