//Jeffrey Karger

//import libraries
import ddf.minim.*;

//declare globals
ArrayList<Asteroid> asteroids;
ArrayList<Bullet> bullets;
Player play;
State states;
AudioPlayer fireSound;
AudioPlayer bgSound;
AudioPlayer dieSound;
Minim minim;    //audio context
static boolean UP_KEY_PRESSED, LEFT_KEY_PRESSED, RIGHT_KEY_PRESSED, SPACE_KEY_PRESSED;
float lastFrameTime, thisFrameTime, deltaTime;

void setup(){
  size(700, 700);
  colorMode(RGB, 255, 255, 255);
  
  //initialize
  states = State.HOME;
  play = new Player();
  asteroids = new ArrayList<Asteroid>();
  bullets = new ArrayList<Bullet>();
  minim = new Minim(this);
  bgSound = minim.loadFile("Mass Effect 3 Soundtrack - The Fleets Arrive.mp3", 2048);
  dieSound = minim.loadFile("deathSound.mp3", 2048);
  fireSound = minim.loadFile("bulletLaunch.mp3", 2048);
  UP_KEY_PRESSED = false;
  LEFT_KEY_PRESSED = false;
  RIGHT_KEY_PRESSED = false;
  SPACE_KEY_PRESSED = false;
  lastFrameTime = 0;
  thisFrameTime = 0;
  deltaTime = 0;
  
  //start the background song
  bgSound.loop();
}

void draw(){
  background(0);
  
  //check what state the game is in
  if (states == State.HOME){
    //print title
    textSize(64);
    fill(255);
    text("ASTEROIDS", (width / 2) - 180, 100);
    
    //place buttons
    fill(200, 0, 0);
    rect((width / 2) - 180, height / 2, 100, 50);
    rect((width / 2) + 50, height / 2, 150, 50);
    
    //Add text to the buttons
    textSize(32);
    fill(255);
    text("Play", (width / 2) - 164, height / 2 + 35);
    text("Controls", (width / 2) + 60, height / 2 + 35);
    
    //change state
    if (mousePressed && mouseX >= ((width / 2) - 180) && mouseX <= ((width / 2) - 80) && mouseY >= (height / 2) && mouseY <= ((height / 2) + 50)) {
      states = State.GAME;
      
      //spawn asteroids
      Asteroid temp;
      int quad = 0;
      for (int i = 0; i < 6; i++){
        //decide what quadrant the asteroid will spawn in
        quad = (int) random(1, 5);
        if (quad == 1){
          temp = new Asteroid(true, random(0, width / 2 - 200), random(0, height / 2 - 200), random(0, 360));
        }
        else if (quad == 2){
          temp = new Asteroid(true, random(0, width / 2 + 200), random(0, height / 2 - 200), random(0, 360));
        }
        else if (quad == 3){
          temp = new Asteroid(true, random(0, width / 2 - 200), random(0, height / 2 + 200), random(0, 360));
        }
        else{
          temp = new Asteroid(true, random(0, width / 2 + 200), random(0, height / 2 + 200), random(0, 360));
        }
        
        //add the asteroid to the list
        asteroids.add(temp);
      }
    }
    if (mousePressed && mouseX >= ((width / 2) + 50) && mouseX <= ((width / 2) + 200) && mouseY >= (height / 2) && mouseY <= ((height / 2) + 50)) {
      states = State.CONTROLS;
    }
  }
  else if (states == State.CONTROLS){
    //display the controls
    textSize(64);
    fill(255);
    text("Space bar to fire", 100, 100);
    text("bullets/reload", 100, 200);
    text("WASD to move", 100, 300);
    
    //place button to homescreen
    fill(200, 0, 0);
    rect((width / 2) - 50, height / 2, 100, 50);

    //Add text to the buttons
    textSize(32);
    fill(255);
    text("Home", (width / 2) - 45, height / 2 + 35);

    //check if player is clicking the start button
    if (mousePressed && mouseX >= ((width / 2) - 50) && mouseX <= ((width / 2) + 50) && mouseY >= (height / 2) && mouseY <= ((height / 2) + 50)) {
      states = State.HOME;
    }
  }
  else if (states == State.GAME_OVER){
    //display game over
    textSize(64);
    fill(255);
    text("GAME OVER!", (width / 2) - 100, 100);
    
    //place button to homescreen
    fill(200, 0, 0);
    rect((width / 2) - 50, height / 2, 100, 50);

    //Add text to the buttons
    textSize(32);
    fill(255);
    text("Home", (width / 2) - 45, height / 2 + 35);

    //check if player is clicking the start button
    if (mousePressed && mouseX >= ((width / 2) - 50) && mouseX <= ((width / 2) + 50) && mouseY >= (height / 2) && mouseY <= ((height / 2) + 50)) {
      states = State.HOME;
    }
  }
  else{
    //remove frame rate cap (codes tolen from ryan geyer's post on the discussion boards
    lastFrameTime = thisFrameTime;
    thisFrameTime = millis();
    deltaTime = (thisFrameTime-lastFrameTime)/1000; //Divide by 1000 to convert from millis to seconds
    
    //check for keyboard input
    //increment/decrement speed
    if(UP_KEY_PRESSED){
      play.speed += .1;
      play.stop = false;
    }
    else{
      if(!play.stop){
        play.speed -= .03;
        if (play.speed <= 0){
          play.stop = true;
          play.speed = 0;
        }
      }
    }
    //rotate player
    if(RIGHT_KEY_PRESSED){
      //update direction
      play.rotation += 15;
      if (play.rotation >= 360){
        play.rotation -= 360;
      }
      RIGHT_KEY_PRESSED = false;
    }
    if(LEFT_KEY_PRESSED){
      //update direction
      play.rotation -= 15;
      if (play.rotation < 0){
        play.rotation += 360;
      }
      LEFT_KEY_PRESSED = false;
    }
    //fire a bullet
    if(SPACE_KEY_PRESSED){
      play.fire();
      
      //reset space key to force them to press it again
      SPACE_KEY_PRESSED = false;
    }
    
    //update player's position
    play.move();
    
    //update positions of bullets
    for (int i = 0; i < bullets.size(); i++){
      bullets.get(i).move();
    }
    
    //check for bullet to asteroid collisions
    for (int i = 0; i < bullets.size(); i++){
      for (int j = 0; j < asteroids.size(); j++){
        if (bullets.get(i).hBox.Collide(asteroids.get(j).hBox.minX, asteroids.get(j).hBox.maxX, asteroids.get(j).hBox.minY, asteroids.get(j).hBox.maxY)){
          if (asteroids.get(j).breakApart){
            asteroids.get(j).seperate();
          }
          asteroids.get(j).pos.x = width + 100;
          bullets.get(i).pos.x = width + 100;
        }
      }
    }
    
    //update asteroids positions
    for (int i = 0; i < asteroids.size(); i++){
      asteroids.get(i).move();
    }
    
    //check for player to asteroid collisions
    for (int j = 0; j < asteroids.size(); j++){
      if (play.hBox.Collide(asteroids.get(j).hBox.minX, asteroids.get(j).hBox.maxX, asteroids.get(j).hBox.minY, asteroids.get(j).hBox.maxY)){
        //activate new life
        play.lives --;
        play.clip = 6;
        play.pos.x = width / 2;
        play.pos.y = height / 2;
        play.speed = 0;
        play.stop = true;
        play.hBox.minX = play.pos.x;
        play.hBox.maxX = play.pos.x + 27;
        play.hBox.minY = play.pos.y;
        play.hBox.maxY = play.pos.y + 27;
        
        //move asteroids in center of map away
        for (int i = 0; i < asteroids.size(); i++){
          if (asteroids.get(i).hBox.minX <= width / 2 && asteroids.get(i).hBox.maxX >= width / 2 && asteroids.get(i).hBox.minY <= height / 2 && asteroids.get(i).hBox.maxY >= height / 2){
            asteroids.get(i).pos.x += 200;
          }
        }
        dieSound.rewind();
        dieSound.play(500);
        break;
      }
    }
    
    //place everything on screen
    play.display();
    for (int i = 0; i < bullets.size(); i++){
      bullets.get(i).display();
    }
    for (int i = 0; i < asteroids.size(); i++){
      asteroids.get(i).display();
    }
    
    //display clip and lives
    textSize(32);
    fill(255);
    text("Clip: " + play.clip, 0, 50);
    text("Lives: " + (int) play.lives, width - 150, 50);
    
    //check if asteroid or bullet is offscreen (and thus needs to be removed)
    for (int i = 0; i < bullets.size(); i++){
      if(bullets.get(i).pos.x > width || bullets.get(i).pos.x < 0 || bullets.get(i).pos.y > height || bullets.get(i).pos.y < 0){
        bullets.remove(i);
        i --;
      }
    }
    for (int i = 0; i < asteroids.size(); i++){
      if(asteroids.get(i).pos.x == width + 100){
        asteroids.remove(i);
        i--;
      }
    }
    
    //check if game over
      //if game over then clear out asteroids and bullets
    if (play.lives <= 0){
      //clear out asteroids and bullets
      asteroids.clear();
      bullets.clear();
      
      //change gamestate
      states = State.GAME_OVER;
      
      //reset player
      play.lives = 3;
      play.clip = 6;
      play.pos.x = width / 2;
      play.pos.y = height / 2;
      play.rotation = 0;
      play.vel.x = 0;
      play.vel.y = 0;
    }
  }
}
  
//keypressed and keyreleased methods stolen/taken from ryan geyer's post in the tips and tricks discussion
void keyPressed(){
  if(key == 'A' || key == 'a'){
    LEFT_KEY_PRESSED = true;
  }

  if(key == 'W' || key == 'w'){
     UP_KEY_PRESSED = true;
  }
  
  if(key == 'D' || key == 'd'){
     RIGHT_KEY_PRESSED = true;
  }
  
  if(key == ' '){
    SPACE_KEY_PRESSED = true;
  }
}

//check if a key was released
void keyReleased(){
   if(key == 'A' || key == 'a'){
      LEFT_KEY_PRESSED = false;
   }

   if(key == 'W' || key == 'w'){
      UP_KEY_PRESSED = false;
   }
   
   if(key == 'D' || key == 'd'){
     RIGHT_KEY_PRESSED = false;
  }
}

void stop()
{
  bgSound.close();
  dieSound.close();
  fireSound.close();
  minim.stop();
  super.stop();
}