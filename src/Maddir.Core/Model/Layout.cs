namespace Maddir.Core.Model {
  public class Layout {
    public Layout(params ICommand[] commands) {
      Commands = commands;
    }

    public ICommand[] Commands{get;private set;}
  }
}