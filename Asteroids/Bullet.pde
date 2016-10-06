class Bullet{
  //fields
  HitBox hBox;
  PVector pos;
  PVector vel;
  PShape dart;
  
  Bullet(float xPos, float yPos, float rotation){
    //initialize variables
    pos = new PVector(xPos, yPos);
    vel = PVector.fromAngle(radians(rotation));
    vel = PVector.mult(vel, 2);
    
    //make the pshape
    dart = createShape();
    dart.beginShape();
    dart.vertex(0, 5);
    dart.vertex(1, 0);
    dart.vertex(2, 5);
    dart.vertex(0, 5);
    dart.endShape();
    dart.setFill(color(255,255,255));
    dart.setStroke(color(255,255,255));
    
    //make hitbox
    hBox = new HitBox(pos.x, pos.y, pos.x + 5, pos.y + 5);
  }
  
  //add rotation in
  void display(){
    pushMatrix();
    translate(pos.x, pos.y);
    shape(dart);
    popMatrix();
  }
  
  void move(){
    pos.add(PVector.mult(vel, deltaTime * 60));
    
    //update hitbox
    hBox.minX = pos.x;
    hBox.maxX = pos.x + 5;
    hBox.minY = pos.y;
    hBox.maxY = pos.y + 5;
  }
}