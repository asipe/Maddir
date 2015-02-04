using System;

namespace Maddir.Core.Model {
  public class EntryFunc<T> {
    public EntryFunc(Func<IEntry, T> onFile,
                     Func<IEntry, T> onDirectory) {
      mOnFile = onFile;
      mOnDirectory = onDirectory;
    }

    public T Execute(IEntry entry) {
      switch (entry.Type) {
        case EntryType.File:
          return mOnFile.Invoke(entry);
        case EntryType.Directory:
          return mOnDirectory.Invoke(entry);
        default:
          throw new MaddirException("Unknown Command: {0}", entry.Type);
      }
    }

    private readonly Func<IEntry, T> mOnDirectory;
    private readonly Func<IEntry, T> mOnFile;
  }
}