using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BoilingEquipment : StorageEquipment
{
    private BoilingType boilingType;
    public enum BoilingType
    {
        mrgvali,
        samkutxedi
    }

    public BoilingEquipment()
    {
        windowTitle = "Boiling Flask";
        // hasInputs = true;
    }


    public override string getResult()
    {
        float input1Value = 0;

        if (input)
        {
            string input1Raw = input.getResult();
            float.TryParse(input1Raw, out input1Value);
        }

        // switch (storageType)
        // {
        //     case CalculationType.Add:
        //         break;
        //     case CalculationType.Sub:
        //         result = (input1Value - input2Value).ToString();
        //         break;
        //     case CalculationType.Mul:
        //         result = (input1Value * input2Value).ToString();
        //         break;
        //     case CalculationType.Div:
        //         result = (input1Value / input2Value).ToString();
        //         break;
        // }
		return (input1Value * Volume).ToString();
    }

}
