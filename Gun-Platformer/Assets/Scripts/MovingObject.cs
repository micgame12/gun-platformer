using UnityEngine;

public class MovingObject : MonoBehaviour
{
    [SerializeField] Transform startMarker, endMarker;
    public Vector3 startPos, endPos;

    public float speed = 1.0F;
    private float startTime;
    private float distance, revDistance;
    private bool reverse;
    public bool moving;

    void Start()
    {
        if (moving)
        {
            startPos = new Vector3(startMarker.position.x, startMarker.position.y, startMarker.position.z);
            endPos = new Vector3(endMarker.position.x, endMarker.position.y, endMarker.position.z);

            startTime = Time.time;
            distance = Vector3.Distance(startPos, endPos);
            revDistance = Vector3.Distance(endPos, startPos);
        }
    }

    void Update()
    {
        if (moving)
        {
            float distCovered = (Time.time - startTime) * speed;
            float path = distCovered / distance;
            float revPath = distCovered / revDistance;

            if (transform.position == endPos) { reverse = true; startTime = Time.time; }
            else if (transform.position == startPos) { reverse = false; startTime = Time.time; }

            if (reverse)
            {
                transform.position = Vector3.Lerp(endPos, startPos, path);
            }
            else
            {
                transform.position = Vector3.Lerp(startPos, endPos, path);
            }
        }
    }
}
