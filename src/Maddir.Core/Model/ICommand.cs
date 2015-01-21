namespace Maddir.Core.Model {
  public interface ICommand {
    void Accept(ICommandVisitor visitor);
  }
}