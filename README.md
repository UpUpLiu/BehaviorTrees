# Behavior Trees

A simple C# example of Behavior Trees + Editor.

## Introduction

**Behavior Trees** (BT) is a simple, scalable, modular solution that represents complex AI-behaviors and provides easy-to-maintain and configure logic.

<img align="right" src="/Images/bt.png">

They have been widely used in robotics and gaming since mid 2000, in particular, such game engines as Unreal, Unity, CryEngine use BT. BT have several advantages over FSM, such as *Maintainability* (nodes (or subtrees) can be designed independent from each other and can be independent changed), *Scalability* (complex BT can be decomposed into small sub-trees), *Reusability* (subtrees can be used as independent nodes)

You can learn more about Behavior Trees at the following links:

* [Behavior tree (Wikipedia)](https://en.wikipedia.org/wiki/Behavior_tree_(artificial_intelligence,_robotics_and_control))
* [Behavior trees for AI: How they work (Gamasutra)](http://www.gamasutra.com/blogs/ChrisSimpson/20140717/221339/Behavior_trees_for_AI_How_they_work.php)
* [Understanding Behavior Trees](http://aigamedev.com/open/articles/bt-overview/)
* [Behavior Trees for Next-Gen Game AI (Video)](http://aigamedev.com/insider/presentation/behavior-trees/)


## About project

This project demonstrates the concept and working principle of the Behavior Trees. Therefore, I tried to make it as simple and laconic as possible. You can fork, adapt and extend the project to suit your needs.

* The project includes **BehaviorTrees** library (C#, .Net Standart) with the main types of nodes: actions, conditions, composites and decorators (20 in total) as well as auxiliary classes. You can add your nodes inheriting from existing ones. Trees can be serialized in json.

* **BehaviorTreesEditor** (.Net Framework WinForms) allows you to edit trees with a simple TreeList control, to save, to load and to run trees.

* **BehaviorTrees.Example1** (.Net Standart) contains simple example of the Behavior Tree with custom node "Move".

![Behavior Trees](/Images/editor.png "Editor")

## Example

Example of custom node "Move" and tree creation using TreeBuilder:

```C#
[DataContract]
[BTNode("Move", "Example")]
public class Move : Node
{
    Point _position;
    bool _completed;

    [DataMember]
    public Point Position
    {
        get { return _position; }
        set {
            _position = value;
            Root.SendValueChanged(this);
        }
    }

    public Move(Point pos)
    {
        Position = pos;
    }

    public override string NodeParameters => $"Move To ({Position.X}, {Position.Y})";

    protected override void OnActivated()
    {
        base.OnActivated();
        Log.Write($"{Owner.Name} moving to {Position}");
        Task.Delay(1000).ContinueWith(_ => completed = true);
    }

    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Log.Write($"{Owner.Name} moving completed ");
    }

    protected override ExecutingStatus OnExecuted()
    {
        return completed ? ExecutingStatus.Success : ExecutingStatus.Running;
    }
}

public static class Example1
{
    public static void Create()
    {
        var exampleBT = new CTreeBuilder<Node>(new Sequence())
            .AddWithChild(new Loop(3))
                .AddWithChild(new Sequence())
                    .Add(new Move(new Point(0, 0)))
                    .Add(new Move(new Point(20, 0)))
                    .AddWithChild(new Delay(2))
                        .Add(new Move(new Point(0, 20)))
                    .Up()
                .Up()
            .Up()
        .ToTree();

        Engine.Instance.LoadScene(new List<Entity> { new Entity("Actor1") },
            new BTScript("MovingTest", exampleBT));
    }
}
```

## Other BT implementations on the Github: ##

* [BehaviorTree.CPP](https://github.com/BehaviorTree/BehaviorTree.CPP) (C++)
* [Fluent Behavior Tree](https://github.com/aequasi/fluent-behavior-tree) (Typescript)
* [Go Behave](https://github.com/askft/go-behave) (Go)
* [Fluid Behavior Tree](https://github.com/ashblue/fluid-behavior-tree) (Unity, C#)

## Third party in this project

* [TreeView with Columns](https://www.codeproject.com/Articles/23746/TreeView-with-Columns) by jkristia 
* [Generic Tree container](https://www.codeproject.com/Articles/12592/Generic-Tree-T-in-C) by peterchen 
* Visual Studio 2015 Image Library. [Microsoft Software License Terms](http://download.microsoft.com/download/0/6/0/0607D8EA-9BB7-440B-A36A-A24EB8C9C67E/Visual%20Studio%202015%20Image%20Library%20EULA.docx)

## License

Copyright (c) 2015-2019 Eugeny Novikov. Code under MIT license.