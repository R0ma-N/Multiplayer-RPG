using UnityEngine;
using UnityEngine.UI;

public class StatItem : MonoBehaviour
{
    [SerializeField] Text value;

    public void ChangeStat(int stat)
    {
        value.text = stat.ToString();
    }
}
