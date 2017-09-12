using UnityEngine;
using System.Collections;

public class AlignProjectionManager : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    Canvas mainCanvas;

    [SerializeField]
    ProjectionAlignment align;

    int[] projectorNums;
    int totalProjectorNum;
    int screenIndex;
    public int ScreenIndex { get { return screenIndex; } }
    Vector2 perScreenResolution;
    public Vector2 PerScreenResolution { get { return perScreenResolution; } }
    Vector2 fullScreenResolution;
    public Vector2 FullScreenResolution { get { return fullScreenResolution; } }
    public int CurrentProjectorNum { get { return projectorNums[screenIndex - 1]; } }
    float stageWidth;
    public float StageWidth { get { return stageWidth; } }
    Vector3 stageSize;

    float cameraWidth
    {
        get
        {
            float height = 2f * mainCamera.orthographicSize;
            return height * mainCamera.aspect;
        }
    }


    public GridPattern gridPattern;
    public PointGrid pointGrid;

    private void Awake()
    {
        LoadCommandLine();
        Initialize();
    }

    public void Initialize()
    {
        for (int i = 0; i < projectorNums.Length; i++)
        {
            totalProjectorNum += projectorNums[i];
        }

        SetStage();
        SetCanvas();
        SetCamera();


        gridPattern.Generate(FullScreenResolution.x);
        pointGrid.Generate(FullScreenResolution.x);
    }

    private void SetStage()
    {
        float height = 2f * mainCamera.orthographicSize;

        stageWidth = height * ((perScreenResolution.x * totalProjectorNum) / perScreenResolution.y);

        fullScreenResolution = new Vector2(perScreenResolution.x * totalProjectorNum, perScreenResolution.y);

        stageSize = new Vector3(stageWidth, 2, mainCamera.farClipPlane);
    }

    private void SetCanvas()
    {
        if (mainCanvas == null)
            return;

        mainCanvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        mainCanvas.GetComponent<Canvas>().worldCamera = mainCamera;
        mainCanvas.GetComponent<RectTransform>().sizeDelta = fullScreenResolution;
        mainCanvas.transform.localScale = Vector3.one * (stageWidth / fullScreenResolution.x);
        mainCanvas.transform.localPosition = new Vector3(0, 1, stageSize.z);
    }

    private void SetCamera()
    {
        if (mainCamera == null)
            return;

        mainCamera.orthographicSize = 1;
        mainCamera.nearClipPlane = -1;

        var screenIdx = screenIndex;
        var projectorNum = projectorNums[screenIdx - 1];

        perScreenResolution = new Vector2(perScreenResolution.x * projectorNum, perScreenResolution.y);

        float x = -(stageWidth / 2f);
        float offset_x = 0;
        for (int i = 0; i < screenIdx; i++)
        {
            var ww = (float)projectorNums[i] / (float)totalProjectorNum;

            if (screenIdx > 1 && i < screenIdx - 1)
            {
                offset_x += (stageWidth * ww);
            }
            else
            {
                offset_x += (stageWidth * (cameraWidth / stageWidth)) / 2f;
            }
        }

        x += offset_x;

        mainCamera.transform.localPosition = new Vector3(x, mainCamera.transform.localPosition.y, 0);
    }

    private void LoadCommandLine()
    {
        string[] args = System.Environment.GetCommandLineArgs();

        if (Application.isEditor)
            args = new string[] { "-ScreenIndex", "1", "-ProjectorNums", "1:4:1", "-ScreenResolution", "1920:1080" };

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];

            if (arg.Contains("-ProjectorNums"))
            {
                var str_v = args[i + 1].Split(':');
                var projectorNums = new int[str_v.Length];
                for (int j = 0; j < str_v.Length; j++)
                {
                    var v = 0;
                    int.TryParse(str_v[j], out v);
                    projectorNums[j] = v;
                }

                this.projectorNums = projectorNums;
            }

            if (arg.Contains("-ScreenResolution"))
            {
                var str_v = args[i + 1].Split(':');

                var resolution = new Vector2();
                var x = 0;
                int.TryParse(str_v[0], out x);
                resolution.x = x;
                var y = 0;
                int.TryParse(str_v[1], out y);
                resolution.y = y;

                this.perScreenResolution = resolution;
            }

            if (arg.Contains("-ScreenIndex"))
            {
                var str_v = args[i + 1];
                var index = 0;
                int.TryParse(str_v, out index);

                this.screenIndex = index;
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(new Vector3(transform.position.x, transform.position.y + stageSize.y / 2, transform.position.z + stageSize.z / 2), stageSize);
    }
}

enum ProjectionAlignment
{
    Horizontal,
    Vertical
}
