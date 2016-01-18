# DevSKiller programming task sample - C#/.NET with MSBuild

## Introduction

With [DevSKiller.com](https://devskiller.com) you can assess candidates' programming skills during your recruitment process. Programming tasks are the best way to test candidates programming skills. The candidate is asked to modify a source code of some existing project.

During the test, the candidate is allowed to edit the source code of the project with our browser-based code editor and can build the project inside the browser at any time. Candidate can also download the project code and edit it locally with the favorite IDE.

Check out how the test looks from candidate's perspective: [Candidate campaign preview](https://www.youtube.com/watch?v=rB4fViXPh5E)


This repo contains an example project for C#/.NET, below you can find a detailed guide for creating your own programming project. Please make sure to read our [Getting started with programming projects](https://docs.devskiller.com/programming_task/index.html) guides first 

## Technical details for C#/.NET support

Any **VisualStudio** solution might be used as a programming task. Your project would be run with the **MSBuild**. We support **NUnit** tests for Unit Tests and **NuGet** for external dependencies.

## Automatic assessment

It is possible to automatically assess solution posted by the candidate. Automatic assessment is based on Unit Tests results and Code Quality measurements. 

All unit tests that are executed during the build will be detected by the DevSKiller platform. 

There are two kinds of unit tests:

1. **Candidate tests** - unit tests that are visible for the candidate during the test. Should be used to do only the basic verification and help the candidate to understand the requirements. Candidate tests WOULD NOT be used to calculate the final score.
2. **Verification tests** - unit tests that are hidden from the candidate during the test. Files containing verification tests would be added to the project after the candidate finishes the test and will be executed during verification phase. Verification tests result would be used to calculate the final score.

After candidate finishes the campaign, our platform builds the project posted by the candidate and executes verification tests and static code analysis.

## DevSKiller project descriptor

Programming task can be configured with the DevSKiller project descriptor file. Just create a `devskiller.json` file and place it in the root directory of your project. Here is an example project descriptor:

```json
{
  "readOnlyFiles" : [ "CalculatorSample.sln" ],
  "hiddenFiles" : [ "hidden_file.txt" ],
  "verification" : {
    "testNamePatterns" : [".*VerifyTests.*"],
    "pathPatterns" : ["CalculatorSample.Tests/VerifyTests.cs"],
    "overwrite" : {
        "CalculatorSample.Tests/CalculatorSample.VerifyTests.csproj" : "CalculatorSample.Tests/CalculatorSample.Tests.csproj"
    }
  }
}
```

You can find more details about `devskiller.json` descriptor in our [documentation](https://docs.devskiller.com/)

## Automatic verification with verification tests

To enable automatic verification of candidates' solution, you need to define which tests should be treated as verification tests.

All files classified as verification tests will be removed from a project prepared for the candidate.

To define verification tests, you need to set two configuration properties in `devskiller.json` project descriptor:

- `testNamePatterns` - an array of RegEx patterns which should match all test names of verification tests. 
Test name contains: `[namespace_name].[Class_name].[method_name]`. In our sample project all verification tests are inside `VerifyTests` class, so following pattern will be sufficient:

```json
"testNamePatterns" : [".*VerifyTests.*"]
```

- `pathPatterns` - an array of GLOB patterns which should match all files containing verification tests. All files that match defined patterns will be deleted from candidates' projects and will be added to the projects during the verification phase. 

```json
"pathPatterns" : ["CalculatorSample.Tests/VerifyTests.cs"]
```

Because files with verification tests will be deleted from candidates' projects, we need to address that in `CalculatorSample.Tests.csproj` file. 
By default the `csproj` file contains following entry:

```xml
  <ItemGroup>
    <Compile Include="CalculatorTest.cs" />
    <Compile Include="VerifyTests.cs" />
    <Compile Include="Properties\AssemblyInfo.cs" />
  </ItemGroup>
```

which is incorrect from candidate's perspective, because there is no `VerifyTests.cs` file in project prepared for the candidate.


There are two ways to handle this:

### 1. Define `<Compile>` as a wildcard. 

You will just need to substitute above entry with the following entry:

```xml
  <ItemGroup>
    <Compile Include="*.cs" />
  </ItemGroup>
```

With such configuration build process will discover all `*.cs` files at runtime.

### 2. Create an alternative `*.csproj` file that will overwrite the candidate's `*.csproj` file during verification phase. 

To do so, we would need to perform three steps:
 
 - create a copy of `CalculatorSample.Tests.csproj` and call it, for example, `CalculatorSample.VerifyTests.csproj` 
 - delete `VerifyTests.cs` entry from the original `CalculatorSample.Tests.csproj`, so we it will not try to compile a file which doesn't exist in candidate's project.
 - add an `"overwrite"` entry to `devskiller.json` project descriptor:
 
```json
"overwrite" : {
"CalculatorSample.Tests/CalculatorSample.VerifyTests.csproj" : "CalculatorSample.Tests/CalculatorSample.Tests.csproj"
}
```