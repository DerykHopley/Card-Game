import * as schema from "@colyseus/schema";

export class FieldState extends schema.Schema {
    constructor(left, mid, right) {
        super();
        this.left = left;
        this.mid = mid;
        this.right = right;
    }
  }
  schema.defineTypes(FieldState, {
    left: "string",
    mid: "string",
    right: "string",
  });