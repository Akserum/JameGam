using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        Handles.color = Color.white;
        Vector3 position = fov.transform.position + new Vector3(0f, 1f, 0f);
        Handles.DrawWireArc(position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(position, position + viewAngle01 * fov.radius);
        Handles.DrawLine(position, position + viewAngle02 * fov.radius);

        Handles.color = Color.green;
        Handles.DrawWireDisc(position, Vector3.up, fov.radius);
    }

    protected Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}