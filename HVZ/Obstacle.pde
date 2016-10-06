class Obstacle{
  //fields
  PVector position;
  float radius;
  PImage obst;
  
  //constructor
  Obstacle(float x, float y, float r){
    position = new PVector(x, y);
    radius = r;
    
    //PImage initialization
    int imgNum = (int) random(3);
    if (imgNum == 0){
      obst = loadImage("deadtree3.png");
    }
    else if (imgNum == 1){
      obst = loadImage("tree2_1.png");
    }
    else{
      obst = loadImage("tree-strange.png");
    }
    obst.resize((int) radius, (int) radius);
  }
  
  //display obstacle
  void display(){
    image(obst, position.x, position.y);
  }
}