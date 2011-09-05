// 
// UnitTestExplorerService.cs
//  
// Author:
//       Dale Ragan <dale.ragan@moncai.com>
// 
// Copyright (c) 2011 Copyright (c) 2011 Moncai, LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Threading;

using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Projects;
using MonoDevelop.UnitTesting;

namespace MonoDevelop.UnitTesting.UnitTestExplorer
{
	public class UnitTestExplorerService
	{
		static UnitTestExplorerService instance;
		public static UnitTestExplorerService Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new UnitTestExplorerService ();
					instance.AsyncFindUnitTests ();
				}
				
				return instance;
			}
		}
		
		public UnitTest[] UnitTestSuite { get; private set; }
		
		public event EventHandler UnitTestSuiteChanged;
		
		IList<IUnitTestProvider> unitTestProviders;
		
		UnitTestExplorerService ()
		{
			unitTestProviders = new List<IUnitTestProvider> ();
			
			IdeApp.Workspace.ReferenceAddedToProject += OnWorkspaceChanged;
			IdeApp.Workspace.ReferenceRemovedFromProject += OnWorkspaceChanged;
			IdeApp.Workspace.WorkspaceItemOpened += OnWorkspaceChanged;
			IdeApp.Workspace.WorkspaceItemClosed += OnWorkspaceChanged;
			IdeApp.Workspace.ItemAddedToSolution += OnWorkspaceChanged;
			IdeApp.Workspace.ItemRemovedFromSolution += OnWorkspaceChanged;
			
			AddinManager.AddExtensionNodeHandler ("/MonoDevelop/UnitTesting/UnitTestProviders", OnExtensionNodeChanged);
		}
		
		void OnExtensionNodeChanged (object s, ExtensionNodeEventArgs args)
		{
			if (args.Change == ExtensionChange.Add)
			{
				IUnitTestProvider unitTestProvider = args.ExtensionObject as IUnitTestProvider;
				unitTestProviders.Add (unitTestProvider);
				LoggingService.LogInfo ("Added Unit Test Provider: {0}", unitTestProvider.GetType ());
			}
			else
			{
				IUnitTestProvider unitTestProvider = args.ExtensionObject as IUnitTestProvider;
				unitTestProviders.Remove (unitTestProvider);
				LoggingService.LogInfo ("Removed Unit Test Provider: {0}", unitTestProvider.GetType ());
			}
		}
		
		void AsyncFindUnitTests ()
		{
			Thread thread = new Thread (FindUnitTests);
			thread.Name = "Unit Test Explorer";
			thread.IsBackground = true;
			thread.Start ();
		}
		
		void FindUnitTests ()
		{
			List<UnitTest> unitTests = new List<UnitTest> ();
			
			// TODO: For now, we'll just get the projects.  We'll add the other types of workspace items later, like SolutionFolders
			foreach (Project project in IdeApp.Workspace.GetAllProjects ())
			{
				DotNetProject dotNetProject = project as DotNetProject;
				if (dotNetProject != null)
				{
					foreach (IUnitTestProvider unitTestProvider in unitTestProviders)
					{
						if (IsUnitTestProviderFrameworkReferenced (unitTestProvider, dotNetProject))
						{
							LoggingService.LogInfo ("Found project that references the {0} Framework: {1}", unitTestProvider.FrameworkName, dotNetProject.Name);
							unitTests.Add (new UnitTestProject (dotNetProject, unitTestProvider));
						}
					}
				}
			}
			
			UnitTestSuite = unitTests.ToArray ();
			NotifyUnitTestSuiteChanged ();
		}
		
		bool IsUnitTestProviderFrameworkReferenced (IUnitTestProvider unitTestProvider, DotNetProject dotNetProject)
		{
			bool isReferenced = false;
			
			foreach (ProjectReference pr in dotNetProject.References)
			{
				if (pr.Reference.IndexOf (unitTestProvider.AssemblyName) == -1)
					continue;
				
				isReferenced = true;
			}
			
			return isReferenced;
		}
		
		void OnWorkspaceChanged (object sender, EventArgs e)
		{
			AsyncFindUnitTests ();
		}
		
		void NotifyUnitTestSuiteChanged ()
		{
			if (UnitTestSuiteChanged != null)
				UnitTestSuiteChanged.Invoke (this, EventArgs.Empty);
		}
	}
	
	public class UnitTestProject : UnitTest
	{
		readonly DotNetProject dotNetProject;
		readonly IUnitTestProvider unitTestProvider;
		
		public string AssemblyPath
		{
			get { return dotNetProject.GetOutputFileName (IdeApp.Workspace.ActiveConfiguration); }
		}
		
		public bool AssemblyExists
		{
			get { return System.IO.File.Exists (AssemblyPath); }
		}
		
		public UnitTestProject (DotNetProject dotNetProject, IUnitTestProvider unitTestProvider) : base (dotNetProject.Name)
		{
			this.dotNetProject = dotNetProject;
			this.unitTestProvider = unitTestProvider;
		}
	}
}
