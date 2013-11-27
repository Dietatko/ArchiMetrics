// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppContext.cs" company="Reimers.dk">
//   Copyright � Reimers.dk 2013
//   This source is subject to the Microsoft Public License (Ms-PL).
//   Please see http://go.microsoft.com/fwlink/?LinkID=131993 for details.
//   All other rights reserved.
// </copyright>
// <summary>
//   Defines the AppContext type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ArchiMetrics.UI.DataAccess
{
	using System;
	using System.Collections.Specialized;
	using System.ComponentModel;
	using System.Reactive.Linq;
	using System.Runtime.CompilerServices;
	using ArchiMetrics.Common.Structure;

	public class AppContext : IAppContext
	{
		private readonly IAvailableRules _availableRules;
		private bool _includeCodeReview;
		private string _path;
		private EdgeSource _source;
		private readonly IDisposable _subscription;
		private string _rulesSource;

		public AppContext(IAvailableRules availableRules)
		{
			_rulesSource = string.Empty;
			_availableRules = availableRules;
			_subscription = Observable.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
				h => _availableRules.CollectionChanged += h,
				h => _availableRules.CollectionChanged -= h)
				.Throttle(TimeSpan.FromSeconds(3))
				.Subscribe(x => OnPropertyChanged(string.Empty));
			CutOff = TimeSpan.FromDays(7);
		}

		~AppContext()
		{
			Dispose(false);
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public TimeSpan CutOff { get; set; }

		public string Path
		{
			get
			{
				return _path;
			}

			set
			{
				if (_path != value)
				{
					_path = value;
					OnPropertyChanged();
				}
			}
		}

		public bool IncludeCodeReview
		{
			get
			{
				return _includeCodeReview;
			}

			set
			{
				if (_includeCodeReview != value)
				{
					_includeCodeReview = value;
					OnPropertyChanged();
				}
			}
		}

		public string RulesSource
		{
			get { return _rulesSource; }
			set
			{
				if (_rulesSource != value)
				{
					_rulesSource = value;
					OnPropertyChanged();
				}
			}
		}

		public EdgeSource Source
		{
			get
			{
				return _source;
			}

			set
			{
				if (_source != value)
				{
					_source = value;
					OnPropertyChanged();
				}
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		private void Dispose(bool isDisposing)
		{
			if (isDisposing)
			{
				_subscription.Dispose();
			}
		}
	}
}