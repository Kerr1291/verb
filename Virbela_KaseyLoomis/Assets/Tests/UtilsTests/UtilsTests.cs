using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace verb.tests
{
    public class UtilsTests
    {
        [Test]
        [NUnit.Framework.TestCase]
        public void ToVector3XZ_TypeTest()
        {
            Vector2 init = Vector2.zero;
            object result = Vector2Extensions.ToVector3XZ(init, 1f);

            Assert.IsAssignableFrom(typeof(Vector3), result);            
        }

        [Test]
        [NUnit.Framework.TestCase(1f, ExpectedResult = 1f)]
        public float ToVector3XZ_ValueTest(float value)
        {
            Vector2 init = Vector2.zero;
            Vector3 result = Vector2Extensions.ToVector3XZ(init, 1f);

            return result.y;
        }
    }
}