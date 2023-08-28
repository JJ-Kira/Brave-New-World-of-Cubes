using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class CubeSmash : MonoBehaviour
{
    [SerializeField] private int cubesPerAxis = 8;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision");
        if (collision.gameObject.name == "Crowbar" && transform.localScale.x > 0.2f)
            SmashCube();
    }

    private void SmashCube()
    {
        for (int x = 0; x < cubesPerAxis; x++)
            for (int y = 0; y < cubesPerAxis; y++)
                for (int z = 0; z < cubesPerAxis; z++)
                    CreateSmallCube(new Vector3(x, y, z));
        Destroy(gameObject);
    }

    private void CreateSmallCube(Vector3 coordinates)
    {
        var cube = Instantiate(gameObject, Vector3.zero, Quaternion.identity);
        cube.transform.localScale = transform.localScale / cubesPerAxis;
        var firstCube = transform.position - transform.localScale / 2 + cube.transform.localScale / 2;
        cube.transform.position = firstCube + Vector3.Scale(coordinates, cube.transform.localScale);

        var physicsGrab = cube.GetComponent<PhysicsGrabbable>();
        physicsGrab.InjectGrabbable(cube.GetComponent<Grabbable>());
        physicsGrab.InjectRigidbody(cube.GetComponent<Rigidbody>());
    }
}