# OctopusProjectBuilder
A project to provide an ability to configure Octopus projects from text, source controlled files (like yaml).

## Build
To build project, execute a following command from Powershell:
`PS> .\build.ps1` 

## Yaml configuration manual
To see the yaml configuration manual, please take a look at [Manual.md](Manual.md)

## Supported Octopus resource configuration:

* [Project Groups](Manual.md#YamlProjectGroup)
* [Projects](Manual.md#YamlProject)
* [Lifecycles](Manual.md#YamlLifecycle)
* [Library Variable Sets](Manual.md#YamlLibraryVariableSet) \(including script modules\)

## Octopus resource configuration templating:

OctopusProjectBuilder allows template definition for most repetative configuration sections like:

* Projects - see [Project Templates](Manual.md#YamlProjectTemplate)
* Project Steps - see [Deployment Step Templates](Manual.md#YamlDeploymentStepTemplate)
* Project Step Actions - see [Deployment Action Templates](Manual.md#YamlDeploymentActionTemplate)

It is possible to create parameterized templates with ability to parameterize any properties of string type.
It is also possible to use templates for lower configuration sections in template of higher configuration sections. For example The Project template can use Step templates that can be composed from Action templates.

For more information, please take a look at [Template manual](Manual.md#YamlTemplates).
