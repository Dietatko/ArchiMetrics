ArchiMetrics
============

Various code analysis tools for C#.

To build simply run the build.ps1 script.

If that fails, please log an issue, because the idea is to have a one click build experience.

About the project
--------------------

ArchiMetrics is a collection of code analysis tools using Roslyn.
It will calculate code metrics and code reviews which can be stored in a RavenDB and used for reports later.

Using project
-------------------
If you are going to use metrics, you must install

[Microsoft Build Tools 2015 RC](http://www.microsoft.com/en-us/download/details.aspx?id=46882&WT.mc_id=rss_alldownloads_all)

You also may need to install this package

```
Install-Package Microsoft.Composition
```

See this sample that load solution and print cyclomatic complexity for each namespace that belong your solution

````csharp
using System;
using System.Linq;
using System.Threading.Tasks;
using ArchiMetrics.Analysis;
using ArchiMetrics.Common;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Run();
            task.Wait();
        }

        private static async Task Run()
        {
            Console.WriteLine("Loading Solution");
            var solutionProvider = new SolutionProvider();
            var solution = await solutionProvider.Get(@"MyFullPathSolutionFile.sln");
            Console.WriteLine("Solution loaded");

            var projects = solution.Projects.ToList();

            Console.WriteLine("Loading metrics, wait it may take a while.");
            var metricsCalculator = new CodeMetricsCalculator();
            var calculateTasks = projects.Select(p => metricsCalculator.Calculate(p, solution));
            var metrics = (await Task.WhenAll(calculateTasks)).SelectMany(nm => nm);
            foreach (var metric in metrics)
                Console.WriteLine("{0} => {1}", metric.Name, metric.CyclomaticComplexity);
        }
    }
}
```
