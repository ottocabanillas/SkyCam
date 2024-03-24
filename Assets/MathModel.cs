using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class MathModel : MonoBehaviour
{
    private GlobalVariables g_variables;
    // Values for Field
    // -Size
    private float heightValue, widthValue, lengthValue;

    // Values for Argos - Skycam
    // -Size
    private float heightSkyCam = 500.0f, widthSkyCam = 500.0f, lengthSkyCam = 500.0f;
    // -SP Position
    public static float sPX, sPY, sPZ;
    // -SP lenght rope
    public float r1sP, r2sP, r3sP, r4sP;

    private float maxSpeed;
    private float currentSpeed;

    // Values from UC (ESP32)
    // -Real length rope
    public float r1Length, r2Length, r3Length, r4Length; 
    
    // Calculated values ​​for UC
    // -Velocidad del motor (X) ---> (dX/time)
    public float v1, v2, v3, v4;
    // -Direccion de giro
    public string[] motorsDirections = new string[4];

    // Values ​​to calculate
    // -Calculate position from real world
    public float pX, pY, pZ;
    // -Diferencia entre el pto (x, y, z) del real contra el esperado, SP
    private float dX, dY, dZ;
    // -Time (D/V)
    public float time;
    // -Diferencia entre el largo real (rXLength) y y esperado (rXsP) [mm]
    private float dR1, dR2, dR3, dR4;

    private float aux1, aux2, aux3;
    // -New length of rope
    public float newRopeLength;

    // Objects used by unity
    public GameObject skyCamArgos, rope1, rope2, rope3, rope4;

    // 
    public TMP_Text sPXText, sPYText, sPZText;

    public TMP_Text r1sPText, r2sPText, r3sPText, r4sPText;

    // UI Text for real ropes length
    public TMP_Text r1LengthText, r2LengthText, r3LengthText, r4LengthText;

    // UI Text for motor velocities
    public TMP_Text v1Text, v2Text, v3Text, v4Text;

    // UI Text for real x,y,z positions
    public TMP_Text pXText, pYText, pZText;

    public TMP_Text distanceText;

    public TMP_Text timeText;

    void Start()
    {
        g_variables = GlobalVariables.Instance;
        setupValues();
        
        // Iniciar la corutina para mandar constantemente el TTL
        StartCoroutine(sendTimeToLivePeriodically());
        StartCoroutine(sendCommands());
    }

    // Update is called once per frame
    void Update()
    {
        setupValues();
        calculateDeltaForRope();
        directkinematics();
        calculateDeltaForDistance();
        calculateRopeLength();
        calculateTime();
        calculateMotorVelocities();
        setMotorsDirections();  
        showUI(); 
    }

    void setupValues()
    {
        // Convert to mm 
        // Area de trabajo 
        heightValue = PlayerPrefs.GetFloat(CommonConfigKeys.HEIGHT.ToString()) * 1000.0f;
        widthValue = PlayerPrefs.GetFloat(CommonConfigKeys.WIDTH.ToString()) * 1000.0f;
        lengthValue = PlayerPrefs.GetFloat(CommonConfigKeys.LENGTH.ToString()) * 1000.0f;

        //Debug Valores en [mm]
        // Debug.Log(heightValue);
        // Debug.Log(widthValue);
        // Debug.Log(lengthValue);
        
        // Length Rope skycam
        r1sP = (float)g_variables.sp1 * 1000.0f;
        r2sP = (float)g_variables.sp2 * 1000.0f;
        r3sP = (float)g_variables.sp3 * 1000.0f;
        r4sP = (float)g_variables.sp3 * 1000.0f;

        // Debug Valores en [mm]
        // Debug.Log("Largo de Cuerda r1sP" + r1sP);
        // Debug.Log(r2sP);
        // Debug.Log(r3sP);
        // Debug.Log(r4sP);

        // Length Rope from UC
        r1Length = (float)g_variables.R1;
        r2Length = (float)g_variables.R2;
        r3Length = (float)g_variables.R3;
        r4Length = (float)g_variables.R4;

        //Debug Valores en [mm]
        //Debug.Log("Largo de Cuerda r1Length" + r1Length);
        // Debug.("Largo de Cuerda r1Length" + r1Length);
        // Debug.("Largo de Cuerda r1Length" + r1Length);
        // Debug.("Largo de Cuerda r1Length" + r1Length);
        
        // Position Box
        sPX = (float)g_variables.spx * 1000.0f;
        sPY = (float)g_variables.spz * 1000.0f;
        sPZ = (float)g_variables.spy * 1000.0f;

        //Debug Valores en [mm]
        // Debug.Log("sPX: " + sPX);
        // Debug.Log("sPy: " + sPY);
        // Debug.Log("sPz: " + sPZ);


    }

    // Calcula la diferencia entre el largo de las cuerdas rXsP(1, 2 , 3, 4) rXLength(1, 2 , 3, 4)
    // -dRX(1, 2 , 3, 4) = rXsP(1, 2 , 3, 4) - rXLength(1, 2 , 3, 4)
    public void calculateDeltaForRope()
    {
        dR1 = r1sP - r1Length;
        dR2 = r2sP - r2Length; 
        dR3 = r3sP - r3Length; 
        dR4 = r4sP - r4Length;
    }

    // Cinemática directa (a partir de las cuerdas, calcula la posición de la cámara)
    public void directkinematics()
    {
    
        pX = ((r1Length * r1Length) - (r4Length * r4Length) + (lengthValue * lengthValue)-(lengthValue  * lengthSkyCam)) / (2 * (lengthValue - lengthSkyCam));
        pZ = ((r4Length * r4Length) - (r3Length * r3Length) + (widthValue * widthValue)-(widthValue  * widthSkyCam)) / (2 * (lengthValue - lengthSkyCam));

        aux1 = r1Length * r1Length;
        aux2 = pX - (lengthSkyCam / 2); 
        aux3 = pZ - (widthSkyCam / 2);

        pY = (heightValue) - (heightSkyCam / 2) - (float)(Math.Sqrt((aux1) - (aux2 * aux2) - (aux3 * aux3)));
        

        //Debug Valores en [mm]
        // Debug.Log("Pto real de x: "pX);
        // Debug.Log("Pto real de z: "pY);
        // Debug.Log("Pto real de y: "pZ);
    } 

    // Calcula la diferencia entre los puntos sP(x, y, z) y p(x, z y)
    // -d(x, y , z) = sP(x, y, z) - p(x, z y)
    public void calculateDeltaForDistance()
    {
        calculateDeltaX();
        calculateDeltaY();
        calculateDeltaZ();
    }

    public void calculateDeltaX()
    {
        if (double.IsNaN(pX) || double.IsNaN(sPX))
        {
            dX = 0;
            return;
        }

        dX = sPX - pX;
        return;
    }

    public void calculateDeltaY()
    {
        if (double.IsNaN(pY) || double.IsNaN(sPY))
        {
            dY = 0;
            return;
        }

        dY = sPY - pY;
        return;
    }   

    public void calculateDeltaZ()
    {
        if (double.IsNaN(pZ) || double.IsNaN(sPZ))
        {
            dZ = 0;
            return;
        }

        dZ = sPZ - pZ;
        return;
    }       
    
    public void calculateRopeLength()
    {
        newRopeLength = (float)Math.Sqrt((dX*dX)+(dY*dY)+(dZ*dZ));
    }

    public void calculateTime()
    {
        time = newRopeLength / currentSpeed;
    }
    
    public void calculateMotorVelocities()
    {
        // if (float.IsNaN(time))
        // {
        //     v1 = 0;
        //     v2 = 0;
        //     v3 = 0;
        //     v4 = 0;
        //     return;
        // }

        // Velocidades de los 4 motores en [mm/s]
        // v1 = Math.Round((dR1 / time), MidpointRounding.AwayFromZero);
        // v2 = Math.Round((dR2 / time), MidpointRounding.AwayFromZero);
        // v3 = Math.Round((dR3 / time), MidpointRounding.AwayFromZero);
        // v4 = Math.Round((dR4 / time), MidpointRounding.AwayFromZero);

        // Velocidades de prueba seteadas en 50 mm/s        
        v1 = (dR1 >= 10) ? 50 : (dR1 <= -10) ? -50 : 0;
        v2 = (dR2 >= 10) ? 50 : (dR2 <= -10) ? -50 : 0;
        v3 = (dR3 >= 10) ? 50 : (dR3 <= -10) ? -50 : 0;
        v4 = (dR4 >= 10) ? 50 : (dR4 <= -10) ? -50 : 0;

        //Debug.Log("Velocidad 1: "+ v1);
        //Debug.Log("Velocidad 2: "+ v2);
        //Debug.Log("Velocidad 3: "+ v3);
        //Debug.Log("Velocidad 4: "+ v4);
        return;
    }
    
    // Funcion encargada de establecer las direcciones de cada motor
    // de acuerdo al signo de cada velocidad calculada
    public void setMotorsDirections()
    {
        motorsDirections[0] = v1 >= 0 ? "F" : "R";
        motorsDirections[1] = v2 >= 0 ? "F" : "R";
        motorsDirections[2] = v3 >= 0 ? "F" : "R";
        motorsDirections[3] = v4 >= 0 ? "F" : "R";
        return;

        // Debug Direccion de motores
        // Debug.Log("Dir Motor 1: " + motorsDirections[0]);
        // Debug.Log("Dir Motor 2: " + motorsDirections[1]);
        // Debug.Log("Dir Motor 3: " + motorsDirections[2]);
        // Debug.Log("Dir Motor 4: " + motorsDirections[3]);
    }

    public void showUI()
    {
        // UI Unity x,y,z
        sPXText.SetText("SPx: " + (sPX).ToString("N0") + " mm");
        sPYText.SetText("SPy: " + (sPY).ToString("N0") + " mm");
        sPZText.SetText("SPz: " + (sPZ).ToString("N0") + " mm");

        // UI unity rope lengths
        r1sPText.SetText("SP1: " + (r1sP).ToString("N0") + " mm");
        r2sPText.SetText("SP2: " + (r2sP).ToString("N0") + " mm");
        r3sPText.SetText("SP3: " + (r3sP).ToString("N0") + " mm");
        r4sPText.SetText("SP4: " + (r4sP).ToString("N0") + " mm");

        // UI real rope lengths
        r1LengthText.SetText("R1: " + (r1Length).ToString("N0") + " mm");
        r2LengthText.SetText("R2: " + (r2Length).ToString("N0") + " mm");
        r3LengthText.SetText("R3: " + (r3Length).ToString("N0") + " mm");
        r4LengthText.SetText("R4: " + (r4Length).ToString("N0") + " mm");

        // UI real x,y,z
        pXText.SetText("Rx: " + (pX).ToString("N2") + " mm");
        pYText.SetText("Ry: " + (pY).ToString("N2") + " mm");
        pZText.SetText("Rz: " + (pZ).ToString("N2") + " mm");

        distanceText.SetText("D: " + (newRopeLength).ToString("N0") + " mm");

        timeText.SetText("T: " + (time).ToString("N0") + " s");
        
        // UI motor velocities
        v1Text.SetText("V1: " + (v1).ToString("N2") + " mm/s");
        v2Text.SetText("V2: " + (v2).ToString("N2") + " mm/s");
        v3Text.SetText("V3: " + (v3).ToString("N2") + " mm/s");
        v4Text.SetText("V4: " + (v4).ToString("N2") + " mm/s");
    }

    private IEnumerator sendTimeToLivePeriodically()
    {
        while (ArduinoController.isSerialConnEstablished)
        {
            ArduinoController.Instance.SendValue("*");
            yield return new WaitForSeconds(0.25f);
        }
    }

    /* Funcion encargada de mandar los comandos requeridos por ArgosUC
    */
    private IEnumerator sendCommands()
    {
        string l_lastPayload = "";
        while (ArduinoController.isSerialConnEstablished)
        {
            //Debug.Log("Send commands 2");
            string payload = "S50," + Math.Round(r1sP).ToString() + "S50," + Math.Round(r2sP).ToString() + "S50," + Math.Round(r3sP).ToString() + "S50," + Math.Round(r4sP).ToString() + "*";
            if (!payload.Equals(l_lastPayload))
            {
                l_lastPayload = payload;
                Debug.Log("Enviando: " + payload);
                ArduinoController.Instance.SendValue(payload); 
            } 
            //Debug.Log("DATOS ENVIADOS: " + payload);
            yield return new WaitForSeconds(1f);
        }
    }
}
