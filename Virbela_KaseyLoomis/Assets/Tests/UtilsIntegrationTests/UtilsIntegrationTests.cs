using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class UtilsIntegrationTests
{
    [UnityTest]
    public IEnumerator MeshRendererExtensions_SetStandardShaderColorTest()
    {
        const float tolerance = 0.001f;

        TestSetup.CreateTestCam();
        var ball = GameObject.Instantiate(TestSetup.CreateObjectPrefab("test ball"));
        ball.SetActive(true);
        yield return null;

        Color magicPink = Color.magenta;
        MeshRenderer mr = ball.GetComponentInChildren<MeshRenderer>(true);
        verb.MeshRendererExtensions.SetStandardShaderColor(mr, magicPink);

        yield return null;

        MaterialPropertyBlock mb = new MaterialPropertyBlock();
        mr.GetPropertyBlock(mb);
        Color testColor = mb.GetColor("_Color");

        Assert.AreEqual(magicPink.r, testColor.r, tolerance);
        Assert.AreEqual(magicPink.g, testColor.g, tolerance);
        Assert.AreEqual(magicPink.b, testColor.b, tolerance);

        yield return null;
    }

    public class TestSetup
    {
        public static Camera testCam;

        public static void CreateTestCam()
        {
            if (testCam != null)
                return;

            var test = new GameObject("test camera ");
            testCam = test.AddComponent<Camera>();

            test.transform.localPosition = -test.transform.forward * 5f;
            test.transform.localPosition = test.transform.localPosition - test.transform.up * 3f;

            var light = new GameObject("test light ");
            Light l = test.AddComponent<Light>();
            l.type = LightType.Directional;
            light.transform.SetParent(test.transform);
        }

        public static Dictionary<string, GameObject> gameObjectsPrefabs = new Dictionary<string, GameObject>();

        public static GameObject CreateObjectPrefab(string name)
        {
            if (gameObjectsPrefabs.ContainsKey(name))
                return gameObjectsPrefabs[name];

            var test = new GameObject("test ojbect " + name);

            var type = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            type.transform.SetParent(test.transform);
            type.transform.localPosition = Vector3.zero;

            test.SetActive(false);

            gameObjectsPrefabs[name] = test;

            return test;
        }
    }
}
