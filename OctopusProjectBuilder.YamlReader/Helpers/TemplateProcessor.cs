using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using OctopusProjectBuilder.YamlReader.Model;
using OctopusProjectBuilder.YamlReader.Model.Templates;

namespace OctopusProjectBuilder.YamlReader.Helpers
{
    public static class TemplateProcessor
    {
        private static readonly Regex ParameterReplacement = new Regex(@"(^|[^\\])(\\\\)*(\$\{([A-Za-z0-9_]+)\})", RegexOptions.Compiled);

        public static void ApplyTemplate<TModel, TTemplate>(this TModel model, params TTemplate[] templates) where TTemplate : TModel, IYamlTemplate where TModel : IYamlTemplateBased
        {
            if (model.UseTemplate == null)
                return;
            if (string.IsNullOrWhiteSpace(model.UseTemplate.Name))
                throw new InvalidOperationException($"{typeof(TModel)} has UseTemplate property but template name is not specified.");

            var template = templates.EnsureNotNull().FirstOrDefault(t => t.TemplateName == model.UseTemplate.Name);


            if (template == null)
                throw new InvalidOperationException($"No template with name '{model.UseTemplate.Name}' has been found.");

            var modelParameters = model.UseTemplate.Parameters.EnsureNotNull().ToDictionary(p=>p.Name,p=>p.Value);

            VerifyTemplateParameters(template, modelParameters);
            try
            {
                ApplyTemplate(model, DeepCopy(template), modelParameters);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException($"Unable to apply template {template.TemplateName}: {e.Message}", e);
            }
        }

        private static T DeepCopy<T>(T template)
        {
            using (var memory = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memory, template);
                memory.Seek(0, SeekOrigin.Begin);
                return (T)formatter.Deserialize(memory);
            }
        }

        private static void ApplyTemplate<T>(T model, T template, Dictionary<string, string> parameters)
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                var modelValue = propertyInfo.GetValue(model);
                if (modelValue == null)
                    propertyInfo.SetValue(model, MaterializeTemplateValue(propertyInfo.GetValue(template), parameters));
            }
        }

        private static object MaterializeTemplateValue(object value, Dictionary<string, string> parameters)
        {
            if (value == null)
                return null;
            if (value is string)
                return ApplyParameters((string)value, parameters);
            var valueType = value.GetType();

            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                return MaterializeDictionary(value, parameters);
            if (valueType.IsArray)
                return MaterializeArray(value, parameters);
            if (valueType.IsClass)
            {
                foreach (var propertyInfo in valueType.GetProperties())
                    propertyInfo.SetValue(value, MaterializeTemplateValue(propertyInfo.GetValue(value), parameters));
            }
            return value;
        }

        private static object MaterializeArray(object value, Dictionary<string, string> parameters)
        {
            return typeof(TemplateProcessor).GetMethod(nameof(MaterializeTemplateArray), BindingFlags.Static | BindingFlags.NonPublic)
               .MakeGenericMethod(value.GetType().GetElementType())
               .Invoke(null, new[] { value, parameters });
        }

        private static object MaterializeDictionary(object value, Dictionary<string, string> parameters)
        {
            return typeof(TemplateProcessor).GetMethod(nameof(MaterializeTemplateDictionary), BindingFlags.Static | BindingFlags.NonPublic)
                .MakeGenericMethod(value.GetType().GenericTypeArguments)
                .Invoke(null, new[] { value, parameters });
        }

        private static Dictionary<TKey, TValue> MaterializeTemplateDictionary<TKey, TValue>(Dictionary<TKey, TValue> source, Dictionary<string, string> parameters)
        {
            return source.ToDictionary(kv => (TKey)MaterializeTemplateValue(kv.Key, parameters),
                kv => (TValue)MaterializeTemplateValue(kv.Value, parameters));
        }

        private static T[] MaterializeTemplateArray<T>(T[] source, Dictionary<string, string> parameters)
        {
            return source.Select(v => (T)MaterializeTemplateValue(v, parameters)).ToArray();
        }

        private static string ApplyParameters(string value, Dictionary<string, string> parameters)
        {
            var sb = new StringBuilder();
            while (true)
            {
                var match = ParameterReplacement.Match(value);
                if (!match.Success)
                {
                    Unescape(sb, value);
                    break;
                }
                var grp = match.Groups["3"];
                Unescape(sb, value.Substring(0, grp.Index));

                string paramValue;
                if (!parameters.TryGetValue(match.Groups["4"].Value, out paramValue))
                    throw new KeyNotFoundException($"Parameter ${{{match.Groups["4"].Value}}} is not specified in template");
                sb.Append(paramValue);

                value = value.Substring(match.Index + match.Length);
            }
            return sb.ToString();
        }

        private static void Unescape(StringBuilder sb, string value)
        {
            int last = 0;
            int pos = 0;
            while ((pos = value.IndexOf("\\$", last, StringComparison.Ordinal)) >= 0)
            {
                int count = 0;
                int i = pos;
                while (i >= last && value[i--] == '\\')
                    ++count;
                sb.Append(value.Substring(last, pos - last + 1 - (count % 2)).Replace("\\\\", "\\"));
                last = pos + 1;
            }
            sb.Append(value.Substring(last).Replace("\\\\", "\\"));
        }

        private static void VerifyTemplateParameters(IYamlTemplate template, IReadOnlyDictionary<string, string> modelParameters)
        {
            var valid = modelParameters.EnsureNotNull().Count() == template.TemplateParameters.EnsureNotNull().Count();
            valid = valid && (modelParameters == null || template.TemplateParameters.EnsureNotNull().All(modelParameters.ContainsKey));

            if (!valid)
                throw new InvalidOperationException($"Invalid template parameters used for template {template.TemplateName}, expected: {string.Join(", ", template.TemplateParameters.EnsureNotNull())}, got: {string.Join(", ", modelParameters.EnsureNotNull().Select(kv => kv.Key))}");
        }
    }
}