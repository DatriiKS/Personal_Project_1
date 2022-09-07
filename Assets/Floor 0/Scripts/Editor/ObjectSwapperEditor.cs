using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(ObjectSwapper))]
public class ObjectSwapperEditor : Editor
{
    private bool swapping = false;

    private GameObject newObject;

    private void OnEnable()
    {
        SceneView.beforeSceneGui += OnSceneViewBeforeSceneGUI;
    }
    private void OnDisable()
    {
        SceneView.beforeSceneGui -= OnSceneViewBeforeSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        swapping = EditorGUILayout.Toggle("Is tool on", swapping);

        newObject = EditorGUILayout.ObjectField("Object to swap to", newObject, typeof(GameObject), true) as GameObject;
    }

    private void OnSceneViewBeforeSceneGUI(SceneView sceneView)
    {
        ObjectSwapper swapper = (ObjectSwapper)target;

        if (swapping && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000f))
            {

                GameObject newParent = null;
                if (hit.collider.gameObject.transform.parent != null)
                {
                    newParent = GetWallObject(hit.collider.gameObject).transform.parent.gameObject;
                }
                Vector3 newPosition = GetWallObject(hit.collider.gameObject).transform.position;

                DestroyImmediate(GetWallObject(hit.collider.gameObject));

                Debug.Log("hi");
                swapper.SetUp(newObject, newParent, newPosition);
            }
            Event.current.Use();
        }

        if (swapping && Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.R)
        {
            Debug.Log("Rotate");

            swapper.RotateWall();
        }
    }

    private GameObject GetWallObject(GameObject rayTarget)
    {
        GameObject current = rayTarget;

        if (current.GetComponent<Wall>())
        {
            return current;
        }
        else
        {
            if (current.transform.parent.gameObject != null)
            {
                current = GetWallObject(current.transform.parent.gameObject);
            }
            else
            {
                Debug.Log("No Parent!");
            }
        }

        return current;
    }
}