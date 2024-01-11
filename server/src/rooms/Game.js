import { Room } from "@colyseus/core";
import { GameState } from "./schema/GameState.js";
import { FieldState } from "./schema/FieldState.js";

export class Game extends Room {

    //support only 2 clients connected
    maxClients = 2;
    fields = ["Arctic","Lava","Grass","Ocean","Desert","Hole","Void","Blackhole"];
    _initFields = {left:"",mid:"",right:""}

    //determine what should happen when a room is created
    onCreate(options) {
        //init 3 random fields
        for (var field in this._initFields){
            this._initFields[field] = this.fields.splice(Math.floor(Math.random()*this.fields.length), 1)[0];
        }
        
        console.log("Game Room created!", options);
        console.log("Room ID: " + this.roomId);
        console.log("Game Room fields!", this._initFields);

        //set a custom state from a created schema
        this.setState(new GameState());
        
        //when a message is received of type "message," broadcast it with the type "server-message" to all clients
        this.onMessage("message", (client, message) => {
            console.log("Game Room received message from", client.sessionId, ":", message);
            this.broadcast("server-message", message, {except: client});
        });

        //when a message is received of type "game-message," broadcast it with the type "game-message" to all clients except for the one that sent it
        this.onMessage("game-message", (client, message) => {
            this.broadcast("game-message", message, { except: client });
        });

        //when a message is received of type "client-request", respond to the sending client with the appropriate data
        this.onMessage("client-request", (client, message) => {
            console.log("Sent Via Game.js", message);
            client.send("client-request", {kind: message}, { except: client });
        })

    }

    //determine what should happen when a client joins
    onJoin(client, options) {
        console.log(client.sessionId, "joined!");
        //TODO: fix to use actual GameState
        //Trigger game env setup
        this.broadcast("server-message", {type:"game_start",FieldState:this._initFields});
        //Trigger first turn
        client.send("client-request",  {kind: "start_turn"})
    }

    //determine what should happen when a client leaves
    onLeave(client, consented) {
        console.log(client.sessionId, "left!");
        this.broadcast("server-message", `${client.sessionId} left.`);
    }

    //determine what should happen when a room is closed
    onDispose() {
        console.log("room", this.roomId, "disposing...");
    }

}
