using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SushiTypeSc : MonoBehaviour
{
    public enum SushiType
    {
        Tamago,
        Ebi,
        Ika,
        Maguro,
        Ikura
    }

    public SushiType type;
}
