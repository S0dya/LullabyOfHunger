using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static int curSceneId = 1;
    public static SceneNameEnum curScene;

    public static float[] musicStats = new float[3] {1, 1, 1};

    //game
    public static float camFov;
    public static bool firstTime;

    public static bool showBlood;
    //in game
    public static int curMagsN = 1;
    public static bool hasGasMask;

}
