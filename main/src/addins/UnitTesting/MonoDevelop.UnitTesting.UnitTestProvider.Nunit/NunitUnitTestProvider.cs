// 
// NunitUnitTestProvider.cs
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
using System.Reflection;

using NUnit.Core;
using NUnit.Util;

using MonoDevelop.Core;
using MonoDevelop.UnitTesting;

namespace MonoDevelop.UnitTesting.UnitTestProvider.Nunit
{
	public class NunitUnitTestProvider : IUnitTestProvider
	{
		static bool isNunitInitialized;
		
		public string AssemblyName
		{
			get { return "nunit.framework"; }
		}
		
		public string FrameworkName
		{
			get { return "NUnit"; }
		}
		
		public NunitUnitTestProvider ()
		{
			InitializeNunit ();
		}
		
		public void ExploreAssembly (Assembly assembly, UnitTest parentUnitTest)
		{
			TestRunner testRunner = CreateTestRunner (assembly.Location);
			var assemblyUnitTest = new NunitAssemblyUnitTest (assembly, testRunner);
			parentUnitTest.AddChild (assemblyUnitTest);
			
			foreach (ITest assemblyTestSuite in testRunner.Test.Tests)
				CreateUnitTestChildren (assembly, assemblyUnitTest, assemblyTestSuite);
		}
		
		static void CreateUnitTest (Assembly assembly, NunitUnitTest parentUnitTest, ITest nunitTest)
		{
			var createdUnitTest = new NunitUnitTest (nunitTest.TestName.Name, nunitTest);
			parentUnitTest.AddChild (createdUnitTest);
			CreateUnitTestChildren (assembly, createdUnitTest, nunitTest);
		}
		
		static void CreateUnitTestChildren (Assembly assembly, NunitUnitTest parentUnitTest, ITest parentNunitTest)
		{
			if (parentNunitTest.Tests != null)
				foreach (ITest childNunitTest in parentNunitTest.Tests)
					CreateUnitTest (assembly, parentUnitTest, childNunitTest);
		}
		
		static TestRunner CreateTestRunner (string assemblyLocation)
		{
			TestPackage package = new TestPackage (@"Tests");
			package.Settings.Add (@"AutoNamespaceSuites", true);
			package.Assemblies.Add (assemblyLocation);

			TestRunner testRunner = new RemoteTestRunner ();
			if (!testRunner.Load (package))
				LoggingService.LogError ("Could not load test package with assembly: {0}", assemblyLocation);

			return testRunner;
		}
		
		static void InitializeNunit ()
		{
			if (!isNunitInitialized)
			{
				ServiceManager.Services.AddService (new DomainManager ());
				ServiceManager.Services.AddService (new AddinRegistry ());
				ServiceManager.Services.AddService (new AddinManager ());
				ServiceManager.Services.AddService (new TestAgency ());
				ServiceManager.Services.InitializeServices ();
				
				AppDomain.CurrentDomain.SetData ("AddinRegistry", Services.AddinRegistry);
				
				isNunitInitialized = true;
			}
		}
	}
}
