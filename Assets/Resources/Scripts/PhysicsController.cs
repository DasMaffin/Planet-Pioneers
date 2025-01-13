using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public Transform centerOfMass;

    private Rigidbody myRB;

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        foreach(GravityMesh gravityMesh in GetGravityMeshesInRadius(centerOfMass.position, 10f))
        {
            Vector3 nearestVertex = gravityMesh.NearestVertexTo(centerOfMass.position);
            Debug.DrawLine(centerOfMass.position, nearestVertex, Color.red);

            myRB.AddForce((nearestVertex - centerOfMass.position).normalized * 10f);
        }
    }

    public List<GravityMesh> GetGravityMeshesInRadius(Vector3 position, float radius)
    {
        // List to store the GameObjects with GravityMesh component
        List<GravityMesh> gravityMeshesInRange = new List<GravityMesh>();

        // Use Physics.OverlapSphere to get colliders within the radius
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach(Collider collider in colliders)
        {
            // Check if the collider has the GravityMesh component
            GravityMesh gravityMesh = collider.GetComponent<GravityMesh>();

            if(gravityMesh != null)
            {
                // If the GravityMesh component is found, add the GameObject to the list
                gravityMeshesInRange.Add(gravityMesh);
            }
        }

        return gravityMeshesInRange;
    }
}
