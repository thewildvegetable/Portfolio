class Asteroid{
  //fields
  HitBox hBox;
  boolean breakApart;  //will this asteroid break apart when hit by a bullet?
  PVector pos;
  PVector vel;
  PVector direction;
  PShape aster;
  
  Asteroid(boolean initAsteroid, float xPos, float yPos, float direct){
    //initialize variables
    breakApart = initAsteroid;
    pos = new PVector(xPos, yPos);
    direction = PVector.sub(play.pos, pos).normalize();
    vel = new PVector(0, 0);
    
    //create hitbox
    if (breakApart){
      hBox = new HitBox(pos.x, pos.y, pos.x + 50, pos.y + 50);
    }
    else{
      hBox = new HitBox(pos.x, pos.y, pos.x + 25, pos.y + 25);
    }
    
    //make PShape
    aster = createShape();
    aster.beginShape();
    aster.vertex(0, 5);
    aster.vertex(5, 0);
    aster.vertex(10, 15);
    aster.vertex(15, 10);
    aster.vertex(25, 5);
    aster.vertex(35, 10);
    aster.vertex(40, 0);
    aster.vertex(50, 10);
    aster.vertex(50, 25);
    aster.vertex(45, 35);
    aster.vertex(40, 45);
    aster.vertex(40, 50);
    aster.vertex(20, 50);
    aster.vertex(15, 35);
    aster.vertex(25, 30);
    aster.vertex(10, 20);
    aster.vertex(5, 25);
    aster.vertex(0, 5);
    aster.endShape();
    aster.setFill(color(255,255,255));
    aster.setStroke(color(255,255,255));
  }
  
  //asteroid was hit, have it seperate
  void seperate(){
    //add 2 new asteroids
    asteroids.add(new Asteroid(false, pos.x + 10, pos.y + 10, atan2(-direction.y, -direction.x)));
    asteroids.add(new Asteroid(false, pos.x - 10, pos.y - 10, atan2(direction.y, direction.x)));
  }
  
  void display(){
    pushMatrix();
    translate(pos.x, pos.y);
    if (!breakApart){
      scale(.5);
    }
    shape(aster);
    popMatrix();
  }
  
  void move(){
    vel = PVector.mult(direction, 1);
    pos.add(PVector.mult(vel, deltaTime * 60));
    
    //reset velocity
    vel = PVector.mult(vel, 0);
    
    //screen wrapping
    if (pos.x < -aster.width){
      pos.x = width + aster.width - 1;
      
    }
    else if (pos.x > width + aster.width){
      pos.x = -aster.width;
      
    }
    else if (pos.y < -aster.height){
      pos.y = height + aster.height - 1;
      
    }
    else if (pos.y > height + aster.height){
      pos.y = -aster.height;
      
    }
    
    //update hitbox
    hBox.minX = pos.x;
    hBox.minY = pos.y;
    if (breakApart){
      hBox.maxX = pos.x + 50;
      hBox.maxY = pos.y + 50;
    }
    else{
      hBox.maxX = pos.x + 25;
      hBox.maxY = pos.y + 25;
    }
    
  }
}