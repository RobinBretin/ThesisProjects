using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PID_Controller : MonoBehaviour
{
    public enum DerivativeMeasurement
    {
        Velocity,
        ErrorOfChangeRate
    }

    public float proportionalGain;
    public float integralGain;
    public float derivativeGain;

    public float integrationStored;
    public float integrationSaturation;

    public float errorLast;
    public float valueLast;
    public DerivativeMeasurement derivativeMeasurement;

    public bool derivativeInitialized;

    
    public float UpdatePID(float dt, float currentValue, float targetValue)
    {
        float error = targetValue - currentValue;
        
        // Calculate P term
        float P = proportionalGain * error;

        //Calculate D term
        float errorRateOfChange = (error - errorLast) / dt;
        errorLast = error;

        float valueRateOfChange = (currentValue - valueLast) / dt;
        valueLast = currentValue;

        //Chose D term to use
        float deriveMeasure=0;

        if (derivativeInitialized)
        {
            if (derivativeMeasurement == DerivativeMeasurement.Velocity)
            {
                deriveMeasure = -valueRateOfChange;
            }
            else
            {
                deriveMeasure = errorRateOfChange;
            }
        }
        else
        {
            derivativeInitialized = true;
        }
        
        float D = derivativeGain * deriveMeasure;

        //Calculate I
        integrationStored = Mathf.Clamp(integrationStored + (error * dt), -integrationSaturation, integrationSaturation);

        float I = integralGain * integrationStored;


        float result = P + I+ D;

        return result;
    }


    public void Reset()
    {
        derivativeInitialized = false;
    }
}
