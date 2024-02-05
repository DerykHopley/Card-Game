using Godot;
using System;

namespace CardCollection {
    public class MoveCard : KinematicBody2D
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        [Export] public int speed = 200;
        [Export] public Vector2 target;
        public Vector2 velocity = new Vector2();

        public override void _Ready()
        {
            target = Position;
        }

        public override void _PhysicsProcess(float delta)
        {
            velocity = Position.DirectionTo(target) * speed;
            // LookAt(target);
            if (Position.DistanceTo(target) > 5)
            {
                velocity = MoveAndSlide(velocity);
            }
        }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    }
}