using System;

namespace Maddir.Core.Model {
  public class EntryAction {
    public EntryAction(Action<IEntry> onFile,
                       Action<IEntry> onDirectory) {
      mOnFile = onFile;
      mOnDirectory = onDirectory;
    }

    public void Execute(IEntry entry) {
      switch (entry.Type) {
        case EntryType.File:
          mOnFile.Invoke(entry);
          break;
        case EntryType.Directory:
          mOnDirectory.Invoke(entry);
          break;
        default:
          throw new MaddirException("Unknown Command: {0}", entry.Type);
      }
    }

    private readonly Action<IEntry> mOnDirectory;
    private readonly Action<IEntry> mOnFile;
  }
}