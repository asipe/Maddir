using System;

namespace Maddir.Core.Model {
  public class EntryFunc<T> {
    public EntryFunc(Func<FileEntry, T> onFile,
                     Func<DirectoryEntry, T> onDirectory) {
      mOnFile = onFile;
      mOnDirectory = onDirectory;
    }

    public T Execute(IEntry entry) {
      switch (entry.Type) {
        case EntryType.File:
          return mOnFile.Invoke((FileEntry)entry);
        case EntryType.Directory:
          return mOnDirectory.Invoke((DirectoryEntry)entry);
        default:
          throw new MaddirException("Unknown Command: {0}", entry.Type);
      }
    }

    private readonly Func<DirectoryEntry, T> mOnDirectory;
    private readonly Func<FileEntry, T> mOnFile;
  }
}