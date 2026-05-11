using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.U2D;

public class metaball : MonoBehaviour
{
    #region variables
    #region variablesExternas
    private SpriteShapeController metaballController;
    private Vector2[] directionPoint;
    private float[] amplitudPoints;
    private Vector2[] tangentPositions;
    #endregion

    private AudioSource musica;

    private float[] objetivoPoints;
    private float[] spectrumData = new float[1024];
    private float time;
    private float baseAngle;
    [Header("Config")]
    [Header("Metaball Config")]
    public int NumberPoints = 16;
    public float VelocidadRotacion = 0.1f;
    public float VelocidadAmplitud = 20.0f;
    public float VelocidadAmplitudObjetivo = 1.0f;
    public float Amplitud = 3.0f;
    public float TangentAmplitud = 0.5f;
    public float FuerzaMusica = 10.0f;
    public float FuerzaMaxima = 5.0f;
    [Header("Music Config")]
    public int StartDataView = 900;
    public float GetMusicEvery = 0.1f;
    #endregion

    #region core
    private void Start()
    {
        metaballController = GetComponent<SpriteShapeController>();
        musica = GetComponent<AudioSource>();
        CreateCirclePoints();
    }

    private void Update()
    {
        RotateCircle();

        AddLocalTime();

        MoveCircle();
    }
    #endregion

    private void AddLocalTime()
    {
        time += Time.deltaTime;
    }

    //anima la metaball siguiendo el espectro de audio
    private void MoveCircle()
    {
        int progressSpectro = (spectrumData.Length - StartDataView) / NumberPoints;

        if (time >= GetMusicEvery)
        {
            musica.GetSpectrumData(spectrumData, 0, FFTWindow.Rectangular);
        }

        for (int i = 0; i < NumberPoints; i++)
        {
            amplitudPoints[i] = Mathf.Lerp(amplitudPoints[i], objetivoPoints[i], VelocidadAmplitud * Time.deltaTime);
            amplitudPoints[i] = amplitudPoints[i] <= Amplitud ? Amplitud : amplitudPoints[i];

            if (time >= GetMusicEvery)
            {
                objetivoPoints[i] = Amplitud + spectrumData[progressSpectro * i] * FuerzaMusica;
                objetivoPoints[i] = objetivoPoints[i] >= FuerzaMaxima ? FuerzaMaxima : objetivoPoints[i];
            }
            else
            {
                objetivoPoints[i] = Mathf.Lerp(objetivoPoints[i], Amplitud, VelocidadAmplitudObjetivo * Time.deltaTime);
            }

            metaballController.spline.SetPosition(i, directionPoint[i] * amplitudPoints[i]);
        }
        time = time >= GetMusicEvery ? 0.0f : time;
    }

    //rota el circulo para darle mas variedad
    private void RotateCircle()
    {
        baseAngle += VelocidadRotacion * Time.deltaTime;

        transform.rotation = quaternion.RotateZ(baseAngle);
    }

    /// <summary>
    /// sirve para recalcular los datos del circulo para datos que solo se calculan una vez
    /// </summary>
    public void CreateCirclePoints()
    {
        metaballController.spline.Clear();

        float progresoPorIteracion = 360.0f / NumberPoints;

        tangentPositions = new Vector2[NumberPoints];
        directionPoint = new Vector2[NumberPoints];     
        amplitudPoints = new float[NumberPoints];
        objetivoPoints = new float[NumberPoints];

        for (int i = 0; i < NumberPoints; i++)
        {
            float angle = progresoPorIteracion * i;
            float radAngle = angle * Mathf.Deg2Rad;
            float tangentAnglerad = (angle - 90.0f) * Mathf.Deg2Rad;

            Vector2 pointAngle = new Vector2(MathF.Cos(radAngle) , -MathF.Sin(radAngle) );
            Vector3 positionTangent = new Vector3(MathF.Cos(tangentAnglerad) * TangentAmplitud, -MathF.Sin(tangentAnglerad) * TangentAmplitud, 0.0f);

            metaballController.spline.InsertPointAt(i , new Vector3(pointAngle.x * Amplitud, pointAngle.y * Amplitud, 0.0f) );
            metaballController.spline.SetTangentMode(i, ShapeTangentMode.Continuous);

            metaballController.spline.SetLeftTangent(i, positionTangent);
            metaballController.spline.SetRightTangent(i, -positionTangent);

            directionPoint[i] = pointAngle;
            amplitudPoints[i] = Amplitud;
            objetivoPoints[i] = Amplitud;
            tangentPositions[i] = positionTangent;
        }

        metaballController.spline.isOpenEnded = false;
    }

    public Vector2[] TangentPositions => tangentPositions;

    public float[] AmplitudPoints => amplitudPoints;

    public Vector2[] DirectionPoint => directionPoint;

    public SpriteShapeController MetaballController => metaballController;

}
