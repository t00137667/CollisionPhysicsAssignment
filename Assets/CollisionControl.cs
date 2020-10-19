using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CollisionControl : MonoBehaviour
{
    public List<SphereMovement> spheres;
    List<PlaneScript> planes;

    // Start is called before the first frame update
    void Start()
    {
        spheres = FindObjectsOfType<SphereMovement>().ToList();
        Debug.Log(spheres.Count);
        planes = FindObjectsOfType<PlaneScript>().ToList();
        Debug.Log(planes.Count);
    }

    // Update is called once per frame
    void Update()
    {
        
        CollisionCheck();
    }

    public float DistanceBetween(Vector3 firstPoint, Vector3 secondPoint)
    {
        return Vector3.Dot(firstPoint, secondPoint);
    }

    private void CollisionCheck()
    {

        for (int i = 0; i < spheres.Count - 1; i++)
        {
            for (int j = i + 1; j < spheres.Count; j++)
            {
                //Process sphere i with sphere j
                if (Vector3.Distance(spheres[i].transform.position, spheres[j].transform.position) - (spheres[i].radius + spheres[j].radius) <= 0)
                {
                    spheres[i].CollidesWith(spheres[j].gameObject);
                }
            }
        }
        for (int i = 0; i < spheres.Count; i++)
        {
            for (int j = 0; j < planes.Count; j++)
            {
                if (planes[j].DistanceTo(spheres[i].transform.position) - spheres[i].radius < 0)
                {
                    spheres[i].CollidesWith(planes[j].gameObject);
                }
            }
        }
    }
}
