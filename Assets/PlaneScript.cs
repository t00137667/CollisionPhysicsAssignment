using Assets;
using UnityEngine;

public class PlaneScript : MonoBehaviour, ICollidable
{

    Vector3 point_on_plane, normal_to_plane;
    public float Coefficient_of_Restitution = 0.5f;

    public Vector3 normal { get { return normal_to_plane; }}

    // Start is called before the first frame update
    void Start()
    {
        FindPlaneDimensions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FindPlaneDimensions()
    {
        point_on_plane = transform.position;
        normal_to_plane = transform.up;
    }

    public void DefinePlane(Vector3 point, Vector3 normal)
    {
        point_on_plane = point;
        normal_to_plane = normal.normalized;

        transform.position = point_on_plane;
        transform.up = normal_to_plane;
    }

    public float DistanceTo(Vector3 point)
    {
        Vector3 point_on_plane_to_point =  point - point_on_plane;

        return Vector3.Dot(point_on_plane_to_point, normal_to_plane);
    }

    public void CollidesWith(GameObject target)
    {

    }
}
