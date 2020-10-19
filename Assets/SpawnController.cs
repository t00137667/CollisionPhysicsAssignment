using UnityEngine;

public class SpawnController : MonoBehaviour
{
    CollisionControl CollisionControl;
    SphereMovement sphere;

    // Start is called before the first frame update
    void Start()
    {
        CollisionControl = FindObjectOfType<CollisionControl>();
        sphere = FindObjectOfType<SphereMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 spawn_point = transform.position;
            spawn_point.z += 7.5f;
            SpawnSphere(spawn_point);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 spawn_point = transform.position;
            spawn_point.z -= 7.5f;
            SpawnSphere(spawn_point);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Vector3 spawn_point = transform.position;
            spawn_point.x -= 7.5f;
            SpawnSphere(spawn_point);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Vector3 spawn_point = transform.position;
            spawn_point.x += 7.5f;
            SpawnSphere(spawn_point);
        }
    }

    void SpawnSphere(Vector3 spawnPoint)
    {
        SphereMovement clone;
        clone = Instantiate(sphere, spawnPoint, transform.rotation);

        int radius_multipler = Random.Range(1, 4);
        clone.radius = clone.radius * radius_multipler;
        clone.transform.localScale = new Vector3(clone.radius * 2, clone.radius * 2, clone.radius * 2);

        int mass_multiplier = Random.Range(1, 5);
        clone.mass = clone.mass * mass_multiplier;

        CollisionControl.spheres.Add(clone);
    }
}
