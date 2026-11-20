using UnityEngine;
using TMPro;
 
public class ScoreCounter : MonoBehaviour
{
    public static ScoreCounter Instance;
 
    public TMP_Text label;
    public string prefix = "Destroyed: ";
 
    int count;
 
    public int Count => count;
 
    void Awake()
    {
        Instance = this;
        if (label == null) label = GetComponent<TMP_Text>();
        UpdateLabel();
    }
 
    public void Add(int amount = 1)
    {
        count += amount;
        UpdateLabel();
    }
 
    void UpdateLabel()
    {
        if (label != null) label.text = prefix + count;
    }
}