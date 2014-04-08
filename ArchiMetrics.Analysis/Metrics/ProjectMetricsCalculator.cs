// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectMetricsCalculator.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2013
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ProjectMetricsCalculator type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Analysis.Metrics
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using ArchiMetrics.Common.Metrics;
	using Microsoft.CodeAnalysis;

	internal class ProjectMetricsCalculator : IProjectMetricsCalculator
	{
		private readonly ICodeMetricsCalculator _metricsCalculator;

		public ProjectMetricsCalculator(ICodeMetricsCalculator metricsCalculator)
		{
			_metricsCalculator = metricsCalculator;
		}

		public async Task<IEnumerable<IProjectMetric>> Calculate(Solution solution)
		{
			var tasks = from project in solution.Projects select Calculate(project, solution);

			return await Task.WhenAll(tasks);
		}

		public async Task<IProjectMetric> Calculate(Project project, Solution solution)
		{
			if (project == null)
			{
				return null;
			}

			var metricsTask = _metricsCalculator.Calculate(project, solution);

			var referencedProjects = project.ProjectReferences
				.Select(x => solution.GetProject(x.ProjectId).AssemblyName)
				.Concat(project.MetadataReferences.Select(x => x.Display));

			var compilation = await project.GetCompilationAsync();
			var assemblyTypes = compilation.Assembly.TypeNames;
			var metrics = (await metricsTask).ToArray();

			var internalTypesUsed = metrics
				.SelectMany(x => x.ClassCouplings)
				.Where(x => x.Assembly == project.AssemblyName);
			var relationalCohesion = (internalTypesUsed.Count() + 1.0) / assemblyTypes.Count;
			
			return new ProjectMetric(project.Name, metrics, referencedProjects, relationalCohesion);
		}
	}
}