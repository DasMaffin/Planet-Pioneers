using System.Collections.Generic;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public Transform feet;
    public Transform head;

    private Rigidbody myRB;

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        foreach(GravityMesh gravityMesh in GetGravityMeshesInRadius(feet.position, 10f))
        {
            // Find the nearest vertex on the gravity mesh
            Vector3 nearestVertex = gravityMesh.NearestVertexTo(feet.position);

            // Draw a line for debugging purposes
            Debug.DrawLine(feet.position, nearestVertex, Color.red);

            // Calculate the direction from the parent (character's body) to the nearest vertex
            Vector3 directionToVertex = (nearestVertex - feet.position).normalized;

            // Apply a force to the Rigidbody to move the character towards the nearest vertex
            myRB.AddForce(directionToVertex * 10f);

            // Move the parent object (character body) towards the nearest vertex
            transform.position = nearestVertex;

            // Optionally adjust the parent object's rotation to face the nearest vertex
            Vector3 directionToFace = nearestVertex - head.position;
            Quaternion targetRotation = Quaternion.LookRotation(directionToFace);

            // Smoothly rotate the parent object to face the direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

            // Optionally adjust the head's rotation to smoothly face the direction as well
            head.rotation = Quaternion.Slerp(head.rotation, targetRotation, Time.deltaTime * 5f);
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
