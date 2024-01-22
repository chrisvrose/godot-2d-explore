using Godot;
using System;

public partial class Door : StaticBody2D
{
	public enum DoorState{
		OPEN,CLOSED
	}
	
	[Export(PropertyHint.Enum,"Open,Closed")]
	public DoorState doorState = DoorState.OPEN;

	private AnimatedSprite2D doorAnimatedSprite;
	private CollisionShape2D collisionShape2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		doorAnimatedSprite = GetNode<AnimatedSprite2D>("DoorSprite");
		collisionShape2D = GetNode<CollisionShape2D>("DoorCollisionShape");

		setDoorState(doorState);
	}

	private void setDoorState(DoorState doorState)
    {
        GD.Print("Door> Toggle to ", doorState);
        doorAnimatedSprite.Animation = doorState.ToString();
		this.doorState = doorState;
        setCollisionState(doorState);
    }

    private void setCollisionState(DoorState doorState)
    {
        bool isDoorPlayerCollidable = doorState == DoorState.CLOSED;

        uint isDoorPlayerCollidableBit = isDoorPlayerCollidable ? 1u : 0u;

        this.CollisionLayer = 2 | isDoorPlayerCollidableBit;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}



	public void toggleDoor(){
		if(doorState==DoorState.OPEN){
			setDoorState(DoorState.CLOSED);
		}else{
			setDoorState(DoorState.OPEN);
		}
	}
}
