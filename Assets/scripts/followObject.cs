using UnityEngine;
using System.Collections.Generic;

public class followObject : MonoBehaviour
{
    public List<Vector2> posiciones;
    public float progress = 0.0f;
    public float speed = 1.0f;
    public List<Vector2> startPosition;
    public void addPoint(Vector2 point)
    {
        startPosition.Add(posiciones.Count > 0 ? posiciones[posiciones.Count - 1] : gameObject.transform.position);
        posiciones.Add(point);
        
    }

    private void Update()
    {
        if (posiciones.Count > 0)
        {
            gameObject.transform.position = Vector2.Lerp(startPosition[0], posiciones[0], progress);
            progress = Mathf.Min( progress + Time.deltaTime * speed, 1.0f);
        }
        if (progress  >= 1.0f && posiciones.Count > 0)
        {
            progress = 0;
            posiciones.RemoveAt(0);
            startPosition.RemoveAt(0);
        }
    }
}
