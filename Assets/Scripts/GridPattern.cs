using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridPattern : MonoBehaviour {

    [SerializeField]
    Image patternPrefab;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }
    public void Generate(float width)
    {
        var count = Mathf.CeilToInt(width / patternPrefab.preferredWidth);

        for (int i = 0; i < count; i++)
        {
            var grid = Instantiate(patternPrefab);
            grid.transform.SetParent(this.transform);
            grid.transform.localPosition = Vector3.zero;
            grid.transform.localScale = Vector3.one;
        }
    }
}
