using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ArcamGUI : MonoBehaviour
{
    // Internal Properties
    public Text  cameraPosition,
                 skycamHeight,
                 skycamSpeed;
    public GameObject cameraObject,
                      skycamObject;
    public CameraController cameraController;
    public SkycamController skycamController;

    // Colores para el texto de la altura según la altura promedio de la cámara:
    // Verde: si la altura promedio está entre 0 y 50%
    // Amarillo: si la altura promedio está entre 50% y 75%
    // Rojo: si la altura promedio está entre 75% y 100%
    [SerializeField]
    Color green, yellow, red;

    private GlobalVariables g_variables;

    // Start is called before the first frame update
    void Start()
    {
        g_variables = GlobalVariables.instance;
        cameraController = FindAnyObjectByType<CameraController>();
        cameraPosition = cameraObject.GetComponent<Text>();
        skycamController = FindAnyObjectByType<SkycamController>();

    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.text = "Posicion: " + cameraController.cameraName;
        skycamHeight.text = "Altura: " + skycamController.currentHeight;
        skycamSpeed.text = "Velocidad: " + g_variables.currentSpeed;

        SetSkycamHeightLabelColor(skycamHeight);
    }

private void SetSkycamHeightLabelColor(Text skycamHeight)
    {
        Color currentColor = skycamHeight.color;
        float skycamHeightAverage = (float.Parse(skycamController.currentHeight) * 100f) / 2.28f; //4.5f representa la altura maxima
        Color targetColor;
        float lerpPosition; // Esta variable representa la posición en la interpolación lineal entre dos colores


        switch (skycamHeightAverage)
        {
            case float n when (n > 0 && n <= 50):
            targetColor = Color.green;
            break;

            case float n when (n > 50 && n <= 75):
            // Si la altura promedio está entre 50 y 75, hacemos una interpolación lineal entre verde y amarillo
            lerpPosition = (n - 50) / 25f;
            targetColor = Color.Lerp(Color.green, Color.yellow, lerpPosition); // Transicion de verde a amarillo
            break;

            case float n when (n > 75 && n <= 100):
            // Si la altura promedio está entre 75 y 100, hacemos una interpolación lineal entre amarillo y rojo
            lerpPosition = (n - 75) / 25f;
            targetColor = Color.Lerp(Color.yellow, Color.red, lerpPosition); // Transicion de amarillo a rojo
            break;

            default:
            // Si la altura promedio está fuera de los rangos definidos, el color objetivo es rojo... Algo salio mal???
            targetColor = Color.red;
            break;
        }

        currentColor = Color.Lerp(currentColor, targetColor, 5f * Time.deltaTime);
        skycamHeight.color = currentColor;
    }
}   
