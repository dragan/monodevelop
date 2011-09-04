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

using Mono.Addins;
using MonoDevelop.Core;
using MonoDevelop.UnitTesting;

namespace MonoDevelop.UnitTesting.UnitTestExplorer
{
	public class UnitTestExplorerService
	{
		static UnitTestExplorerService instance;
		
		IList<IUnitTestProvider> unitTestProviders;
		
		public static UnitTestExplorerService Instance
		{
			get
			{
				if (instance == null)
					instance = new UnitTestExplorerService ();
				
				return instance;
			}
		}
		
		UnitTestExplorerService ()
		{
			unitTestProviders = new List<IUnitTestProvider> ();
			
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
	}
}
