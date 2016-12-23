# Octopus Project Builder

[![Build status](https://ci.appveyor.com/api/projects/status/vgt4b26gcxywurb6?svg=true)](https://ci.appveyor.com/project/Suremaker/octopusprojectbuilder)

A project providing an ability to configure Octopus projects from source controlled text files (like yaml).

It is conceptually based on [Jenkins Job Builder](http://docs.openstack.org/infra/jenkins-job-builder/) which I have found to be a great utility tool to work with [Jenkins](https://jenkins.io/).

Using the Octopus Project Builder, together with source control system like git, gives the following benefits:
* change history,
* ability to use the previous version of configuration,
* ability to create a review process before applying changes,
* configuration editing, like extracting variables to library variable sets, or applying bulk changes to many projects.

## Build
To build project, execute a following command from Powershell:
`PS> .\build.ps1` 

## Yaml configuration manual
To see the yaml configuration manual, please take a look at [Configuration Manual](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual)

## Supported Octopus resource configuration

* [Project Groups](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlProjectGroup)
* [Projects](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlProject)
* [Lifecycles](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlLifecycle)
* [Library Variable Sets](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlLibraryVariableSet) \(including script modules\)
* [Environments](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlEnvironment)

## Octopus resource configuration templating

OctopusProjectBuilder allows template definition for most repetative configuration sections like:

* Projects - see [Project Templates](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlProjectTemplate)
* Project Steps - see [Deployment Step Templates](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlDeploymentStepTemplate)
* Project Step Actions - see [Deployment Action Templates](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlDeploymentActionTemplate)

It is possible to create parameterized templates with ability to parameterize any properties of string type.
It is also possible to use templates for lower configuration sections in template of higher configuration sections. For example The Project template can use Step templates that can be composed from Action templates.

For more information, please take a look at [Template manual](https://github.com/Suremaker/OctopusProjectBuilder/wiki/Manual#YamlTemplates).

## OctopusProjectBuilder.Console.exe usage
The OctopusProjectBuilder.Console.exe requires following paramters:

|Parameter|Description|
|---------|-----------|
|a:action|Action to perform: Upload, Download, CleanupConfig|
|d:definitions|Definitions directory|
|k:octopusApiKey|Octopus API key|
|u:octopusUrl|Octopus Url|

The **download** action allows to download the current Octopus configuration to the target directory,
the **upload** action allows to apply the yaml configuration on Octopus server, while
the **cleanupConfig** action allows to rewrite the configuration, and it is helpful to reorder nodes, reformats parameters or remove the entries with default values.

## Limitations

### Sensitive variables

OctopusProjectBuilder.Console.exe is not able to download value of sensitive variables or parameters. The downloaded sensitive variables will have IsSensitive flag set to true, but variable will be null.

If user wants to define a sensitive variable in yaml, it is possible to specify `IsSensitive:true`, but the `Value` property will have to hold plain text value. When OctopusProjectBuilder.Console.exe uploads it, it would be kept in Octopus in encrypted format.
Currently, the only possible workaround is to define a **Library Variable Set** with credentials in Octopus, and refer to it in the project:

```Yaml
Projects:
- Name: My Project
  IncludedLibraryVariableSetRefs:
  - My credentials
  # ...
  Variables:
  - Name: MyPassword
    Value: #{Credentials_my_password}
```

## Examples

To see the example configuration, please browse the [examples](https://github.com/Suremaker/OctopusProjectBuilder/tree/master/example) project directory.
