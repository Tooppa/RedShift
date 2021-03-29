using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


// Credit for ThundThund for ShadowCaster2DExtensions
/// <summary>
/// Creates a Shadow Caster 2D to the gameobject if a supported Collider 2D is found.
/// Supported colliders are PolygonCollider2D and CompositeCollider2D.
/// </summary>
public class AutomaticShadowCaster2D : MonoBehaviour
{
    [SerializeField] private bool generateSelfShadows;
    
    private void Start()
    {
        // Find a supported collider type
        if (TryGetComponent(out PolygonCollider2D polygonCollider2D))
        {
            var pointsInPath3D = new Vector3[polygonCollider2D.points.Length];
            
            // Convert Vector2[] to Vector3[]
            for (int j = 0; j < polygonCollider2D.points.Length; ++j)
            {
                pointsInPath3D[j] = polygonCollider2D.points[j]; 
            }
            
            var shadowCaster2D = gameObject.AddComponent<ShadowCaster2D>();

            shadowCaster2D.selfShadows = generateSelfShadows;
            
            shadowCaster2D.SetPath(pointsInPath3D.ToArray());
            shadowCaster2D.SetPathHash(Random.Range(int.MinValue, int.MaxValue)); // Hash set initiates internal recalculation of shadows
        }
        
        else if (TryGetComponent(out CompositeCollider2D compositeCollider2D))
        {
            // Create the new shadow casters, based on the paths of the composite collider
            int pathCount = compositeCollider2D.pathCount;
            List<Vector2> pointsInPath = new List<Vector2>();
            List<Vector3> pointsInPath3D = new List<Vector3>();

            for (int i = 0; i < pathCount; ++i)
            {
                compositeCollider2D.GetPath(i, pointsInPath);

                GameObject newShadowCaster = new GameObject("ShadowCaster2D") {isStatic = true};
                newShadowCaster.transform.SetParent(compositeCollider2D.transform, false);

                for (int j = 0; j < pointsInPath.Count; ++j)
                {
                    pointsInPath3D.Add(pointsInPath[j]);
                }

                ShadowCaster2D component = newShadowCaster.AddComponent<ShadowCaster2D>();
                component.SetPath(pointsInPath3D.ToArray());
                component.SetPathHash(Random.Range(int.MinValue,
                    int.MaxValue)); // The hashing function GetShapePathHash could be copied from the LightUtility class

                pointsInPath.Clear();
                pointsInPath3D.Clear();
            }
        }
    }
        
}
