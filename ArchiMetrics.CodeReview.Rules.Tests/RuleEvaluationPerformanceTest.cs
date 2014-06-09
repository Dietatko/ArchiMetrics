﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RuleEvaluationPerformanceTest.cs" company="Reimers.dk">
//   Copyright © Reimers.dk 2014
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the RuleEvaluationPerformanceTest type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.CodeReview.Rules.Tests
{
	using System;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using ArchiMetrics.Analysis;
	using ArchiMetrics.Common;
	using ArchiMetrics.Common.CodeReview;
	using metrics;
	using Microsoft.CodeAnalysis.MSBuild;
	using NUnit.Framework;

	public class RuleEvaluationPerformanceTest
	{
		private NodeReviewer _reviewer;

		[SetUp]
		public void Setup()
		{
			_reviewer = new NodeReviewer(
				AllRules.GetRules()
					.Select(
						r =>
						{
							try
							{
								return (IEvaluation)Activator.CreateInstance(r);
							}
							catch
							{
								return null;
							}
						})
					.WhereNotNull());
		}

		[Test, Ignore("Run manually")]
		public void MeasurePerformance()
		{
			var timer = Metrics.Timer(GetType(), "test", TimeUnit.Seconds, TimeUnit.Seconds);
			for (var i = 0; i < 10; i++)
			{
				var amount = timer.Time(() => PerformReview().Result);
			}

			Console.WriteLine("Min" + TimeSpan.FromSeconds(timer.Min));
			Console.WriteLine("Max" + TimeSpan.FromSeconds(timer.Max));
			Console.WriteLine("Mean" + TimeSpan.FromSeconds(timer.Mean));
			Assert.Less(timer.Mean, 90.0);
		}

		private async Task<int> PerformReview()
		{
			using (var workspace = MSBuildWorkspace.Create())
			{
				var path = @"..\..\..\archimetrics.sln".GetLowerCaseFullPath();
				var solution = await workspace.OpenSolutionAsync(path).ConfigureAwait(false);
				var results = await _reviewer.Inspect(solution).ConfigureAwait(false);
				var amount = results.ToArray();
				return amount.Length;
			}
		}
	}
}