﻿using System;
using NUnit.Framework;

namespace TwinTechs.EditorExtensions.Helpers
{
	class MockedViewModelHelper : ViewModelHelper
	{
		public string ReturnFileName { get; set; }

		public int NumberOfGetFileCallsToInvokeBeforeReturningTrue { get; set; }

		internal override string CurrentFileName { get { return ReturnFileName; } }

		public int NumberOfTimeGetFileExistsInvoked { get; set; }

		public string FilenamePassedToOpenDocumentMethod { get; private set; }

		internal override bool GetFileExists (string filename)
		{
			NumberOfTimeGetFileExistsInvoked++;
			return NumberOfTimeGetFileExistsInvoked == NumberOfGetFileCallsToInvokeBeforeReturningTrue;
		}

		internal override void OpenDocument (string filename)
		{
			FilenamePassedToOpenDocumentMethod = filename;
		}
	}


	[TestFixture]
	public class ViewModelHelperTests
	{
		MockedViewModelHelper _helper;

		public ViewModelHelperTests ()
		{
		}

		[SetUp]
		public void Setup ()
		{
			_helper = new MockedViewModelHelper ();
		}

		[TestCase ("/tests/test1.xaml", ExpectedResult = "/tests/test1")]
		[TestCase ("/tests/test1.xaml.cs", ExpectedResult = "/tests/test1")]
		[TestCase ("/tests/test1.cs", ExpectedResult = null)]
		[TestCase ("/tests/test1VM.cs", ExpectedResult = "/tests/test1")]
		[TestCase ("/tests/test1ViewModel.cs", ExpectedResult = "/tests/test1")]


		[TestCase ("test2.xaml", ExpectedResult = "test2")]
		[TestCase ("test2.xaml.cs", ExpectedResult = "test2")]
		[TestCase ("test2.cs", ExpectedResult = null)]
		[TestCase ("test2VM.cs", ExpectedResult = "test2")]
		[TestCase ("test2ViewModel.cs", ExpectedResult = "test2")]

		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = "test/pkg2/pkg3/test3")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = "test/pkg2/pkg3/test3")]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = "test/pkg2/pkg3/test3")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = "test/pkg2/pkg3/test3")]
		public string TestGetRootFileName (string fileName)
		{
			_helper.ReturnFileName = fileName;
			return _helper.RootFileNameForActiveDocument;
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = true)]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = false)]
		public bool TestIsActiveFileXamlFile (string fileName)
		{
			_helper.ReturnFileName = fileName;
			return _helper.IsActiveFileXamlFile;
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = true)]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = false)]
		public bool TestIsActiveFileCodeBehindFile (string fileName)
		{
			_helper.ReturnFileName = fileName;
			return _helper.IsActiveFileCodeBehindFile;
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = false)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = true)]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = true)]
		public bool TestIsActiveFileViewModel (string fileName)
		{
			_helper.ReturnFileName = fileName;
			return _helper.IsActiveFileViewModel;
		}


		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = "test/pkg2/pkg3/test3.xaml")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = "test/pkg2/pkg3/test3.xaml")]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = "test/pkg2/pkg3/test3.xaml")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = "test/pkg2/pkg3/test3.xaml")]
		public string TestXamlFileNameForActiveDocument (string fileName)
		{
			_helper.ReturnFileName = fileName;
			return _helper.XamlFileNameForActiveDocument;
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", 0, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", 0, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3.cs", 0, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", 0, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", 0, ExpectedResult = null)]

		[TestCase ("test/pkg2/pkg3/test3.xaml", 1, ExpectedResult = "test/pkg2/pkg3/test3VM.cs")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", 1, ExpectedResult = "test/pkg2/pkg3/test3VM.cs")]
		[TestCase ("test/pkg2/pkg3/test3.cs", 1, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", 1, ExpectedResult = "test/pkg2/pkg3/test3VM.cs")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", 1, ExpectedResult = "test/pkg2/pkg3/test3VM.cs")]

		[TestCase ("test/pkg2/pkg3/test3.xaml", 2, ExpectedResult = "test/pkg2/pkg3/test3ViewModel.cs")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3ViewModel.cs")]
		[TestCase ("test/pkg2/pkg3/test3.cs", 2, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3ViewModel.cs")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3ViewModel.cs")]

		public string TestXamlFileNameForActiveDocument (string fileName, int numberOfTimesToInvokeFileExists)
		{
			_helper.ReturnFileName = fileName;
			_helper.NumberOfGetFileCallsToInvokeBeforeReturningTrue = numberOfTimesToInvokeFileExists;
			return _helper.VMFileNameForActiveDocument;
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]
		public string TestCodeBehindFileNameForActiveDocument (string fileName)
		{
			_helper.ReturnFileName = fileName;
			return _helper.CodeBehindFileNameForActiveDocument;
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", ExpectedResult = "-> XAML")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", ExpectedResult = "-> CB")]
		[TestCase ("test/pkg2/pkg3/test3.cs", ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", ExpectedResult = "-> VM")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", ExpectedResult = "-> VM")]
		public string TestGetStatusPrefix (string fileName)
		{
			return _helper.GetStatusPrefix (fileName);
		}

		[TestCase ("test/pkg2/pkg3/test3.xaml", 2, ExpectedResult = "test/pkg2/pkg3/test3ViewModel.cs")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3.xaml")]
		[TestCase ("test/pkg2/pkg3/test3.cs", 2, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3.xaml")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3.xaml")]

		public string TestToggleVMAndXaml (string fileName, int numberOfTimesToInvokeFileExists)
		{
			_helper.ReturnFileName = fileName;
			_helper.NumberOfGetFileCallsToInvokeBeforeReturningTrue = numberOfTimesToInvokeFileExists;
			_helper.ToggleVMAndXaml ();
			return _helper.FilenamePassedToOpenDocumentMethod;
		}


		[TestCase ("test/pkg2/pkg3/test3.xaml", 2, ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]
		[TestCase ("test/pkg2/pkg3/test3.xaml.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3ViewModel.cs")]
		[TestCase ("test/pkg2/pkg3/test3.cs", 2, ExpectedResult = null)]
		[TestCase ("test/pkg2/pkg3/test3VM.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]
		[TestCase ("test/pkg2/pkg3/test3ViewModel.cs", 2, ExpectedResult = "test/pkg2/pkg3/test3.xaml.cs")]

		public string TestToggleVMAndCodeBehind (string fileName, int numberOfTimesToInvokeFileExists)
		{
			_helper.ReturnFileName = fileName;
			_helper.NumberOfGetFileCallsToInvokeBeforeReturningTrue = numberOfTimesToInvokeFileExists;
			_helper.ToggleVMAndCodeBehind ();
			return _helper.FilenamePassedToOpenDocumentMethod;
		}

		[Test]
		public void CycleXamlCodeBehindViewModel ()
		{
			_helper.ReturnFileName = "test3.xaml";
			_helper.NumberOfGetFileCallsToInvokeBeforeReturningTrue = 2;

			_helper.CycleXamlCodeBehindViewModel ();
			Assert.That (_helper.FilenamePassedToOpenDocumentMethod, Is.EqualTo ("test3.xaml.cs"));
			_helper.ReturnFileName = _helper.FilenamePassedToOpenDocumentMethod;
			_helper.NumberOfTimeGetFileExistsInvoked = 0;

			_helper.CycleXamlCodeBehindViewModel ();
			Assert.That (_helper.FilenamePassedToOpenDocumentMethod, Is.EqualTo ("test3ViewModel.cs"));
			_helper.ReturnFileName = _helper.FilenamePassedToOpenDocumentMethod;
			_helper.NumberOfTimeGetFileExistsInvoked = 0;

			_helper.CycleXamlCodeBehindViewModel ();
			Assert.That (_helper.FilenamePassedToOpenDocumentMethod, Is.EqualTo ("test3.xaml"));
			_helper.ReturnFileName = _helper.FilenamePassedToOpenDocumentMethod;
			_helper.NumberOfTimeGetFileExistsInvoked = 0;


			_helper.NumberOfGetFileCallsToInvokeBeforeReturningTrue = 1;

			_helper.CycleXamlCodeBehindViewModel ();
			Assert.That (_helper.FilenamePassedToOpenDocumentMethod, Is.EqualTo ("test3.xaml.cs"));
			_helper.ReturnFileName = _helper.FilenamePassedToOpenDocumentMethod;
			_helper.NumberOfTimeGetFileExistsInvoked = 0;

			_helper.CycleXamlCodeBehindViewModel ();
			Assert.That (_helper.FilenamePassedToOpenDocumentMethod, Is.EqualTo ("test3VM.cs"));
			_helper.ReturnFileName = _helper.FilenamePassedToOpenDocumentMethod;
			_helper.NumberOfTimeGetFileExistsInvoked = 0;

			_helper.CycleXamlCodeBehindViewModel ();
			Assert.That (_helper.FilenamePassedToOpenDocumentMethod, Is.EqualTo ("test3.xaml"));
			_helper.ReturnFileName = _helper.FilenamePassedToOpenDocumentMethod;
			_helper.NumberOfTimeGetFileExistsInvoked = 0;

		}


	}
}

