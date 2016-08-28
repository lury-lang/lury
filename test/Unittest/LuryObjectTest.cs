using System;
using Lury.Core.Error;
using Lury.Core.Runtime;
using NUnit.Framework;

namespace Unittest
{
    [TestFixture]
    public class LuryObjectTest
    {
        [Test]
        public void AssignTest()
        {
            var baseObject = new LuryObject(null, null);
            var classObject = new LuryObject(baseObject, null, "BaseClass");
            var luryObject = new LuryObject(baseObject, classObject);

            var attributeObject1 = new LuryObject(baseObject, null, 1);
            var attributeObject2 = new LuryObject(baseObject, null, 2);
            var attributeObject3 = new LuryObject(baseObject, null, 3);
            var attributeObject4 = new LuryObject(baseObject, null, 4);

            luryObject.Assign("foo", attributeObject1);
            classObject.Assign("bar", attributeObject2);
            baseObject.Assign("hoge", attributeObject3);
            baseObject.Assign("fuga", attributeObject4);

            Assert.True(luryObject.Has("foo"));
            Assert.True(luryObject.Has("hoge"));
            Assert.True(luryObject.Has("fuga"));
            
            Assert.True(baseObject.Has("hoge"));
            Assert.True(baseObject.Has("fuga"));

            Assert.False(luryObject.Has("bar"));
            Assert.False(luryObject.Has("baz"));

            Assert.False(baseObject.Has("foo"));
            Assert.False(baseObject.Has("bar"));
        }

        [Test]
        public void FetchTest()
        {
            var baseObject = new LuryObject(null, null);
            var classObject = new LuryObject(baseObject, null, "BaseClass");
            var luryObject = new LuryObject(baseObject, classObject);

            var attributeObject1 = new LuryObject(baseObject, null, 1);
            var attributeObject2 = new LuryObject(baseObject, null, 2);
            var attributeObject3 = new LuryObject(baseObject, null, 3);
            var attributeObject4 = new LuryObject(baseObject, null, 4);

            luryObject.Assign("foo", attributeObject1);
            classObject.Assign("bar", attributeObject2);
            baseObject.Assign("hoge", attributeObject3);
            baseObject.Assign("fuga", attributeObject4);

            Assert.AreEqual(attributeObject1, luryObject.Fetch("foo"));
            Assert.AreEqual(attributeObject3, luryObject.Fetch("hoge"));
            Assert.AreEqual(attributeObject4, luryObject.Fetch("fuga"));

            Assert.AreEqual(attributeObject3, baseObject.Fetch("hoge"));
            Assert.AreEqual(attributeObject4, baseObject.Fetch("fuga"));

            Assert.Throws<AttributeNotDefinedException>(() => luryObject.Fetch("bar"));
            Assert.Throws<AttributeNotDefinedException>(() => luryObject.Fetch("baz"));

            Assert.Throws<AttributeNotDefinedException>(() => baseObject.Fetch("foo"));
            Assert.Throws<AttributeNotDefinedException>(() => baseObject.Fetch("bar"));
        }

        [Test]
        public void FreezeTest()
        {
            var baseObject = new LuryObject(null, null);
            var classObject = new LuryObject(baseObject, null, "BaseClass");
            var luryObject = new LuryObject(baseObject, classObject);

            var attributeObject = new LuryObject(baseObject, null, 1);

            luryObject.Assign("foo", attributeObject);
            Assert.IsFalse(luryObject.IsFrozen);
            Assert.IsFalse(attributeObject.IsFrozen);

            luryObject.Freeze();
            Assert.IsTrue(luryObject.IsFrozen);
            Assert.IsFalse(attributeObject.IsFrozen);
            Assert.Throws<CantModifyException>(() => luryObject.Assign("foo", attributeObject));
        }
    }
}
