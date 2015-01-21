using Maddir.Core.Model;

namespace Maddir.Core.Commands {
  public class AddDirectoryCommand : ICommand {
    public AddDirectoryCommand(string path) {
      Properties = new DirectoryProperties(path);
    }

    public DirectoryProperties Properties{get;private set;}

    public void Accept(ICommandVisitor visitor) {
      visitor.Visit(this);
    }
  }
}