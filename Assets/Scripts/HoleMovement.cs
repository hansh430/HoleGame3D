using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMovement : MonoBehaviour
{
    [Header("Hole Mesh")]
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private MeshCollider meshCollider;

    [Header("Hole Vertices radius")]
    [SerializeField] private Vector2 moveLimits;
    [SerializeField] private float radius;
    [SerializeField] private Transform holeCenter;
    [Space(5)]
    [SerializeField] private float moveSpeed;
    private Mesh mesh;
    private List<int> holeVertices;
    private List<Vector3> offsets;
    private int holeVerticesCount;
    private float x, y;
    private Vector3 touch, targetPos;
    private void Start()
    {
        GameConstants.IsGameOver = false;
        GameConstants.IsMoving = false;
        holeVertices = new List<int>();
        offsets = new List<Vector3>();
        mesh = meshFilter.mesh;
        FindHoleVertices();
    }
    private void Update()
    {
        GameConstants.IsMoving = Input.GetMouseButton(0);
        if (!GameConstants.IsGameOver && GameConstants.IsMoving)
        {
            MoveHole();
            UpdateHoleVerticesPosition();
        }
    }

    private void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        touch = Vector3.Lerp(holeCenter.position, holeCenter.position + new Vector3(x, 0, y), moveSpeed * Time.deltaTime);
        targetPos = new Vector3(
            Mathf.Clamp(touch.x, -moveLimits.x, moveLimits.x),
            touch.y,
            Mathf.Clamp(touch.z,-moveLimits.y,moveLimits.y)
            ) ;
        holeCenter.position = targetPos;
    }

    private void UpdateHoleVerticesPosition()
    {
        Vector3[] vertices = mesh.vertices;
        for(int i=0; i<holeVerticesCount; i++)
        {
            vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }
        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    private void FindHoleVertices()
    {
        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            float distance = Vector3.Distance(holeCenter.position, mesh.vertices[i]);
            if (distance < radius)
            {
                holeVertices.Add(i);
                offsets.Add(mesh.vertices[i] - holeCenter.position);
            }
        }
        holeVerticesCount=holeVertices.Count;
    }
}
