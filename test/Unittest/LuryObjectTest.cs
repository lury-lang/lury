using System;
using Lury.Core.Error;
using Lury.Core.Runtime;
using NUnit.Framework;

namespace Unittest
{
    [Parallelizable]
    [TestFixture]
    public class LuryObjectTest
    {
        [Test]
        public void CtorTest()
        {
            var baseObject = new LuryObject(null, null, freeze: true);
            var classObject = new LuryObject(baseObject, null, "BaseClass");
            var luryObject = new LuryObject(baseObject, classObject);

            Assert.That(baseObject.BaseObject, Is.Null);
            Assert.That(baseObject.Class, Is.Null);
            Assert.That(baseObject.Value, Is.Null);
            Assert.That(baseObject.IsFrozen);

            Assert.That(classObject.BaseObject, Is.EqualTo(baseObject));
            Assert.That(classObject.Class, Is.Null);
            Assert.That(classObject.Value, Is.EqualTo("BaseClass"));
            Assert.That(classObject.IsFrozen, Is.False);

            Assert.That(luryObject.BaseObject, Is.EqualTo(baseObject));
            Assert.That(luryObject.Class, Is.EqualTo(classObject));
            Assert.That(luryObject.Value, Is.Null);
            Assert.That(luryObject.IsFrozen, Is.False);
        }

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

            Assert.That(luryObject.Has("foo"));
            Assert.That(luryObject.Has("hoge"));
            Assert.That(luryObject.Has("fuga"));
            
            Assert.That(baseObject.Has("hoge"));
            Assert.That(baseObject.Has("fuga"));

            Assert.That(luryObject.Has("bar"), Is.False);
            Assert.That(luryObject.Has("baz"), Is.False);

            Assert.That(baseObject.Has("foo"), Is.False);
            Assert.That(baseObject.Has("bar"), Is.False);
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
            var attributeObject5 = new LuryObject(baseObject, null, 5);

            luryObject.Assign("foo", attributeObject1);
            classObject.Assign("bar", attributeObject2);
            baseObject.Assign("hoge", attributeObject3);
            baseObject.Assign("fuga", attributeObject4);

            Assert.That(luryObject.Fetch("foo"), Is.EqualTo(attributeObject1));
            Assert.That(luryObject.Fetch("hoge"), Is.EqualTo(attributeObject3));
            Assert.That(luryObject.Fetch("fuga"), Is.EqualTo(attributeObject4));

            Assert.That(baseObject.Fetch("hoge"), Is.EqualTo(attributeObject3));
            Assert.That(baseObject.Fetch("fuga"), Is.EqualTo(attributeObject4));

            luryObject.Assign("hoge", attributeObject5);
            Assert.That(luryObject.Fetch("hoge"), Is.EqualTo(attributeObject5).And.Not.EqualTo(attributeObject3));

            Assert.That(() => baseObject.Fetch("bar"), Throws.TypeOf<AttributeNotDefinedException>());
            Assert.That(() => baseObject.Fetch("baz"), Throws.TypeOf<AttributeNotDefinedException>());

            Assert.That(() => baseObject.Fetch("foo"), Throws.TypeOf<AttributeNotDefinedException>());
            Assert.That(() => baseObject.Fetch("bar"), Throws.TypeOf<AttributeNotDefinedException>());
        }

        [Test]
        public void FreezeTest()
        {
            var baseObject = new LuryObject(null, null);
            var classObject = new LuryObject(baseObject, null, "BaseClass");
            var luryObject = new LuryObject(baseObject, classObject);

            var attributeObject = new LuryObject(baseObject, null, 1);

            luryObject.Assign("foo", attributeObject);
            Assert.That(luryObject.IsFrozen, Is.False);
            Assert.That(attributeObject.IsFrozen, Is.False);

            luryObject.Freeze();
            Assert.That(luryObject.IsFrozen);
            Assert.That(attributeObject.IsFrozen, Is.False);
            Assert.That(() => luryObject.Assign("foo", attributeObject), Throws.TypeOf<CantModifyException>());
        }
    }
}
