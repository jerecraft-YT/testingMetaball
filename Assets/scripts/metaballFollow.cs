using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class metaballFollow : MonoBehaviour
{
    public metaball metaball;
    private SpriteShapeController controller;
    private bool m_Follow = false;
    private List<float> amplitudPoints;
    public float velocidadSeguimiento = 5.0f;
    void Start()
    {
        controller = GetComponent<SpriteShapeController>();
        controller.spline.Clear();
    }
    void Update()
    {
        if (metaball.controller.spline.GetPointCount() != 0)
        {
            if (!m_Follow)
            {
                m_Follow = true;
                for (int i = 0; i < metaball.NumberPoints; i++)
                {
                    controller.spline.InsertPointAt(i, metaball.controller.spline.GetPosition(i));
                    controller.spline.SetTangentMode(i, ShapeTangentMode.Continuous);

                    controller.spline.SetLeftTangent(i, metaball.tangentPositions[i]);
                    controller.spline.SetRightTangent(i, -metaball.tangentPositions[i]);
                    amplitudPoints = new List<float>(metaball.amplitudPoints);
                }
            }

            for (int i = 0; i < metaball.NumberPoints; i++)
            {
                controller.spline.SetPosition(i, metaball.directionPoint[i] * amplitudPoints[i]);
                
                amplitudPoints[i] = amplitudPoints[i] < metaball.amplitudPoints[i] ? metaball.amplitudPoints[i] : amplitudPoints[i];

                amplitudPoints[i] = amplitudPoints[i] > metaball.amplitudPoints[i] ? Mathf.Lerp(amplitudPoints[i], metaball.Amplitud, velocidadSeguimiento * Time.deltaTime) : amplitudPoints[i];
            }
        }
    }
}
