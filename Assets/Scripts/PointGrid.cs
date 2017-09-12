using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointGrid : MonoBehaviour {

    public GameObject text;

    // Use this for initialization
    void Start()
    {
    }

    public void Generate(float width)
    {
        float totalWidth = width;
        float w = 1920 / 4;
        for (int i = 0; i < totalWidth / w; i++)
        {
            var t = Instantiate(text);
            t.GetComponent<RectTransform>().sizeDelta = new Vector2(w, 100f);
            t.GetComponentInChildren<Text>().text = ((i * w) / totalWidth).ToString();
            t.transform.SetParent(this.transform);
            t.transform.localScale = Vector3.one;
        }
    }
}
