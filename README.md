# C#/.NET with MSBuild
## How to create custom C#/.NET programing task under .NET framework with MSBuild.

**TIP**: Please make sure to read [Getting started with programming tasks](https://help.devskiller.com/creating-tasks-and-tests/getting-started-with-programming-tasks) first.

You can start with our sample project that can be found on GitHub:
[Open sample project](https://github.com/maciej-halaczkiewicz/devskiller-sample-csharp-calculator)
[Download sample project](https://github.com/maciej-halaczkiewicz/devskiller-sample-csharp-calculator/archive/master.zip)

## Automatic assessment
It is possible to automatically assess a solution posted by the candidate.
Automatic assessment is based on Tests results and Code Quality measurements. 

All unit tests that are executed during the build will be detected by the Devskiller platform. 

There are two kinds of unit tests:

1. **Candidate tests** - unit tests that are visible for the candidate during the test. These should be used to do only basic verification and are designed to help the candidate understand the requirements. Candidate tests **WILL NOT** be used to calculate the final score.
2. **Verification tests** - unit tests that are hidden from the candidate during the assessment. Files containing verification tests will be added to the project after the candidate finishes the test and will be executed during verification phase. Verification test results **WILL** be used to calculate the final score.
After the candidate finishes the test, our platform builds the project posted by the candidate and executes the verification tests and static code analysis.

## Technical details for .NET support
To create automatic assessment, you'll need compilable **.NET solution** along with working unit tests. Visual Studio 2017 is recomended for that. Any language of .NET platform can be used **(C#, F#, VisualBasic)**, though this article focus on c# only. Currently Devskiller platform supports .NET version range: *4.0-4.7.2*
MSBuild will be used to build your solution. Nuget.exe will restore your nuget packages and TestAgent will run your tests. You can use any unit-testing framework like **NUnit, XUnit or MSTest**, or others. Remember to attach adequate test adapter/runner required by used framework. Otherwise you will get error that no tests can be found.


## Preparing solution for automatic tests
To prepare your solution for automatic assessment you should follow those 3 steps:

### 1. Prepare separate project in your solution for verification tests.
This project should reside in separate folder in the repository. The folder structure could look like this:

```
CalculatorTask
│   .gitignore
│   README.md
│   devskiller.json
│   CalculatorSample.sln   
│   
└───CalculatorSample
│      CalculatorSample.csproj
│      Calculator.cs
│   
└───CalculatorSample.Tests
│      CalculatorSample.Tests.csproj
│      Tests.cs
│   
└───CalculatorSample.VerifyTests
	   CalculatorSample.sln
       CalculatorSample.VerifyTests.csproj
       VerifyTests.cs	
```

The **CalculatorTask\CalculatorSample.VerifyTests** folder from example above, contains the *.csproj and code file for verification tests that will be invisible for candidate. Please note there is also additional *.sln file in this folder - we will get back to it later.

### 2. Prepare devskiller.json file - Devskiller project descriptor. 
Programming task can be configured with the Devskiller project descriptor file. Just create a `devskiller.json` file and place it in the root directory of your project. Here is an example project descriptor:
```
{
  "readOnlyFiles" : [ "CalculatorSample.sln" ],
  "verification" : {
    "testNamePatterns" : [".*VerifyTests.*"],
    "pathPatterns" : ["**VerifyTests**"],
    "overwrite" : {
		"CalculatorSample.VerifyTests/CalculatorSample.sln" : "CalculatorSample.sln"
    }
  }
}
```
You can find more details about devskiller.json descriptor in our [documentation](https://help.devskiller.com/creating-tasks-and-tests/using-custom-programming-tasks/programming-task-project-descriptor)

In example above, by setting `readOnlyFiles` field with a solution file, we make sure candidate won't be able to edit it. **It's important during phase of verification tests execution, don't forget to add it!**
- `testNamePatterns` - an array of RegEx patterns which should match all the test names of verification tests. Test names should contain: `[namespace_name].[Class_name].[method_name]` . In our sample project, all verification tests are inside VerifyTests  class, so the following pattern will be sufficient:
```
"testNamePatterns"  : [".*VerifyTests.*"]
```
- `pathPatterns` - an array of GLOB patterns which should match all the files containing verification tests. All the files that match defined patterns will be deleted from candidates projects and will be added to the projects during the verification phase. 
```
"pathPatterns" : ["**VerifyTests**"]
```

Because files with verification tests will be deleted from candidates projects, you need to make sure, that during final solution build, Devskiller platform will be aware of them.
To make that happen, you must point which solution file should be overwritten - you want solution from **CalculatorTask\CalculatorSample.VerifyTests** folder to overwrite the **root** solution file
```
"CalculatorSample.VerifyTests/CalculatorSample.sln" : "CalculatorSample.sln"
```
So the last thing is to prepare proper solution files:


### 3. Preparing two solution files.

You need *two* solution files. 
One in root folder, this is the solution file that will be used by the candidate. It should have project structure that the candidate should see, so there should be no verification test project there:

`CalculatorTask\CalculatorSample.sln` solution structure:
```
Solution 'CalculatorSample'
│   
└───CalculatorSample
│   
└───CalculatorSample.Tests
```

Second one is the 'temporary' solution file residing in verification tests folder. **During final testing it will override the root solution file** and thanks to that, test platform will be aware of existence of `CalculatorSample.VerifyTests.csproj` in solution

`CalculatorSample.VerifyTests\CalculatorSample.sln` solution structure:
```
Solution 'CalculatorSample'
│   
└───CalculatorSample
│   
└───CalculatorSample.Tests
│   
└───CalculatorSample.VerifyTests
```

Easiest way to have those two *.sln files is to prepare solution with verification tests, copy it to verification tests folder, than go back to root solution, remove the verification tests project and save it.


## Hints

1. Remember that you aren't bounded to unit tests. You can do some integration tests
2. Make sure, each test is self-runnable and independent of external components like: native system libraries, external databases, etc.
3. Avoid using external libraries in your source. If you need some external libraries please reference them as NuGet packages. This will make sure, you're code will behave on Devskiller platform in the same way it behaves on your environment.
4. When needed and applicable, consider usage of in-memory database engine emulation when aplicable (like Effort). It's fast to run and easy to use.
5. Try to make test names clear and as short as possible. Long names will be harder to read for recruiters and can be confusing. Some test-parameter-injection methods tend to generate complex text output when executing tests. Make sure to check, if the test output looks good.
6. Remember to describe clearly the task instructions in `README.md` file.
7. When leaving gaps in code to be filled by candidate, consider throwing `NotImplementedException`, rather than in ex. returning `null`, `false`, `0`, etc. in your methods. As those returned values, dependent on logic, could be some edge cases, besides, the unit tests execution will instantly fail even before checking assertions, so the candidadte will be 100% sure where he is expected to make code changes.
8. You want candidate to start working on the task as soon as possible, not to struggle with configuration. Project delivered for candidate should be compilable and working without any configuration needed. It should only fail tests on execution.
