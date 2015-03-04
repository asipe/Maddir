using System.Collections;
using Maddir.Core.Model;
using Maddir.Core.TreeGeneration;
using Maddir.Core.Utility;
using NUnit.Framework;

namespace Maddir.UnitTests.Core.TreeGeneration {
  [TestFixture]
  public class SplitterTest : BaseTestCase {
    public class Validation {
      public Validation() {
        ContentStartDelimiter = '[';
        ContentEndDelimiter = ']';
      }

      public char ContentEndDelimiter{get;set;}
      public char ContentStartDelimiter{get;set;}
      public string Markup{get;private set;}
      public IEntry[] Expected{get;private set;}

      public Validation SetMarkup(params string[] lines) {
        Markup = StringUtils.ToNewLineSepString(lines);
        return this;
      }

      public Validation SetExpected(params IEntry[] expected) {
        Expected = expected;
        return this;
      }
    }

    [TestCaseSource("GetUsageTests")]
    public void TestUsage(Validation validation) {
      var splitter = new Splitter(new Settings {
                                                 ContentStartDelimiter = validation.ContentStartDelimiter,
                                                 ContentEndDelimiter = validation.ContentEndDelimiter
                                               });
      Assert.That(splitter.Split(validation.Markup), Are.EqualTo(validation.Expected));
    }

    public IEnumerable GetUsageTests() {
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("")
                                      .SetExpected());
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("    ")
                                      .SetExpected());
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt []")
                                      .SetExpected(new FileEntry(0, "file0.txt", "")));
      yield return new TestCaseData(new Validation {
                                                     ContentStartDelimiter = '<',
                                                     ContentEndDelimiter = '>'
                                                   }
                                      .SetMarkup("f  file0.txt <>")
                                      .SetExpected(new FileEntry(0, "file0.txt", "")));
      yield return new TestCaseData(new Validation {
                                                     ContentStartDelimiter = '|',
                                                     ContentEndDelimiter = '|'
                                                   }
                                      .SetMarkup("f  file0.txt ||")
                                      .SetExpected(new FileEntry(0, "file0.txt", "")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("  f  file0.txt []   ")
                                      .SetExpected(new FileEntry(0, "file0.txt", "")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [0]")
                                      .SetExpected(new FileEntry(0, "file0.txt", "0")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f    file0.txt  [a]")
                                      .SetExpected(new FileEntry(1, "file0.txt", "a")));
      yield return new TestCaseData(new Validation {
                                                     ContentStartDelimiter = '<',
                                                     ContentEndDelimiter = '>'
                                                   }
                                      .SetMarkup("f    file0.txt  <a>")
                                      .SetExpected(new FileEntry(1, "file0.txt", "a")));
      yield return new TestCaseData(new Validation {
                                                     ContentStartDelimiter = '|',
                                                     ContentEndDelimiter = '|'
                                                   }
                                      .SetMarkup("f    file0.txt  |a|")
                                      .SetExpected(new FileEntry(1, "file0.txt", "a")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f      file0.txt    [abc]")
                                      .SetExpected(new FileEntry(2, "file0.txt", "abc")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("d  dir0")
                                      .SetExpected(new DirectoryEntry(0, "dir0")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("d    dir0")
                                      .SetExpected(new DirectoryEntry(1, "dir0")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("d      dir0")
                                      .SetExpected(new DirectoryEntry(2, "dir0")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt []",
                                                 "f  file1.txt []")
                                      .SetExpected(new FileEntry(0, "file0.txt", ""),
                                                   new FileEntry(0, "file1.txt", "")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [c1]",
                                                 "f  file1.txt [c2 c2 c2]",
                                                 "f  file2.txt [ c3 ]")
                                      .SetExpected(new FileEntry(0, "file0.txt", "c1"),
                                                   new FileEntry(0, "file1.txt", "c2 c2 c2"),
                                                   new FileEntry(0, "file2.txt", " c3 ")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [c1]",
                                                 "f    file1.txt [c2 c2 c2]",
                                                 "f      file2.txt [ c3 ]")
                                      .SetExpected(new FileEntry(0, "file0.txt", "c1"),
                                                   new FileEntry(1, "file1.txt", "c2 c2 c2"),
                                                   new FileEntry(2, "file2.txt", " c3 ")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("d  dir1",
                                                 "d  dir2",
                                                 "d  dir3")
                                      .SetExpected(new DirectoryEntry(0, "dir1"),
                                                   new DirectoryEntry(0, "dir2"),
                                                   new DirectoryEntry(0, "dir3")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("d  dir1",
                                                 "d    dir2",
                                                 "d      dir3")
                                      .SetExpected(new DirectoryEntry(0, "dir1"),
                                                   new DirectoryEntry(1, "dir2"),
                                                   new DirectoryEntry(2, "dir3")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt []",
                                                 "d  dir0")
                                      .SetExpected(new FileEntry(0, "file0.txt", ""),
                                                   new DirectoryEntry(0, "dir0")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt []",
                                                 "d  dir0",
                                                 "f    file1.txt [c1]",
                                                 "f    file2.txt [c2]",
                                                 "d    dir1",
                                                 "d  dir2")
                                      .SetExpected(new FileEntry(0, "file0.txt", ""),
                                                   new DirectoryEntry(0, "dir0"),
                                                   new FileEntry(1, "file1.txt", "c1"),
                                                   new FileEntry(1, "file2.txt", "c2"),
                                                   new DirectoryEntry(1, "dir1"),
                                                   new DirectoryEntry(0, "dir2")));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [multiple",
                                                 "line",
                                                 "contents]")
                                      .SetExpected(new FileEntry(0, "file0.txt", StringUtils.ToNewLineSepString("multiple", "line", "contents"))));
      yield return new TestCaseData(new Validation {
                                                     ContentStartDelimiter = '(',
                                                     ContentEndDelimiter = ')'
                                                   }
                                      .SetMarkup("f  file0.txt (multiple",
                                                 "line",
                                                 "contents)")
                                      .SetExpected(new FileEntry(0, "file0.txt", StringUtils.ToNewLineSepString("multiple", "line", "contents"))));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [",
                                                 "multiple",
                                                 "line",
                                                 "contents",
                                                 "]")
                                      .SetExpected(new FileEntry(0, "file0.txt", StringUtils.ToNewLineSepString("", "multiple", "line", "contents", ""))));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [content0]",
                                                 "f  file1.txt [content1a",
                                                 "content1b]")
                                      .SetExpected(new FileEntry(0, "file0.txt", "content0"),
                                                   new FileEntry(0, "file1.txt", StringUtils.ToNewLineSepString("content1a", "content1b"))));
      yield return new TestCaseData(new Validation()
                                      .SetMarkup("f  file0.txt [content0]",
                                                 "f  file1.txt [content1a",
                                                 "content1b]",
                                                 "d  dir0",
                                                 "d    dir1",
                                                 "f      file3.txt [content3a",
                                                 "content3b",
                                                 "content3c]",
                                                 "d  dir2",
                                                 "f  file3.txt []")
                                      .SetExpected(new FileEntry(0, "file0.txt", "content0"),
                                                   new FileEntry(0, "file1.txt", StringUtils.ToNewLineSepString("content1a", "content1b")),
                                                   new DirectoryEntry(0, "dir0"),
                                                   new DirectoryEntry(1, "dir1"),
                                                   new FileEntry(2, "file3.txt", StringUtils.ToNewLineSepString("content3a", "content3b", "content3c")),
                                                   new DirectoryEntry(0, "dir2"),
                                                   new FileEntry(0, "file3.txt", "")));
    }
  }
}