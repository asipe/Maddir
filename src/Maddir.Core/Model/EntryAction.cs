using System;

namespace Maddir.Core.Model {
  public class EntryAction {
    public EntryAction(Action<FileEntry> onFile,
                       Action<DirectoryEntry> onDirectory) {
      mOnFile = onFile;
      mOnDirectory = onDirectory;
    }

    public void Execute(IEntry entry) {
      switch (entry.Type) {
        case EntryType.File:
          mOnFile.Invoke((FileEntry)entry);
          break;
        case EntryType.Directory:
          mOnDirectory.Invoke((DirectoryEntry)entry);
          break;
        default:
          throw new MaddirException("Unknown Command: {0}", entry.Type);
      }
    }

    private readonly Action<DirectoryEntry> mOnDirectory;
    private readonly Action<FileEntry> mOnFile;
  }
}