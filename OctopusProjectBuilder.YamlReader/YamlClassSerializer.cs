using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Yaml;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlClassSerializer
    {
        private readonly YamlConfig _config;

        public YamlClassSerializer(YamlConfig config)
        {
            _config = config;
        }

        public YamlClassSerializer() : this(YamlNode.DefaultConfig)
        {
        }

        public void Serialize<T>(Stream stream, T model)
        {
            ToNode(model).ToYaml(stream, _config);
        }

        public T Deserialize<T>(Stream stream)
        {
            return FromNode<T>(YamlNode.FromYaml(stream, _config));
        }

        private T FromNode<T>(YamlNode[] nodes)
        {
            return (T)FromNode(typeof(T), nodes.Single());
        }

        private object FromNode(Type type, YamlNode node)
        {
            if ((node as YamlScalar)?.Tag == "!!null")
                return null;

            if (type == typeof(string))
                return ConvertNode<YamlScalar, string>(node, s => s.Value);
            if (type == typeof(int))
                return ConvertNode<YamlScalar, int>(node, s => int.Parse(s.Value, CultureInfo.InvariantCulture));
            if (type == typeof(double))
                return ConvertNode<YamlScalar, double>(node, s => double.Parse(s.Value, CultureInfo.InvariantCulture));
            if (type == typeof(bool))
                return ConvertNode<YamlScalar, bool>(node, s => bool.Parse(s.Value));
            if (typeof(Enum).IsAssignableFrom(type))
                return ConvertNode<YamlScalar, object>(node, s => Enum.Parse(type, s.Value, true));
            if (typeof(IDictionary<,>).IsAssignableFrom(type))
                return ConvertNode<YamlMapping, object>(node, n => NodeToDictionary(type, n));
            if (type.IsArray)
                return ConvertNode<YamlSequence, object>(node, n => NodeToArray(type, n));

            if (type.IsValueType)
                throw new InvalidOperationException($"Type {type} is not supported");

            return ConvertNode<YamlMapping, object>(node, n => NodeToClass(type, n));
            ;
        }

        private object NodeToClass(Type type, YamlMapping node)
        {
            var instance = Activator.CreateInstance(type);
            var properties = type.GetProperties().ToDictionary(p => p.Name);
            foreach (var pair in node)
            {
                var propName = ConvertToPropertyName(type, pair.Key);
                PropertyInfo prop;
                if (!properties.TryGetValue(propName, out prop))
                    throw new InvalidOperationException($"Class {type} does not have property {propName}");
                try
                {
                    prop.SetValue(instance, FromNode(prop.PropertyType, pair.Value));
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException($"Unable to deserialize {type} property {propName}: {e.Message}", e);
                }
            }
            return instance;
        }

        private string ConvertToPropertyName(Type type, YamlNode key)
        {
            try
            {
                return ConvertNode<YamlScalar, string>(key, n => n.Value);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to convert node to class {type} property: {e.Message}", e);
            }
        }

        private object NodeToArray(Type type, YamlSequence node)
        {
            var array = Array.CreateInstance(type.GetElementType(), node.Count);
            for (int i = 0; i < node.Count; ++i)
                array.SetValue(node[i], i);
            return array;
        }

        private object NodeToDictionary(Type type, YamlMapping mappings)
        {
            var dictionary = (IDictionary)Activator.CreateInstance(type);
            var genericArguments = type.GetGenericArguments();
            foreach (var pair in mappings)
                dictionary.Add(FromNode(genericArguments[0], pair.Key), FromNode(genericArguments[1], pair.Value));
            return dictionary;
        }


        private TValue ConvertNode<TNode, TValue>(YamlNode node, Func<TNode, TValue> func) where TNode : YamlNode
        {
            if (!(node is TNode))
                throw new InvalidOperationException($"{typeof(TValue)} requires node of type {typeof(TNode).Name}, while got {node.GetType().Name}");
            try
            {
                return func((TNode)node);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to deserialize type {typeof(TValue)}: {e.Message}", e);
            }
        }

        private YamlNode ToNode(object model)
        {
            if (model == null)
                return new YamlScalar(null);
            if (model is string)
                return new YamlScalar((string)model);
            if (model is int)
                return new YamlScalar((int)model);
            if (model is double)
                return new YamlScalar((double)model);
            if (model is bool)
                return new YamlScalar((bool)model);
            if (model is Enum)
                return new YamlScalar(model.ToString());
            if (typeof(IDictionary<,>).IsAssignableFrom(model.GetType()))
                return DictionaryToNode((IDictionary)model);
            if (model.GetType().IsArray)
                return new YamlSequence(((IEnumerable)model).Cast<object>().Select(ToNode).ToArray());
            if (model.GetType().IsValueType)
                throw new InvalidOperationException($"Type {model.GetType()} is not supported");

            return ClassToNode(model);
        }

        private YamlNode DictionaryToNode(IDictionary model)
        {
            var nodes = new List<YamlNode>();
            var e = model.GetEnumerator();
            while (e.MoveNext())
            {
                nodes.Add(ToNode(e.Key));
                nodes.Add(ToNode(e.Value));
            }
            return new YamlMapping(nodes.ToArray());
        }

        private YamlNode ClassToNode(object model)
        {
            var properties = new List<YamlNode>();
            foreach (var p in model.GetType().GetProperties().OrderBy(p => p.Name))
            {
                properties.Add(new YamlScalar(p.Name));
                properties.Add(ToNode(p.GetValue(model)));
            }
            return new YamlMapping(properties.ToArray());
        }
    }

}
