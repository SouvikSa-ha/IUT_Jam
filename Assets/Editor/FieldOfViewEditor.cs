using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
  private void OnSceneGUI()
  {
    FieldOfView fov = (FieldOfView)target;
    Handles.color = Color.white;
    Handles.DrawWireArc(fov.transform.position, Vector3.forward, Vector3.up, 360, fov.radius);

    Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.z, -fov.angle / 2);
    Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.z, fov.angle / 2);

    Handles.color = Color.yellow;
    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
    Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

    if (fov.canSeePlayer)
    {
      Handles.color = Color.green;
      Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
    }
  }

  private Vector2 DirectionFromAngle(float eulerZ, float angleInDegrees)
  {
    angleInDegrees += eulerZ + 90;

    return new Vector2(Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), Mathf.Sin(angleInDegrees * Mathf.Deg2Rad));
  }
}
