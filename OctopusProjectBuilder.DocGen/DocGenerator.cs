using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.DocGen
{
    public class DocGenerator
    {
        private readonly Dictionary<string, Type> _modelTypes;
        private readonly Type[] _indexedTypes;
        private readonly Type _modelRootType;

        public DocGenerator(Type modelRootType)
        {
            _modelRootType = modelRootType;
            _modelTypes = modelRootType.Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && !t.GetCustomAttributes<CompilerGeneratedAttribute>().Any() && t.Namespace.Contains("Model"))
                .ToDictionary(t => t.Name);
            _indexedTypes = _modelTypes.OrderBy(p => p.Key).Select(p => p.Value).ToArray();
        }

        public string Generate()
        {
            var builder = new StringBuilder();
            GenerateTableOfContents(builder);
            GenerateModelContent(builder);
            return builder.ToString();
        }

        private void GenerateModelContent(StringBuilder builder)
        {
            GenerateHeader(builder, "Model description");
            for (var i = 0; i < _indexedTypes.Length; ++i)
            {
                builder.AppendLine($"### <a name=\"{_indexedTypes[i].Name}\"></a>{i + 1}. {_indexedTypes[i].Name}").AppendLine();
                GenerateClassDescription(builder, _indexedTypes[i]);
                GenerateClassMembers(builder, _indexedTypes[i]);
                builder.AppendLine();
            }
        }

        private void GenerateClassMembers(StringBuilder builder, Type type)
        {
            builder.AppendLine("|Property|Type|Description|");
            builder.AppendLine("|--------|----|:----------|");
            foreach (var propertyInfo in type.GetProperties().OrderBy(p => p.GetCustomAttributes<YamlMemberAttribute>().Select(a => a.Order).FirstOrDefault()))
            {
                var name = propertyInfo.Name;
                var propertyType = GetPropertyTypeText(propertyInfo.PropertyType);
                var description = GetPropertyText(propertyInfo);
                builder.AppendLine($"|**{name}**|{propertyType}|{description}|");
            }
        }

        private string GetPropertyText(PropertyInfo propertyInfo)
        {
            var sb = new StringBuilder();

            sb.Append(GetDescription(propertyInfo)).Append(" ");

            if (propertyInfo.PropertyType.IsEnum)
            {
                AppendEnum(sb, propertyInfo.PropertyType);
            }
            else if (GetEnumerableTypes(propertyInfo.PropertyType).Any(t => t.IsEnum))
            {
                AppendEnum(sb, GetEnumerableTypes(propertyInfo.PropertyType).First(t => t.IsEnum));
            }

            sb.Append("Default value: **").Append(GetPropertyDefaultValueText(propertyInfo)).Append("**. ");

            return sb.ToString();
        }

        private static void AppendEnum(StringBuilder sb, Type @enum)
        {
            sb.Append("Possible values: **")
                    .AppendFormat(string.Join("**, **", Enum.GetNames(@enum)))
                    .Append("**. ");
        }

        private static string GetPropertyDefaultValueText(PropertyInfo propertyInfo)
        {
            var defaultValue = propertyInfo.GetCustomAttributes<DefaultValueAttribute>()
                .Select(a => a.Value?.ToString() ?? "null")
                .FirstOrDefault() ?? GetDefaultValueText(propertyInfo.PropertyType);
            return defaultValue;
        }

        private static string GetDefaultValueText(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type).ToString() : "null";
        }

        private static string GetDescription(MemberInfo memberInfo)
        {
            return memberInfo.GetCustomAttributes<DescriptionAttribute>(true).Select(a => a.Description).FirstOrDefault() ?? string.Empty;
        }

        private string GetPropertyTypeText(Type propertyType)
        {
            if (propertyType.IsArray)
                return GetPropertyTypeText(propertyType.GetElementType()) + "\\[\\]";
            if (propertyType.IsGenericType)
                return propertyType.GetGenericTypeDefinition().Name.Split('`')[0] + "<" + string.Join(", ", propertyType.GetGenericArguments().Select(GetPropertyTypeText)) + ">";
            if (_modelTypes.ContainsKey(propertyType.Name))
                return $"[{propertyType.Name}](#{propertyType.Name})";
            return propertyType.Name;
        }

        private void GenerateClassDescription(StringBuilder builder, Type type)
        {
            var description = GetDescription(type);
            if (description.Length > 0)
                builder.AppendLine(description).AppendLine();
        }

        private void GenerateTableOfContents(StringBuilder builder)
        {
            builder.AppendLine("# Configuration Manual v" + GetType().Assembly.GetName().Version.ToString(4));
            GenerateHeader(builder, "Table of contents");

            for (var i = 0; i < _indexedTypes.Length; ++i)
                builder.AppendLine($"{i + 1}. [{_indexedTypes[i].Name}](#{_indexedTypes[i].Name})");
            builder.AppendLine();

            builder.AppendLine($"To start with model root type, please see: [{_modelRootType.Name}](#{_modelRootType.Name})");
        }

        private void GenerateHeader(StringBuilder builder, string header)
        {
            builder.Append("## ").AppendLine(header).AppendLine();
        }

        public static IEnumerable<Type> GetEnumerableTypes(Type type)
        {
            if (type.IsInterface)
            {
                if (type.IsGenericType
                    && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    yield return type.GetGenericArguments()[0];
                }
            }
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    yield return intType.GetGenericArguments()[0];
                }
            }
        }
    }
}