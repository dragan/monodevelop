// 
// UnitTestExplorerPad.cs
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

using MonoDevelop.Core;
using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.Ide.Gui.Pads;

namespace MonoDevelop.UnitTesting.UnitTestExplorer.Gui
{
	public class UnitTestExplorerPad : TreeViewPad
	{
		UnitTestExplorerService unitTestExplorerService = UnitTestExplorerService.Instance;
		
		public UnitTestExplorerPad ()
		{
		}
		
		public override void Initialize (NodeBuilder[] builders, TreePadOption[] options, string contextMenuPath)
		{
			base.Initialize (builders, options, contextMenuPath);
			
			unitTestExplorerService.UnitTestSuiteChanged += (EventHandler) DispatchService.GuiDispatch (new EventHandler (OnUnitTestSuiteChanged));
			
			if (unitTestExplorerService.UnitTestSuite != null)
				foreach (UnitTest unitTest in unitTestExplorerService.UnitTestSuite)
					TreeView.AddChild (unitTest);
			
			LoggingService.LogInfo ("UnitTestExplorerPad:  Initialized");
		}
		
		void OnUnitTestSuiteChanged (object sender, EventArgs e)
		{
			if (unitTestExplorerService.UnitTestSuite.Length > 0)
			{
				TreeView.Clear ();
				foreach (UnitTest unitTest in unitTestExplorerService.UnitTestSuite)
					TreeView.AddChild (unitTest);
			}
			else
			{
				TreeView.Clear ();
			}
		}
	}
}
