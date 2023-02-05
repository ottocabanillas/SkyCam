using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
	Esta clase la vamos a usar para poner el modelo matematico y calcular las velocidades de las cuerdas
	Esto lo cree para probar de mandar valores a la placa Arduino. Los calculos que hice aca son solamente
	de prueba, los saque de ChatGPT y es algo simple y basico que use para probar y nada mas. 
**/
public class RopeSpeedCalculator {

	public static float fixedTimeStep = 0.02f;

	public static float CalculateRopeSpeed(Vector3 displacement, Vector3 rightTopVertex, Vector3 previousCubePosition)
   {
        // Calculate the displacement of the cube in meters
        float displacementInMeters = displacement.magnitude;
        // Calculate the speed at which the cube is moving in meters per second
        float cubeSpeed = displacementInMeters / fixedTimeStep;
        // Inverse kinematics calculations to determine the rope release/contract speed based on the cube speed
        float ropeSpeed = cubeSpeed * 1; 
		ropeSpeed *= Mathf.Sign(Vector3.Dot(displacement.normalized, (rightTopVertex - previousCubePosition).normalized));
   		return ropeSpeed;
   }
}