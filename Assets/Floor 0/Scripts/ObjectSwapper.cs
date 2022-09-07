using UnityEngine;
[ExecuteInEditMode]
public class ObjectSwapper : MonoBehaviour
{
    public GameObject newWall { get; set; }
    public void SetUp(GameObject newObject, GameObject newParent, Vector3 newPosition)
    {
        if (newParent != null)
        {
            newWall = Instantiate(newObject, newPosition, Quaternion.identity, newParent.transform);

            newWall.SetActive(true);
        }
    }
    public void RotateWall()
    {
        newWall.transform.Rotate(0, 90, 0);
    }
}
