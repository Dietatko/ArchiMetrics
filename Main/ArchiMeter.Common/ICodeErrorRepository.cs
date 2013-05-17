﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICodeErrorRepository.cs" company="Roche">
//   Copyright © Roche 2012
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993] for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the ICodeErrorRepository type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace ArchiMeter.Common
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;

	public interface ICodeErrorRepository : IDisposable
	{
		Task<IEnumerable<EvaluationResult>> GetErrorsAsync();

		Task<IEnumerable<EvaluationResult>> GetErrorsAsync(string source, bool isTest);

		IEnumerable<EvaluationResult> GetErrors();

		IEnumerable<EvaluationResult> GetErrors(string source, bool isTest);
	}
}