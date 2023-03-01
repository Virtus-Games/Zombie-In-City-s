using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectControl : MonoBehaviour
{
    [SerializeField] List<GameObject> effects;

    public void EnableEffect(string effectName)
    {
        foreach (var effect in effects)
        {
            if (effect.name.Equals(effectName))
            {
                effect.SetActive(true);
                break;
            }
        }
    }
}
