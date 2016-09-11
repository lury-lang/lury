using Lury.Core.Runtime;
using NUnit.Framework;

namespace Unittest
{
    [Parallelizable]
    [TestFixture]
    public class LuryContextTest
    {
        [Test]
        public void CtorTest()
        {
            var context1 = new LuryContext();
            var context2 = new LuryContext(context1);

            Assert.That(context1.Parent, Is.Null);
            Assert.That(context2.Parent, Is.EqualTo(context1));
        }

        [Test]
        public void AssignTest()
        {
            var context1 = new LuryContext();
            var context2 = new LuryContext(context1);
            var context3 = new LuryContext(context2);
            var object1 = new LuryObject(null, null, 1);
            var object2 = new LuryObject(null, null, 2);
            var object3 = new LuryObject(null, null, 3);
            var object4 = new LuryObject(null, null, 4);

            context1["foo"] = object1;
            context2["bar"] = object2;
            context3["hoge"] = object3;

            Assert.That(context1.Has("foo"));
            Assert.That(context2.Has("foo"));
            Assert.That(context3.Has("foo"));

            Assert.That(context2.Has("bar"));
            Assert.That(context3.Has("bar"));

            Assert.That(context3.Has("hoge"));

            Assert.That(context3.Has("fuga"), Is.False);

            context3["foo"] = object4;
            Assert.That(context3.Has("foo"));
            Assert.That(context1.Has("foo"));
            Assert.That(context3["foo"], Is.EqualTo(context1["foo"]).And.EqualTo(object4));
        }
    }
}
