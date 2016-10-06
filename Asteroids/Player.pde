class Player{
  //fields
  HitBox hBox;
  int lives;  //lives had
  int clip;  //bullets left in clip
  PVector pos;
  PVector vel;
  PVector acceleration;
  PVector direction;
  PShape playr;
  float speed;
  float rotation;
  float maxSpeed;
  boolean stop;
  
  Player(){
    //initialize variables
    pos = new PVector(width / 2, height / 2);
    vel = new PVector(0, 0);
    acceleration = new PVector(0, 0);
    rotation = 0;
    direction = new PVector(0, 0);
    speed = 0;
    maxSpeed = 5;
    rotation = 0;
    clip = 6;
    lives = 3;
    stop = true;
    
    //make the pshape
    playr = createShape();
    playr.beginShape();
    playr.vertex(20, 0);
    playr.vertex(27, 25);
    playr.vertex(20, 25);
    playr.vertex(17, 15);
    playr.vertex(10, 15);
    playr.vertex(7, 25);
    playr.vertex(0, 25);
    playr.vertex(6, 0);
    playr.vertex(20, 0);
    playr.endShape();
    playr.setFill(color(255,255,255));
    playr.setStroke(color(255,255,255));
    
    //make the hitbox
    hBox = new HitBox(width / 2, height / 2, width / 2 + 27, height / 2 + 27);
    
  }
  
  //move the player
  void move(){
    if (stop){
      vel = PVector.mult(vel, 0);
      if (speed > 0){
        stop = false;
      }
    }
    else{
      //get acceleration
      direction = PVector.fromAngle(radians(rotation - 90));
      acceleration = PVector.mult(direction, speed);

      //increment the velocity and move
      vel.add(acceleration);
      vel.limit(maxSpeed);
      pos.add(PVector.mult(vel, deltaTime * 60));
      
      //screen wrapping
      if (pos.x < -playr.width){
        pos.x = width + playr.width - 1;
      }
      else if (pos.x > width + playr.width){
        pos.x = -playr.width;
      }
      if (pos.y < -playr.height){
        pos.y = height + playr.height - 1;
      }
      else if (pos.y > height + playr.height){
        pos.y = -playr.height;
      }
      
      //update hitbox
      hBox.minX = pos.x;
      hBox.maxX = pos.x + 27;
      hBox.minY = pos.y;
      hBox.maxY = pos.y + 27;
      
      //reset the acceleration
      acceleration.set(0, 0);
    }
  }
  
  //fire bullet
  void fire(){
    if (clip == 0 && bullets.size() < 6){
      clip = 6;
    }
    else if (clip > 0){
      bullets.add(new Bullet(pos.x, pos.y, rotation - 90));
      clip--;
            
      //play music
      fireSound.rewind();
      fireSound.play();
    }
  }
  
  //add rotation in
  void display(){
    pushMatrix();
    translate(pos.x, pos.y);
    rotate(radians(rotation));
    shape(playr);
    popMatrix();
  }
  
  
}