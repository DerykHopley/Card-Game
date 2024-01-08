import * as schema from "@colyseus/schema";
import { FieldState } from "./FieldState.js";

//define custom state schema
export class GameState extends schema.Schema {

  constructor() {
    super();
    this.roomFields = new schema.MapSchema();
  }
}

schema.defineTypes(GameState, {
  roomFields: {map: FieldState},
});
