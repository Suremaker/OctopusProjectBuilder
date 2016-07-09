using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Tests
{
    [TestFixture]
    public class SerializerTests
    {
        class Model
        {
            [YamlMember(Order = 1)]
            public string Text { get; set; }

            [DefaultValue(4)]
            [YamlMember(Order = 3)]
            public int Number { get; set; } = 4;
            public double Double { get; set; }
            public DateTime Date { get; set; }
            [YamlMember(Order = 2)]
            public int[] Array { get; set; }
            public Dictionary<string, Model> Dictionary { get; set; }
            public DateTimeKind Enum { get; set; }
            public Model Inner { get; set; }
        }

        [Test]
        public void It_should_serialize_and_deserialize_type()
        {
            var model = new Model
            {
                Text = "Abc",
                Number = 55,
                Double = 55.55,
                Date = DateTime.UtcNow,
                Enum = DateTimeKind.Utc,
                Inner = new Model { Text = "abc" },
                Array = new[] { 1, 2, 3 },
                Dictionary = new Dictionary<string, Model> { { "a", new Model { Text = "Def" } }, { "b", null } }
            };
            var actual = SerializeAndDeserialize(model);

            Assert.That(actual.Text, Is.EqualTo(model.Text));
            Assert.That(actual.Number, Is.EqualTo(model.Number));
            Assert.That(actual.Double, Is.EqualTo(model.Double));
            Assert.That(actual.Array, Is.EqualTo(model.Array));
            Assert.That(actual.Enum, Is.EqualTo(model.Enum));
            Assert.That(actual.Inner.Text, Is.EqualTo(model.Inner.Text));
            Assert.That(actual.Dictionary.Keys.ToArray(), Is.EqualTo(model.Dictionary.Keys.ToArray()));
            Assert.That(actual.Dictionary["a"].Text, Is.EqualTo(model.Dictionary["a"].Text));
            Assert.That(actual.Dictionary["b"], Is.EqualTo(model.Dictionary["b"]));
        }

        [Test]
        public void It_should_not_serialize_defaults()
        {
            string expected = "Text: abc\r\n";
            Assert.That(Serialize(new Model { Number = 4, Text = "abc" }), Is.EqualTo(expected));
        }

        [Test]
        public void It_should_throw_on_unknown_properties()
        {
            var ex = Assert.Throws<YamlException>(() => Deserialize<Model>("Text: abc\r\nUnknown: def\r\n"));
            Assert.That(ex.InnerException.Message, Is.StringContaining($"Property 'Unknown' not found on type '{typeof(Model)}'"));
        }

        [Test]
        public void It_should_serialize_data_in_order()
        {
            string expected = "Text: abc\r\nArray:\r\n- 1\r\n- 2\r\n- 3\r\nNumber: 0\r\n";
            Assert.That(Serialize(new Model { Text = "abc", Array = new[] { 1, 2, 3 }, Number = 0 }), Is.EqualTo(expected));
        }

        [Test]
        public void It_should_deserialize_defaults()
        {
            var actual = Deserialize<Model>("Text: abc");
            Assert.That(actual.Text, Is.EqualTo("abc"));
            Assert.That(actual.Number, Is.EqualTo(4));
            Assert.That(actual.Date, Is.EqualTo(DateTime.MinValue));
        }

        private static T SerializeAndDeserialize<T>(T model)
        {
            var yaml = Serialize(model);
            Console.WriteLine(yaml);
            return Deserialize<T>(yaml);
        }

        private static string Serialize<T>(T model)
        {
            var builder = new StringBuilder();

            using (var writer = new StringWriter(builder))
                new Serializer().Serialize(writer, model);

            return builder.ToString();
        }

        private static T Deserialize<T>(string yaml)
        {
            using (var reader = new StringReader(yaml))
                return new Deserializer().Deserialize<T>(reader);
        }
    }
}