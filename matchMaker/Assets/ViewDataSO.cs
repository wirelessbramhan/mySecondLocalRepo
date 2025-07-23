using UnityEngine;

[CreateAssetMenu]
public class ViewDataSO : ScriptableObject
{
    public RectOffset Padding;
    public float Spacing = 16.0f;

    public int RowCount, ColCount;
}
