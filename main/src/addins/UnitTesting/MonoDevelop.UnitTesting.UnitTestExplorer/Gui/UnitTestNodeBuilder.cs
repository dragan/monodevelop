// 
// UnitTestNodeBuilder.cs
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

using MonoDevelop.Ide;
using MonoDevelop.Ide.Gui.Components;
using MonoDevelop.UnitTesting;

namespace MonoDevelop.UnitTesting.UnitTestExplorer.Gui
{
	public class UnitTestNodeBuilder : TypeNodeBuilder
	{
		EventHandler unitTestChanged;
		
		public UnitTestNodeBuilder ()
		{
			unitTestChanged = (EventHandler) DispatchService.GuiDispatch (new EventHandler (OnUnitTestChanged));
		}
		
		public override Type NodeDataType
		{
			get { return typeof (UnitTest); }
		}
		
		public override string GetNodeName (ITreeNavigator thisNode, object dataObject)
		{
			return ((UnitTest)dataObject).Name;
		}
		
		public override void BuildNode (ITreeBuilder treeBuilder, object dataObject, ref string label, ref Gdk.Pixbuf icon, ref Gdk.Pixbuf closedIcon)
		{
			UnitTest unitTest = dataObject as UnitTest;
			
			label = unitTest.Name;
		}
		
		public override void BuildChildNodes (ITreeBuilder builder, object dataObject)
		{
			UnitTest unitTest = dataObject as UnitTest;
			if (unitTest == null)
				return;
			
			foreach (UnitTest childUnitTest in unitTest.Children)
				builder.AddChild (childUnitTest);
		}
		
		public override bool HasChildNodes (ITreeBuilder builder, object dataObject)
		{
			UnitTest unitTest = dataObject as UnitTest;
			return unitTest != null && unitTest.Children.Count > 0;
		}
		
		public override void OnNodeAdded (object dataObject)
		{
			UnitTest unitTest = dataObject as UnitTest;
			if (unitTest != null)
				unitTest.UnitTestChanged += unitTestChanged;
		}
		
		public override void OnNodeRemoved (object dataObject)
		{
			UnitTest unitTest = dataObject as UnitTest;
			if (unitTest != null)
				unitTest.UnitTestChanged -= unitTestChanged;
		}
		
		public void OnUnitTestChanged (object sender, EventArgs args)
		{
			ITreeBuilder tb = Context.GetTreeBuilder (sender);
			if (tb != null)
				tb.UpdateAll ();
		}
	}
}
