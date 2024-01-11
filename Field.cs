using Godot;
using System;
using System.Linq;
using static GameManager;

public class Field : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    private void EvaluateScore()
    {
        GD.Print(CardsInDropZone.OpponentLeft.Count);
        if (CardsInDropZone.OpponentLeft.Count > 0){
            int opponentPowerLeft = CardsInDropZone.OpponentLeft.Sum(card=>card.Power);
            ((Label) GetNode<Label>("FieldZone/FieldZoneLeft/OpponentPowerPanelLeft/OpponentTotalPowerLeft")).Text = opponentPowerLeft.ToString();
        }
        
        /* 
        int opponentPowerMid = _gm.CardsInDropZone.OpponentMid.Sum(item => item.Power);
        ((Label) GetNode<Label>("FieldZone/FieldZoneMid/OpponentPowerPanelMid/OpponentTotalPowerMid")).Text = opponentPowerMid.ToString();
        int opponentPowerRight = _gm.CardsInDropZone.OpponentRight.Sum(item => item.Power);
        ((Label) GetNode<Label>("FieldZone/FieldZoneRight/OpponentPowerPanelRight/OpponentTotalPowerRight")).Text = opponentPowerRight.ToString(); */
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
