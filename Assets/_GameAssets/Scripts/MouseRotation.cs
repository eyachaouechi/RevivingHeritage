using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    private Vector2 mouseStartPos;
    public float rotationSpeed = 1f;
    private Transform objectTransform;
    private Collider objectCollider;
    bool canMove = false;

    private void Start()
    {
        objectTransform = transform;
        objectCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (objectCollider.Raycast(ray, out hit,20))
            {
                mouseStartPos = Input.mousePosition;
                canMove = true;
                Debug.Log(objectCollider);
            }
        }
        else if (Input.GetMouseButton(0)&& canMove)
        {
            if (Vector2.Distance(Input.mousePosition, mouseStartPos) > 20f) // Adjust the threshold distance as needed
            {
                float rotationAmount = (Input.mousePosition.x - mouseStartPos.x) * rotationSpeed;
                objectTransform.Rotate(Vector3.up, rotationAmount);
            }
        }
        else if(Input.GetMouseButtonUp(0) && canMove)
        {
            canMove = false;
        }
    }
}