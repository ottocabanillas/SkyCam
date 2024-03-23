using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PilotInfoDisplay : MonoBehaviour
{
    [SerializeField]
    private TMP_Text sp_xText; // Texto para mostrar el punto x deseado

    [SerializeField]
    private TMP_Text sp_yText; // Texto para mostrar el punto y deseado

    [SerializeField]
    private TMP_Text sp_zText; // Texto para mostrar el punto z deseado

    [SerializeField]
    private TMP_Text r1Text; // Texto para mostrar el largo real de cuerda de VMU1

    [SerializeField]
    private TMP_Text r2Text; // Texto para mostrar el largo real de cuerda de VMU2

    [SerializeField]
    private TMP_Text r3Text; // Texto para mostrar el largo real de cuerda de VMU3

    [SerializeField]
    private TMP_Text r4Text; // Texto para mostrar el largo real de cuerda de VMU4

    [SerializeField]
    private TMP_Text rxText; // Texto para mostrar la posicion real del dispositivo en el eje X

    [SerializeField]
    private TMP_Text ryText; // Texto para mostrar la posicion real del dispositivo en el eje Y

    [SerializeField]
    private TMP_Text rzText; // Texto para mostrar la posicion real del dispositivo en el eje Z

    [SerializeField]
    private TMP_Text distanceText; // Texto para mostrar la distancia entre la skycam en Unity y la skycam real

    [SerializeField]
    private TMP_Text timeText; // Texto para mostrar el tiempo

    [SerializeField]
    private TMP_Text v1Text; // Texto para mostrar la velocidad del motor 1

    [SerializeField]
    private TMP_Text v2Text; // Texto para mostrar la velocidad del motor 2

    [SerializeField]
    private TMP_Text v3Text; // Texto para mostrar la velocidad del motor 3

    [SerializeField]
    private TMP_Text v4Text; // Texto para mostrar la velocidad del motor 4

    [SerializeField]
    private GameObject skycam; // Skycam

    private DirectKinematic directKinematicModel;

    private GlobalVariables g_variables;

    private Vector3 desiredPosition;

    // Start is called before the first frame update
    void Start()
    {
        g_variables = GlobalVariables.Instance;
        //directKinematicModel = DirectKinematic.Instance;
        desiredPosition = skycam.transform.position;
        sp_xText.SetText("SPx: " + (desiredPosition.x * 1000).ToString("N0") + " mm");
        sp_yText.SetText("SPy: " + (desiredPosition.z * 1000).ToString("N0") + " mm");
        sp_zText.SetText("SPz: " + (desiredPosition.y * 1000).ToString("N0") + " mm");

        // Largos reales de cuerdas emitidos por ArgosUC
        r1Text.SetText("R1: " + (g_variables.R1).ToString("N0") + " mm");
        r2Text.SetText("R2: " + (g_variables.R2).ToString("N0") + " mm");
        r3Text.SetText("R3: " + (g_variables.R3).ToString("N0") + " mm");
        r4Text.SetText("R4: " + (g_variables.R4).ToString("N0") + " mm");

        // Posicion X,Y,Z real
        rxText.SetText("Rx: " + (g_variables.Rx * 1000).ToString("N2") + " mm");
        ryText.SetText("Ry: " + (g_variables.Rz * 1000).ToString("N2") + " mm");
        rzText.SetText("Rz: " + (g_variables.Ry * 1000).ToString("N2") + " mm");

        // Texto distancia
        distanceText.SetText("D: " + (g_variables.d * 1000).ToString("N0") + " mm");

        // Tiempo
        timeText.SetText("T: " + (g_variables.T * 1000).ToString("N0") + " s");
        
        // Velocidades de los motores
        v1Text.SetText("V1: " + (g_variables.v1).ToString("N2") + " mm/s");
        v2Text.SetText("V2: " + (g_variables.v2).ToString("N2") + " mm/s");
        v3Text.SetText("V3: " + (g_variables.v3).ToString("N2") + " mm/s");
        v4Text.SetText("V4: " + (g_variables.v4).ToString("N2") + " mm/s");
    }

    // Update is called once per frame
    void Update()
    {
        desiredPosition = skycam.transform.position;
        // Posicion deseada X,Y,Z
        sp_xText.SetText("SPx: " + (desiredPosition.x * 1000).ToString("N0") + " mm");
        sp_yText.SetText("SPy: " + (desiredPosition.z * 1000).ToString("N0") + " mm");
        sp_zText.SetText("SPz: " + (desiredPosition.y * 1000).ToString("N0") + " mm");

        // Largos reales de cuerdas emitidos por ArgosUC
        r1Text.SetText("R1: " + (g_variables.R1).ToString("N0") + " mm");
        r2Text.SetText("R2: " + (g_variables.R2).ToString("N0") + " mm");
        r3Text.SetText("R3: " + (g_variables.R3).ToString("N0") + " mm");
        r4Text.SetText("R4: " + (g_variables.R4).ToString("N0") + " mm");

        // Posicion X, Y, Z real
        if (double.IsNaN(g_variables.Rx))
            rxText.SetText("Rx: N/A");
        else
            rxText.SetText("Rx: " + (g_variables.Rx * 1000).ToString("N0") + " mm");

        if (double.IsNaN(g_variables.Rz))
            ryText.SetText("Ry: N/A");
        else
            ryText.SetText("Ry: " + (g_variables.Rz * 1000).ToString("N0") + " mm");

        if (double.IsNaN(g_variables.Ry))
            rzText.SetText("Rz: N/A");
        else
            rzText.SetText("Rz: " + (g_variables.Ry * 1000).ToString("N0") + " mm");

        // Texto distancia
        distanceText.SetText("D: " + (g_variables.d * 1000).ToString("N0") + " mm");

        // Tiempo
        timeText.SetText("T: " + (g_variables.T * 1000).ToString("N0") + " s");
        
        // Velocidades de los motores
        v1Text.SetText("V1: " + (g_variables.v1).ToString("N2") + " mm/s");
        v2Text.SetText("V2: " + (g_variables.v2).ToString("N2") + " mm/s");
        v3Text.SetText("V3: " + (g_variables.v3).ToString("N2") + " mm/s");
        v4Text.SetText("V4: " + (g_variables.v4).ToString("N2") + " mm/s");
    }

}