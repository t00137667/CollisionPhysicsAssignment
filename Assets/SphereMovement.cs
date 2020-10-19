using Assets;
using UnityEngine;

public class SphereMovement : MonoBehaviour, ICollidable
{
    PlaneScript nearestPlane;
    SphereMovement nearestSphere;
    Vector3 velocity = new Vector3(0, 0, 0);
    Vector3 acceleration = new Vector3(0, -9.8f, 0);
    Vector3 lastPosition;
    public float Coefficent_of_Restitution = 0.9f;
    public float radius = 0.5f;
    public float mass = 1f;

    public bool IsColliding { get; set; } = false;
    public PlaneScript NearestPlane { get => nearestPlane; set => nearestPlane = value; }
    public SphereMovement NearestSphere { get => nearestSphere; set => nearestSphere = value; }

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity(velocity + acceleration * Time.deltaTime);
        UpdatePosition(velocity * Time.deltaTime);
    }

    public void UpdateVelocity(Vector3 newVelocity)
    {
        velocity = newVelocity;
        if (velocity.magnitude > 50)
        {
            velocity = velocity * 0.5f;
            Debug.Log("Brakes applied");
        }
    }

    public void UpdatePosition(Vector3 velocity)
    {
        lastPosition = transform.position;
        transform.position += velocity;
    }

    public void CollidesWith(GameObject target)
    {
        switch (target.tag)
        {
            case "sphere":
                {
                    Debug.Log("Sphere Collision Detected");
                    SphereMovement otherSphere = target.GetComponent<SphereMovement>();
                    float d1 = otherSphere.DistanceTo(lastPosition) - radius;
                    float d2 = otherSphere.DistanceTo(transform.position) - radius;
                    float s1 = DistanceTo(otherSphere.lastPosition) - otherSphere.radius;
                    float s2 = DistanceTo(otherSphere.transform.position) - otherSphere.radius;

                    float time_of_impact = Time.deltaTime * d1 / (d1 - d2);
                    float time_of_other_impact = Time.deltaTime * s1 / (s1 - s2);
                    //transform.position -= velocity * (Time.deltaTime - time_of_impact);
                    UpdatePosition(-velocity * (Time.deltaTime - time_of_impact));
                    otherSphere.UpdatePosition(-otherSphere.velocity * (Time.deltaTime - time_of_other_impact));

                    Vector3 normal = Normal(otherSphere.transform.position) * radius;
                    Vector3 par = parallelComponent(velocity, normal);
                    Vector3 per = perpendicularComponent(velocity, normal);

                    Vector3 otherNormal = otherSphere.Normal(transform.position) * otherSphere.radius;
                    Vector3 otherPar = otherSphere.parallelComponent(otherSphere.velocity, otherNormal);
                    Vector3 otherPer = otherSphere.perpendicularComponent(otherSphere.velocity, otherNormal);

                    float total_restitution = Coefficent_of_Restitution * otherSphere.Coefficent_of_Restitution;
                    Vector3 newVelocity =  per + Momentum(par, otherPar, mass, otherSphere.mass) * total_restitution;
                    Vector3 newVelocityTwo = otherPer + Momentum(otherPar, par, otherSphere.mass, mass) * total_restitution;


                    //Calculate new velocity
                    UpdateVelocity(newVelocity);
                    otherSphere.UpdateVelocity(newVelocityTwo);

                    //Apply new velocity
                    UpdatePosition(velocity * (Time.deltaTime - time_of_impact));
                    otherSphere.UpdatePosition(otherSphere.velocity * (Time.deltaTime - time_of_other_impact));

                    break;
                }
            case "plane":
                {
                    Debug.Log("Plane Collision Detected");
                    PlaneScript plane = target.GetComponent<PlaneScript>();
                    float d1 = plane.DistanceTo(lastPosition) - radius;
                    float d2 = plane.DistanceTo(transform.position) - radius;

                    Vector3 par = parallelComponent(velocity, plane.normal);
                    Vector3 per = perpendicularComponent(velocity, plane.normal);

                    //Backtrack to point of impact
                    float time_of_impact = Time.deltaTime * d1 / (d1 - d2);
                    UpdatePosition(-velocity * (Time.deltaTime - time_of_impact));

                    //Calculate new velocity
                    velocity = per - par * Coefficent_of_Restitution;

                    //Apply new velocity
                    UpdatePosition(velocity * (Time.deltaTime - time_of_impact));

                    break;
                }
            default: { break; }

        }
    }

    public Vector3 Momentum(Vector3 parallelOne, Vector3 parallelTwo, float massOne, float massTwo)
    {
        Vector3 vector = ((massOne - massTwo) / (massOne + massTwo) * parallelOne) + ((massTwo * 2) / (massOne + massTwo) * parallelTwo);
        return vector;
    }

    public float DistanceTo(Vector3 sphere)
    {
        return Vector3.Distance(transform.position, sphere) - radius;
    }

    private Vector3 Normal(Vector3 otherSphere)
    {
        return (otherSphere - this.transform.position).normalized;
    }

    private Vector3 perpendicularComponent(Vector3 vector, Vector3 normal)
    {
        return vector - parallelComponent(vector, normal);
    }

    private Vector3 parallelComponent(Vector3 vector, Vector3 normal)
    {
        return Vector3.Dot(vector, normal) * normal;
    }
}
