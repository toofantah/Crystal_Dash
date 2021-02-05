using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Atatch this script in the GameObject parent of the dscripts GameObjects childs you do not want to destroy on loading scenessdadssdsdsdsaasd
/// </summary>
public class Wave_DoNotDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
}
