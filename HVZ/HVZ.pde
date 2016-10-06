
//Beginning to code autonomous agents
//Will implement inheritance with a Vehicle class and a Seeker class


Seeker s;
ArrayList<Obstacle> obst;
ArrayList<Seeker> zombie;
ArrayList<Fleeer> human;
static boolean D_KEY_PRESSED;
float circDist;
float circRad;

void setup() {
  size(700, 700);
  obst = new ArrayList<Obstacle>();
  zombie = new ArrayList<Seeker>();
  human = new ArrayList<Fleeer>();
  
  //fill arraylists
  int radius = 0;
  for (int i = 0; i < 5; i++){
    radius = (int) random(40, 90);
    obst.add(new Obstacle(random(0, width - 2 * radius), random(0, height - 2 * radius), radius));
  }
  zombie.add(new Seeker(100, 100, 40, 2, 0.1));
  zombie.add(new Seeker(width - 200, 100, 40, 2, 0.1));
  float x = 0;
  float y = 0;
  for (int i = 0; i < 5; i++){
    x = random(width / 2 - 150, width / 2 + 150);
    y = random(height / 2 - 150, height / 2 + 150);
    human.add(new Fleeer(x, y, 40, 2.5, 0.5));
  }
  
  //initialize variables
  D_KEY_PRESSED = false;
  circRad = .5;
  circDist = 2;
}

void draw() {
  background(255);
  
  //turn on/off debug lines
  if (D_KEY_PRESSED && zombie.get(0).debuggin){
      //turn off debug lines
    for (int i = 0; i < zombie.size(); i++){
      zombie.get(i).debuggin = false;
    }
    for (int i = 0; i < human.size(); i++){
      human.get(i).debuggin = false;
    }
  }
  else if (D_KEY_PRESSED){
    //turn on debug lines
    for (int i = 0; i < zombie.size(); i++){
      zombie.get(i).debuggin = true;
    }
    for (int i = 0; i < human.size(); i++){
      human.get(i).debuggin = true;
    }
  }
  
  //update the vehicles
  for (int i = 0; i < zombie.size(); i++){
    zombie.get(i).update();
  }
  for (int i = 0; i < human.size(); i++){
    human.get(i).update();
  }
  
  //check for collisions
  Seeker temp = null;  //temp variable for creating new zombie on collision
  for (int i = 0; i < human.size(); i++){
    for (int j = 0; j < zombie.size(); j++){
      if (human.get(i).collide(zombie.get(j))){
        //make new zombie and remove human from the human array
        temp = new Seeker(human.get(i).position.x, human.get(i).position.y, human.get(i).radius, 2, human.get(i).maxForce);
        zombie.add(temp);
        human.remove(i);
        i--;
        break;
      }
    }
  }
  
  //display vehicles
  for (int i = 0; i < zombie.size(); i++){
    zombie.get(i).display();
  }
  for (int i = 0; i < human.size(); i++){
    human.get(i).display();
  }
  
  //display obstacles
  for (int i = 0; i < obst.size(); i++){
    obst.get(i).display();
  }
}


void keyPressed(){
  if(key == 'D' || key == 'd'){
     D_KEY_PRESSED = true;
  }
}

void mousePressed(){
  human.add(new Fleeer(mouseX, mouseY, 40, 2.5, 0.5));
}