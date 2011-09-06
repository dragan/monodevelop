// 
// UnitTest.cs
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

namespace MonoDevelop.UnitTesting
{
	public class UnitTest
	{
		readonly List<UnitTest> children;
		
		public string Name { get; private set; }
		public UnitTest Parent { get; set; }
		public IList<UnitTest> Children { get { return children; } }
		public event EventHandler UnitTestChanged;
		
		public UnitTest (string name)
		{
			Name = name;
			
			children = new List<UnitTest> ();
		}
		
		public void AddChild (UnitTest unitTest)
		{
			unitTest.Parent = this;
			children.Add (unitTest);
			OnUnitTestChanged ();
		}
		
		protected void ClearChildren ()
		{
			foreach (var unitTest in children)
				unitTest.Parent = null;
				
			children.Clear ();
			OnUnitTestChanged ();
		}
		
		protected void OnUnitTestChanged ()
		{
			if (UnitTestChanged != null)
				UnitTestChanged.Invoke (this, EventArgs.Empty);
		}
	}
}
