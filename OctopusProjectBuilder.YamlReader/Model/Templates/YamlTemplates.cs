using System;
using System.ComponentModel;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Description(@"Templates model definition.

The templating mechanism allows to simplify the model definitions and speed-up the process of defining them, by extracting the common model structure into the templates and later applying them to the models.

#### Specifying template values and applying it to the model
The model template allows to define values of all the properties that are defined in model.
The model defintion itself can refer to the template, but it also can have own specification of the properties.
When the resource is being instantiated, the template would be used to provide the default values of the properties, that would be then customized with model definition.

#### Model property override
The model definition can override the default template value. It happens when both, template and model have defined the property value.
The property value resolution is implemented in a following way:

* if model have defined the value, it is being used,
* if model does not have defined property value (it is null), then template value is used.

Please note, that properties of value type (like int, bool etc) would never be null, so they cannot be templated.
Please also note that properties of collection type (arrays, dictionaries) cannot be partially overriden. If model would contain a definition of such properties, the template property value would be ignored.

#### Parameterized templates
It is possible to parameterize templates with a list of parameters (consisting of alphanumeric and/or underscore characters).
The parameters can be used in property values defined in the template (at any tree level, including inner models, dictionary values, etc.), but they can be only applied to **string** type properties.
They are referenced with `${paramName}` syntax. It is possible to escape the `$` character with `\` if string like should not be updated: `\${this_is_not_a_param}`.

Example:
Assumming that we have parameters: 
* packageId=My_project
* packageVersion='1.2.3.4'

the property value `""${packageId} ver ${packageVersion}""` would be updated to `""My_project ver 1.2.3.4""`")]
    [Serializable]
    public class YamlTemplates
    {
        [Description("List of Deployment Step Action templates")]
        public YamlDeploymentActionTemplate[] DeploymentActions { get; set; }
        [Description("List of Deployment Step templates")]
        public YamlDeploymentStepTemplate[] DeploymentSteps { get; set; }
        [Description("List of Project templates")]
        public YamlProjectTemplate[] Projects { get; set; }

        public static YamlTemplates MergeIn(YamlTemplates dst, YamlTemplates src)
        {
            if (src == null)
                return dst;
            if (dst == null)
                return src;
            dst.DeploymentActions = dst.MergeItemsIn(src, x => x.DeploymentActions);
            dst.DeploymentSteps = dst.MergeItemsIn(src, x => x.DeploymentSteps);
            dst.Projects = dst.MergeItemsIn(src, x => x.Projects);
            return dst;
        }
    }
}