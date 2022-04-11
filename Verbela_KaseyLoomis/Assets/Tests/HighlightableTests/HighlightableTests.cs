using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace verb.tests
{
    public class HighlightableTests
    {
        [UnityTest]
        public IEnumerator MeshRendererExtensions_SetStandardShaderColorTest()
        {
            const float tolerance = 0.001f;

            Color defaultColor = Color.magenta;
            Color activeColor = Color.cyan;

            TestSetup.CreateTestCam();
            verb.HighlightColorData colorData = TestSetup.CreateDataPrefab<HighlightColorData>("color data");
            colorData.defaultColor = defaultColor;
            colorData.highlightColor = activeColor;

            var parent = new GameObject("base");
            parent.SetActive(false);

            var ball = GameObject.Instantiate(TestSetup.CreateObjectPrefab("test ball"), parent.transform);
            var highlighter = ball.AddComponent<StandardHighlightableMonoBehaviour>();
            highlighter.stateColors = colorData;

            MeshRenderer mr = ball.GetComponentInChildren<MeshRenderer>(true);

            ball.SetActive(true);
            parent.SetActive(true);

            yield return null;

            //test active
            highlighter.EnableHighlight();

            yield return null;

            {
                MaterialPropertyBlock mb = new MaterialPropertyBlock();
                mr.GetPropertyBlock(mb);
                Color testColor = mb.GetColor("_Color");

                Assert.AreEqual(activeColor.r, testColor.r, tolerance);
                Assert.AreEqual(activeColor.g, testColor.g, tolerance);
                Assert.AreEqual(activeColor.b, testColor.b, tolerance);
            }

            //test default
            highlighter.ClearHighlight();

            yield return null;

            {
                MaterialPropertyBlock mb = new MaterialPropertyBlock();
                mr.GetPropertyBlock(mb);
                Color testColor = mb.GetColor("_Color");

                Assert.AreEqual(defaultColor.r, testColor.r, tolerance);
                Assert.AreEqual(defaultColor.g, testColor.g, tolerance);
                Assert.AreEqual(defaultColor.b, testColor.b, tolerance);
            }

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

            public static Dictionary<string, ScriptableObject> gameDataPrefabs = new Dictionary<string, ScriptableObject>();

            public static T CreateDataPrefab<T>(string name)
                where T: ScriptableObject
            {
                if (gameDataPrefabs.ContainsKey(name))
                    return gameDataPrefabs[name] as T;

                var test = ScriptableObject.CreateInstance<T>();

                return test;
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
}