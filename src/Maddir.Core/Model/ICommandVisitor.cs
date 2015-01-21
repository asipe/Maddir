using Maddir.Core.Commands;

namespace Maddir.Core.Model {
  public interface ICommandVisitor {
    void Visit(AddDirectoryCommand command);
  }
}