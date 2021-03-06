// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MemberMetric.cs" company="Reimers.dk">
//   Copyright � Matthias Friedrich, Reimers.dk 2014
//   This source is subject to the MIT License.
//   Please see https://opensource.org/licenses/MIT for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the MemberMetric type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.Analysis.Metrics
{
	using System.Collections.Generic;
	using Common;
	using Common.Metrics;

    internal class MemberMetric : IMemberMetric
	{
		private readonly IHalsteadMetrics _halstead;

		public MemberMetric(
			string codeFile,
			AccessModifierKind accessModifier,
			IHalsteadMetrics halstead,
			int lineNumber,
			int linesOfCode,
			double maintainabilityIndex,
			int cyclomaticComplexity,
			string name,
			IEnumerable<ITypeCoupling> classCouplings,
			int numberOfParameters,
			int numberOfLocalVariables,
			int afferentCoupling,
			IMemberDocumentation documentation)
		{
			_halstead = halstead;
			CodeFile = codeFile;
			AccessModifier = accessModifier;
			LineNumber = lineNumber;
			LinesOfCode = linesOfCode;
			MaintainabilityIndex = maintainabilityIndex;
			CyclomaticComplexity = cyclomaticComplexity;
			Name = name;
			ClassCouplings = classCouplings.AsArray();
			NumberOfParameters = numberOfParameters;
			NumberOfLocalVariables = numberOfLocalVariables;
			AfferentCoupling = afferentCoupling;
			Documentation = documentation;
		}

		public string CodeFile { get; private set; }

		public AccessModifierKind AccessModifier { get; private set; }

		public int LineNumber { get; private set; }

		public int LinesOfCode { get; private set; }

		public double MaintainabilityIndex { get; private set; }

		public int CyclomaticComplexity { get; private set; }

		public string Name { get; private set; }

		public IEnumerable<ITypeCoupling> ClassCouplings { get; private set; }

		public int NumberOfParameters { get; private set; }

		public int NumberOfLocalVariables { get; private set; }

		public int AfferentCoupling { get; private set; }

		public IMemberDocumentation Documentation { get; private set; }

		public IHalsteadMetrics GetHalsteadMetrics()
		{
			return _halstead;
		}

		public double GetVolume()
		{
			return _halstead == null
			? 0d
			: _halstead.GetVolume();
		}
	}
}
